using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlateauMed.Infrastructure.Models;

namespace PlateauMed.API.Controllers
{
    public class BaseController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public Tuple<bool, ResponseModel<string>> GetModelStateError(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return new Tuple<bool, ResponseModel<string>>(true, null);

            var a = ModelState.Values.SelectMany(v => v.Errors);
            var reqErrors = a.Select(x => x.ErrorMessage);
            var reqErrorsString = string.Join(",", reqErrors);

            var executionResponse = new ResponseModel<string>
            {
                Message = reqErrorsString
            };

            return new Tuple<bool, ResponseModel<string>>(false, executionResponse);
        }
    }
}

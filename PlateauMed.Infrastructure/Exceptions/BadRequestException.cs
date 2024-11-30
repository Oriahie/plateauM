namespace PlateauMed.Infrastructure.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
            HttpStatusCode = System.Net.HttpStatusCode.BadRequest;
        }
    }
}

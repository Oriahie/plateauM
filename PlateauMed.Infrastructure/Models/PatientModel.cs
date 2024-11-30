using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Models
{
    public class PatientModel
    {
        public Guid PatientID { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string Name { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }
    }
}

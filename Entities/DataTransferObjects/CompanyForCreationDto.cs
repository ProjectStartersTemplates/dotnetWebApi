using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class CompanyForCreationDto
    {

        [Required(ErrorMessage = "Name is a required field.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Address is a required field.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Country is a required field.")]
        public string Country { get; set; }
        public IEnumerable<EmployeeForCreationDto> Employees { get; set; }

    }
}

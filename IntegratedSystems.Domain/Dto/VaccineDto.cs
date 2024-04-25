using IntegratedSystems.Domain.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Domain.Dto
{
    public class VaccineDto
    {
        public string? Manufacturer { get; set; }
        public Guid? PatientId { get; set; }
        public Guid? VaccinationCenter { get; set; }
        public List<string>? AllManufacturers { get; set; }
        public List<Patient>? AllPatients { get; set; }
    }
}

using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Interface
{
    public interface IVaccineService
    {
        public List<Vaccine> GetVaccines();
        public Vaccine AddVaccineToVaccinationCenter(VaccineDto vaccineDto);
        public List<Vaccine> GetVaccinesInVaccinationCenter(Guid centerId);

    }
}

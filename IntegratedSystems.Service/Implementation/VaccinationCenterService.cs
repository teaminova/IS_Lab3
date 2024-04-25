using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository.Interface;
using IntegratedSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Implementation
{
    public class VaccinationCenterService : IVaccinationCenterService
    {
        private readonly IRepository<VaccinationCenter> _vaccinationCenterRepository;

        public VaccinationCenterService(IRepository<VaccinationCenter> vaccinationCenterRepository)
        {
            _vaccinationCenterRepository = vaccinationCenterRepository;
        }

        public VaccinationCenter CreateNewVaccinationCenter(VaccinationCenter vaccinationCenter)
        {
            return _vaccinationCenterRepository.Insert(vaccinationCenter);
        }

        public VaccinationCenter DeleteVaccinationCenter(Guid id)
        {
            return _vaccinationCenterRepository.Delete(GetVaccinationCenterById(id));
        }

        public VaccinationCenter GetVaccinationCenterById(Guid? id)
        {
            return _vaccinationCenterRepository.Get(id);
        }

        public List<VaccinationCenter> GetVaccinationCenters()
        {
            return _vaccinationCenterRepository.GetAll().ToList();
        }

        public VaccinationCenter UpdateVaccinationCenter(VaccinationCenter vaccinationCenter)
        {
            return _vaccinationCenterRepository.Update(vaccinationCenter);
        }
    }
}

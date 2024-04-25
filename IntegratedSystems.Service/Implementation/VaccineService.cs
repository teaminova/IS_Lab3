using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Domain.Dto;
using IntegratedSystems.Repository.Interface;
using IntegratedSystems.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegratedSystems.Service.Implementation
{
    public class VaccineService : IVaccineService
    {
        private readonly IRepository<Vaccine> _vaccineRepository;
        private readonly IRepository<VaccinationCenter> _vaccinationCenterRepository;
        private readonly IRepository<Patient> _patientRepository;

        public VaccineService(IRepository<Vaccine> vaccineRepository, IRepository<VaccinationCenter> vaccinationCenterRepository, IRepository<Patient> patientRepository)
        {
            _vaccineRepository = vaccineRepository;
            _vaccinationCenterRepository = vaccinationCenterRepository;
            _patientRepository = patientRepository;
        }

        public Vaccine AddVaccineToVaccinationCenter(VaccineDto vaccineDto)
        {
            var center = _vaccinationCenterRepository.Get(vaccineDto.VaccinationCenter);

            if (center.MaxCapacity <= 0)
            {
                return null;
            }

            Vaccine vaccine = new Vaccine()
            {
                Id = Guid.NewGuid(),
                Manufacturer = vaccineDto.Manufacturer,
                Certificate = Guid.NewGuid(),
                DateTaken = DateTime.Now,
                PatientId = (Guid)vaccineDto.PatientId,
                PatientFor = _patientRepository.Get(vaccineDto.PatientId),
                VaccinationCenter = (Guid)vaccineDto.VaccinationCenter,
                Center = center
            };

            _vaccineRepository.Insert(vaccine);

            center.MaxCapacity = center.MaxCapacity - 1;
            _vaccinationCenterRepository.Update(center);

            return vaccine;
        }

        public List<Vaccine> GetVaccines()
        {
            return _vaccineRepository.GetAll().ToList();
        }

        public List<Vaccine> GetVaccinesInVaccinationCenter(Guid centerId)
        {
            return _vaccineRepository.GetAll().Where(v => v.VaccinationCenter == centerId).ToList();
        }
    }
}

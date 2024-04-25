using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository;
using IntegratedSystems.Service.Interface;
using IntegratedSystems.Domain.Dto;

namespace IntegratedSystems.Web.Controllers
{
    public class VaccinationCentersController : Controller
    {
        private readonly IVaccinationCenterService vaccinationCenterService;
        private readonly IVaccineService vaccineService;
        private readonly IPatientService patientService;

        public VaccinationCentersController(IVaccinationCenterService vaccinationCenterService, IVaccineService vaccineService, IPatientService patientService)
        {
            this.vaccinationCenterService = vaccinationCenterService;
            this.vaccineService = vaccineService;
            this.patientService = patientService;
        }

        // GET: VaccinationCenters
        public IActionResult Index()
        {
            return View(vaccinationCenterService.GetVaccinationCenters());
        }

        // GET: VaccinationCenters/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetVaccinationCenterById(id);

            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            ViewData["Vaccines"] = vaccineService.GetVaccinesInVaccinationCenter((Guid)id);

            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VaccinationCenters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            vaccinationCenter.Id = Guid.NewGuid();
            vaccinationCenterService.CreateNewVaccinationCenter(vaccinationCenter);

            return RedirectToAction("Index", "VaccinationCenters");
        }

        // GET: VaccinationCenters/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetVaccinationCenterById(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }
            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Address,MaxCapacity,Id")] VaccinationCenter vaccinationCenter)
        {
            if (id != vaccinationCenter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vaccinationCenterService.UpdateVaccinationCenter(vaccinationCenter);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VaccinationCenterExists(vaccinationCenter.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vaccinationCenter);
        }

        // GET: VaccinationCenters/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vaccinationCenter = vaccinationCenterService.GetVaccinationCenterById(id);
            if (vaccinationCenter == null)
            {
                return NotFound();
            }

            return View(vaccinationCenter);
        }

        // POST: VaccinationCenters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var vaccinationCenter = vaccinationCenterService.GetVaccinationCenterById(id);
            if (vaccinationCenter != null)
            {
                vaccinationCenterService.DeleteVaccinationCenter(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VaccinationCenterExists(Guid id)
        {
            return vaccinationCenterService.GetVaccinationCenterById(id) != null;
        }

        public IActionResult AddVaccine(Guid Id)
        {
            VaccineDto dto = new VaccineDto()
            {
                VaccinationCenter = Id,
                AllManufacturers = new List<string> { "M1", "M2", "M3", "M4", "M5" },
                AllPatients = patientService.GetPatients()
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult AddVaccineToVaccinationCenter(VaccineDto model)
        {
            var result = vaccineService.AddVaccineToVaccinationCenter(model);

            if (result == null)
            {
                return View("ErrorPage");
            }

            return RedirectToAction("Index", "VaccinationCenters");
        }
    }
}

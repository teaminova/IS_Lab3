﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IntegratedSystems.Domain.Domain_Models;
using IntegratedSystems.Repository;
using IntegratedSystems.Service.Interface;

namespace IntegratedSystems.Web.Controllers
{
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IVaccineService _vaccineService;

        public PatientsController(IPatientService patientService, IVaccineService vaccineService)
        {
            _patientService = patientService;
            _vaccineService = vaccineService;
        }

        // GET: Patients
        public IActionResult Index()
        {
            return View(_patientService.GetPatients());
        }

        // GET: Patients/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }

            ViewData["Vaccines"] = _vaccineService.GetVaccines().Where(v => v.PatientId == id).ToList();

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Embg,FirstName,LastName,PhoneNumber,Email,Id")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.Id = Guid.NewGuid();
                _patientService.CreateNewPatient(patient);
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }

        // GET: Patients/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Embg,FirstName,LastName,PhoneNumber,Email,Id")] Patient patient)
        {
            if (id != patient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _patientService.UpdatePatient(patient);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.Id))
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
            return View(patient);
        }

        // GET: Patients/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = _patientService.GetPatientById((Guid)id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var patient = _patientService.GetPatientById(id);
            if (patient != null)
            {
                _patientService.DeletePatient(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PatientExists(Guid id)
        {
            return _patientService.GetPatientById(id) != null;
        }
    }
}

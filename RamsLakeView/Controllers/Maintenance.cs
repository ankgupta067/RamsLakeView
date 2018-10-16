using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RamsLakeView.Interfaces;
using RamsLakeView.Models;
using RamsLakeView.Services;
using RamsLakeView.ViewModels;
using System;
using System.Threading.Tasks;

namespace RamsLakeView.Controllers
{
    [Controller]
    public class Maintenance : Controller
    {
        private IMaintenanceEntryService _service;

        public Maintenance(IMaintenanceEntryService service)
        {
            _service = service;
        }
       

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FailureMessage(FailureMessageEntryExistsViewModel entry)
        {
            return View(entry);
        }

        [HttpGet]
        public IActionResult ExceptionMessage(string message)
        {
            ViewBag.Message = message;
            return View(new { Message = message });
        }

        [HttpGet("Fetch")]
        [Authorize]
        public async Task<IActionResult> Fetch()
        {
            var entries = await _service.GetEntries();
            return View(entries);
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
        [HttpPost]
        public  async Task<IActionResult> Index(MaintenanceEntry entry)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                var result = await _service.AddEntry(entry);
                if (result){
                    return RedirectToAction(nameof(Success));
                }
                
            }
            catch (MaintenanceEntryExistsException ex)
            {
                return RedirectToAction(nameof(FailureMessage), new FailureMessageEntryExistsViewModel() {
                    Amount = ex.MaintenanceEntry.Amount, TransactionId= ex.MaintenanceEntry.TransactionId,
                    dateTime = ex.MaintenanceEntry.EntryDateTime });
            }
            catch(ApplicationException ex)
            {
                return RedirectToAction(nameof(ExceptionMessage), new { message = ex.Message });
            }
            return RedirectToAction(nameof(ExceptionMessage), new {message = "Something went wrong please retry" });
        }

    }
}

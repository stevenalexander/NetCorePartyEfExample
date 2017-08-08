using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyData;
using WebApplicationParty.Models;
using PartyData.Entities;
using System;

namespace WebApplicationParty.Controllers
{
    public class HomeController : Controller
    {
        private PartyDbContext _partyDbContext;

        public HomeController(PartyDbContext partyDbContext)
        {
            _partyDbContext = partyDbContext;
        }

        public async Task<IActionResult> Index()
        {
            var parties = await _partyDbContext.Parties.ToListAsync();
            var model = new HomeViewModel() { Parties = parties };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Parties = await _partyDbContext.Parties.ToListAsync();

                return View(model);
            }

            Party party = new Party
            {
                Name = model.Name,
                DateCreated = DateTime.UtcNow,
                DateLastModified = DateTime.UtcNow
            };
            _partyDbContext.Add(party);
            _partyDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

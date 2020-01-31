using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CI.Core.Domain.Models.IdentityServerConfiguration;
using CI.IServices;
using CI.UI.IdentityServer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CI.UI.IdentityServer.Controllers
{
    public class ClientController : Controller
    {
        private IService<Client> _clientService;
        private IMapper _mapper;

        public ClientController(IService<Client> clientService, IMapper mapper) {

            _clientService = clientService;
            _mapper = mapper;
        }
        // GET: Client
        public async Task<ActionResult> Index()
        {
            var clients = await _clientService.GetAllAsync();

            return View(_mapper.Map<List<ClientViewItemDTO>>(clients));
        }

        // GET: Client/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var client = await _clientService.FindByIdAsync(id);

            return View(_mapper.Map<ClientViewItemDTO>(client));
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Client/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
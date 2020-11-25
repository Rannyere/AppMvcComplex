using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IO.App.ViewModels;
using IO.Business.Interfaces;
using AutoMapper;
using IO.Business.Models;

namespace IO.App.Controllers
{
    public class ProvidersController : BaseController
    {
        private readonly IProviderRepository _providerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository,
                                   IAddressRepository addressRepository,
                                   IMapper mapper)
        {
            _providerRepository = providerRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        // GET: Providers
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.SearchAll()));
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var providerViewModel = await GetProviderAddress(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) return View(providerViewModel);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerRepository.Add(provider);

            return RedirectToAction("Index");
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetProviderProductsAddress(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }
            return View(providerViewModel);
        }

        // POST: Providers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(providerViewModel);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerRepository.Update(provider);

            return RedirectToAction("Index");
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var providerViewModel = await GetProviderAddress(id);

            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var providerViewModel = await GetProviderAddress(id);

            if (providerViewModel == null) return NotFound();

            await _providerRepository.Remove(id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetAddress(Guid id)
        {
            var provider = await GetProviderAddress(id);

            if (provider == null) return NotFound();

            return PartialView("_DetailsAddress", provider);
        }

        public async Task<ActionResult> UpdateAddress(Guid id)
        {
            var provider = await GetProviderAddress(id);

            if (provider == null) return NotFound();

            return PartialView("_UpdateAddress", new ProviderViewModel { Address = provider.Address });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress(ProviderViewModel providerViewModel)
        {
            ModelState.Remove("Name");
            ModelState.Remove("Document");

            if (!ModelState.IsValid) return PartialView("_UpdateAddress", providerViewModel);

            await _addressRepository.Update(_mapper.Map<Address>(providerViewModel.Address));

            var url = Url.Action("GetAddress", "Providers", new { id = providerViewModel.Address.ProviderId });
            return Json(new { success = true, url });
        }

        private async Task<ProviderViewModel> GetProviderAddress(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderAddress(id));
        }

        private async Task<ProviderViewModel> GetProviderProductsAddress(Guid id)
        {
            return _mapper.Map<ProviderViewModel>(await _providerRepository.GetProviderProductsAddress(id));
        }
    }
}

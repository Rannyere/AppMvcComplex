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
        private readonly IProviderService _providerService;
        private readonly IMapper _mapper;

        public ProvidersController(IProviderRepository providerRepository,
                                   IProviderService providerService,
                                   IMapper mapper,
                                   INotifier notifier) : base(notifier)
        {
            _providerRepository = providerRepository;
            _providerService = providerService;
            _mapper = mapper;
        }

        [Route("list-providers")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.SearchAll()));
        }

        [Route("details-provider/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var providerViewModel = await GetProviderAddress(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }

            return View(providerViewModel);
        }

        [Route("new-provider")]
        public IActionResult Create()
        {
            return View();
        }

        [Route("new-provider")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProviderViewModel providerViewModel)
        {
            if (!ModelState.IsValid) return View(providerViewModel);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerService.Add(provider);

            if (!ValidOperation()) return View(providerViewModel);

            TempData["Success"] = "Provider successfully created.";

            return RedirectToAction("Index");
        }

        [Route("edition-provider/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var providerViewModel = await GetProviderProductsAddress(id);

            if (providerViewModel == null)
            {
                return NotFound();
            }
            return View(providerViewModel);
        }

        [Route("edition-provider/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProviderViewModel providerViewModel)
        {
            if (id != providerViewModel.Id) return NotFound();

            if (!ModelState.IsValid) return View(providerViewModel);

            var provider = _mapper.Map<Provider>(providerViewModel);
            await _providerService.Update(provider);

            if (!ValidOperation()) return View(await GetProviderProductsAddress(id));

            TempData["Success"] = "Provider successfully update.";

            return RedirectToAction("Index");
        }

        [Route("delete-provider/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var providerViewModel = await GetProviderAddress(id);

            if (providerViewModel == null) return NotFound();

            return View(providerViewModel);
        }

        [Route("delete-provider/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var provider = await GetProviderAddress(id);

            if (provider == null) return NotFound();

            await _providerService.Remove(id);

            if (!ValidOperation()) return View(provider);

            TempData["Success"] = "Provider successfully delete.";

            return RedirectToAction("Index");
        }

        [Route("get-address-provider/{id:guid}")]
        public async Task<IActionResult> GetAddress(Guid id)
        {
            var provider = await GetProviderAddress(id);

            if (provider == null) return NotFound();

            return PartialView("_DetailsAddress", provider);
        }

        [Route("update-address-provider/{id:guid}")]
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

            await _providerService.UpdateAddress(_mapper.Map<Address>(providerViewModel.Address));

            if (!ValidOperation()) return PartialView("_UpdateAdress", providerViewModel);

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

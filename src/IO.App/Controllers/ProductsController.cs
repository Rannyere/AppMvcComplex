using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IO.App.ViewModels;
using AutoMapper;
using IO.Business.Interfaces;
using IO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using IO.App.Extensions;

namespace IO.App.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository,
                                  IProviderRepository providerRepository,
                                  IProductService productService,
                                  IMapper mapper,
                                  INotifier notifier) : base(notifier)
        {
            _productRepository = productRepository;
            _providerRepository = providerRepository;
            _productService = productService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [Route("list-products")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsProviders()));
        }

        [AllowAnonymous]
        [Route("details-product/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null)  return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product","Add")]
        [Route("new-product")]
        public async Task<IActionResult> Create()
        {
            var productViewModel = await PopularProviders(new ProductViewModel());

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Add")]
        [Route("new-product")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await PopularProviders(productViewModel);

            if (!ModelState.IsValid) return View(productViewModel);

            var imgPrefix = Guid.NewGuid() + "_";

            if(! await UploadArchive(productViewModel.ImageUpload, imgPrefix))
            {
                return View(productViewModel);
            }

            productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;

            await _productService.Add(_mapper.Map<Product>(productViewModel));

            if (!ValidOperation()) return View(productViewModel);

            TempData["Success"] = "Product successfully created.";

            return RedirectToAction("Index");

        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("edition-product/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null) return NotFound();

            return View(productViewModel);
        }

        [ClaimsAuthorize("Product", "Edit")]
        [Route("edition-product/{id:guid}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id) return NotFound();

            var productUpdate = await GetProduct(id);
            productViewModel.Provider = productUpdate.Provider;
            productViewModel.Image = productUpdate.Image;
            if (!ModelState.IsValid) return View(productViewModel);

            if (productViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";
                if (!await UploadArchive(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                productUpdate.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            productUpdate.Name = productViewModel.Name;
            productUpdate.Description = productViewModel.Description;
            productUpdate.Value = productViewModel.Value;
            productUpdate.Activ = productViewModel.Activ;

            await _productService.Update(_mapper.Map<Product>(productUpdate));

            if (!ValidOperation()) return View(productViewModel);

            return RedirectToAction("Index");
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("delete-product/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if (product == null) return NotFound();

            return View(product);
        }

        [ClaimsAuthorize("Product", "Delete")]
        [Route("delete-product/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);

            if(product == null) return NotFound();

            await _productService.Remove(id);

            if (!ValidOperation()) return View(product);

            TempData["Success"] = "Product successfully deleted.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductProvider(id));
            product.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.SearchAll());
            return product;
        }

        private async Task<ProductViewModel> PopularProviders(ProductViewModel product)
        {
            product.Providers = _mapper.Map<IEnumerable<ProviderViewModel>>(await _providerRepository.SearchAll());
            return product;
        }

        private async Task<bool> UploadArchive(IFormFile archive, string imgPrefix)
        {
            if (archive.Length <= 0) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + archive.FileName);

            if (System.IO.File.Exists(path))
            {
                ModelState.AddModelError(string.Empty, "A file with this name already exists!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await archive.CopyToAsync(stream);
            }

            return true;
        }
    }
}

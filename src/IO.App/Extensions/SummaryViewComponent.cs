using System;
using System.Threading.Tasks;
using IO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IO.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifier _notifier;

        public SummaryViewComponent(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notificacions = await Task.FromResult(_notifier.GetNotifications());

            notificacions.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Message));

            return View();
        }
    }
}

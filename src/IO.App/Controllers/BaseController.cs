using System;
using IO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IO.App.Controllers
{
    public class BaseController : Controller
    {
        private readonly INotifier _notifier;

        public BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool ValidOperation()
        {
            return !_notifier.HasNotifier();
        }
    }
}

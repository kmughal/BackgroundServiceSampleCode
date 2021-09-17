using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using background_queue.Models;

namespace background_queue.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BackgroundQueue _backgroundQueue;

        public HomeController(ILogger<HomeController> logger, BackgroundQueue backgroundQueue)
        {
            _logger = logger;
            _backgroundQueue = backgroundQueue;
        }

        [HttpGet]
        public async Task<string> SlowService()
        {
            _logger.LogInformation($"Startting process :{DateTime.UtcNow.ToString()}");
            _backgroundQueue.QueueBackgroundWork(async token =>
            {
                await Task.Delay(1000);
                _logger.LogInformation($"Completed process :{DateTime.UtcNow.ToString()}");
            });
            _logger.LogInformation($"Returning response :{DateTime.UtcNow.ToString()}");
            return await Task.FromResult("Response from background slow service!");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

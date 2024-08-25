using AppWithBlobStorage.Models;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AppWithBlobStorage.Services;

namespace AppWithBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IBlobService _blobService;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IBlobService blobService)
        {
            _logger = logger;
            _configuration = configuration;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                BlobContainerClient blobContainer = _blobService.GetContainerClient();
                await blobContainer.CreateIfNotExistsAsync();

                List<Uri> allBlobs = new List<Uri>();
                foreach (BlobItem blob in blobContainer.GetBlobs())
                {
                    if (blob.Properties.BlobType == BlobType.Block)
                    {
                        allBlobs.Add(blobContainer.GetBlobClient(blob.Name).Uri);
                    }
                     
                }

                return View(allBlobs);
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
    }
}

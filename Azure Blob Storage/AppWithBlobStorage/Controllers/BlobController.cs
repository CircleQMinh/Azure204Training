using AppWithBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AppWithBlobStorage.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                var fileName = GetRandomBlobName(file.FileName);
                var stream = file.OpenReadStream();
                await _blobService.UploadToBlobStorageAsync(stream, fileName);

                return RedirectToAction(controllerName:"Home",actionName: "Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                await _blobService.DeleteFromBlobStorageAsync(name);
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
            catch (Exception ex)
            {
                ViewData["message"] = ex.Message;
                ViewData["trace"] = ex.StackTrace;
                return View("Error");
            }
        }
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
    }
}

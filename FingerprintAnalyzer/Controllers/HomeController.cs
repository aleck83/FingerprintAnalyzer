using FingerprintAnalyzer.Helpers;
using FingerprintAnalyzer.Models;
using Microsoft.AspNetCore.Mvc;
using PatternRecognition.FingerprintRecognition.Core;
using SixLabors.ImageSharp.Formats.Bmp;
using System.Diagnostics;

namespace FingerprintAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FingerprintImageProvider imageProvider = new FingerprintImageProvider();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            var model = new FingerPrintViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        var fileStream = file.OpenReadStream();
                        using var imageUploaded = Image.Load(fileStream);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imageUploaded.Save(ms, new BmpEncoder());                            
                            model.OriginImageData = ms.ToArray();
                        }
                        model.BlackedImageData = FingerprintHelper.GetBWImage(model.OriginImageData);
                        model.SkeletonImageData = FingerprintHelper.GetSkeletonImage(model.BlackedImageData);

                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                    _logger.LogError(ex, "Error while file processing.");
                }
            }

            return View("Index", model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using MyWebsite.Models;
using System.Diagnostics;
using System.Text;

namespace MyWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment env;

        // Constructor with dependency injection for logger and hosting environment
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            this.env = env;
        }

        // Action to display the home page
        public IActionResult Index()
        {
            return View();
        }

        // Action to display the tutorials page
        public IActionResult Tutorials()
        {
            return View();
        }

        // Action to display the about me page
        public IActionResult Autobiography()
        {
            return View();
        }

        // Action to display the gallery page
        public IActionResult Gallery()
        {
            return View();
        }

        // Action to display the textbook page
        public IActionResult Textbook()
        {
            return View();
        }

        // Action to handle errors and display the Error page
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Action to provide a sample file download
        public IActionResult Download()
        {
            byte[] bytes = Encoding.UTF8.GetBytes("My first file");
            return File(bytes, "text/plain", "file.txt");
        }

        // Action to stream video content for playback
        public IActionResult Video(string videoFileName)
        {
            // Get the physical path of the video file from the web root
            var filePath = env.WebRootFileProvider.GetFileInfo(videoFileName)?.PhysicalPath;

            // Check if the file exists
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                // Log the file path (optional)
                _logger.LogWarning($"Video file not found: {filePath}");
                // Return a 404 Not Found response
                return NotFound();
            }

            // Return the video file as a response
            return new FileStreamResult(System.IO.File.OpenRead(filePath), "video/mp4")
            {
                FileDownloadName = videoFileName
            };
        }

        // Action to provide a downloadable video file
        public IActionResult DownloadVideo(string videoFileName)
        {
            // Get the physical path of the video file from the web root
            var filePath = env.WebRootFileProvider.GetFileInfo(videoFileName)?.PhysicalPath;

            // Check if the file exists
            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                // Handle the case when the file is not found
                return NotFound();
            }

            // Return the video file as a downloadable response
            return File(System.IO.File.OpenRead(filePath), "video/mp4", videoFileName);
        }
    }
}

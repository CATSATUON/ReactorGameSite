using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Models;

namespace ReactorGame.Pages
{
    public class VideoModel : PageModel
    {
        public List<string> VideoFiles { get; set; } = new List<string>();

        public bool AllowVideoSkipping { get; set; }

        private const string JsonFilePath = "GameSettings/gameSettings.json";

        private const string VideoDirectory = "wwwroot/video";

        private const string VideoAbsolutePath = "https://reactorgame.azurewebsites.net/video/";

        public void OnGet()
        {
            LoadVideos();

            // Get allow skipping setting
            ScenarioSet settings = ScenarioSet.LoadSettingsFromFile(JsonFilePath);
            AllowVideoSkipping = settings.AllowVideoSkipping;
        }

        public void LoadVideos()
        {
            // Get a list of all the video files in the video folder
            string videoFolder = Path.Combine(Environment.CurrentDirectory, VideoDirectory);
            if (Directory.Exists(videoFolder))
            {
                VideoFiles = Directory.GetFiles(videoFolder).Select(Path.GetFileName).ToList();
            }
        }

        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            // Check file is not empty, not too large, and is an MP4 video
            const long maxFileSize = 500 * 1024 * 1024; // 500 MB

            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "No file selected";
                return RedirectToPage();
            }

            if (file.Length > maxFileSize)
            {
                TempData["Error"] = "File is too large";
                return RedirectToPage();
            }

            if (!file.ContentType.Equals("video/mp4", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "File must be an MP4 video";
                return RedirectToPage();
            }

            // Save the video file to the video folder
            string videoFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot", "video");
            string filePath = Path.Combine(videoFolder, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Set the videourls in the scenario set
            SaveVideos();

            return RedirectToPage();
        }

        // Handler for deleting a video file
        public IActionResult OnPostDelete(string file)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, VideoDirectory, file);

            // Check if the file exists
            if (System.IO.File.Exists(filePath))
            {
                // Delete the file
                System.IO.File.Delete(filePath);

                TempData["Error"] = "Video deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Video not found.";
            }

            // Reload and save the videos
            SaveVideos();

            // Redirect to the GET handler to refresh the video list
            return RedirectToPage();
        }

        private void SaveVideos()
        {
            // Load videos to ensure the list is up to date
            LoadVideos();

            // Save the video files to the scenario set
            ScenarioSet settings = ScenarioSet.LoadSettingsFromFile(JsonFilePath);
            settings.VideoUrls = VideoFiles.Select(f => VideoAbsolutePath + f).ToList();
            settings.SaveSettings(JsonFilePath);
        }

        public IActionResult OnPostToggleSkip([FromBody] CheckboxInput input)
        {
            bool skipVideoAllowed = input.AllowSkip;

            ScenarioSet settings = ScenarioSet.LoadSettingsFromFile(JsonFilePath);
            settings.AllowVideoSkipping = skipVideoAllowed;
            settings.SaveSettings(JsonFilePath);

            return new JsonResult(new { success = true, allowSkip = skipVideoAllowed });
        }

        public class CheckboxInput
        {
            public bool AllowSkip { get; set; }
        }
    }
}

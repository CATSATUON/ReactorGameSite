using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorBuild.Pages;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ReactorGame.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILogger<SettingsModel> _logger;
        // Make the path not public
        private const string JsonFilePath = "GameSettings/gameSettings.json";

        [BindProperty]
        [AllowNull]
        public GameSettings GameSettings { get; set; }


        public SettingsModel(ILogger<SettingsModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Load the settings from a file
            Console.WriteLine("Loading settings from file");
            GameSettings.LoadSettings(JsonFilePath);

            if (GameSettings == null)
            {
                GameSettings = new GameSettings();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            // Get form input
            GameSettings = new GameSettings();
            TryUpdateModelAsync(GameSettings);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Console.WriteLine("Model state is valid");
            GameSettings.SaveSettings(JsonFilePath);

            return RedirectToPage("/Confirmation");
        }
    }
}
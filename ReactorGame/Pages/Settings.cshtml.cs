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
            GameSettings = GameSettings.LoadSettings(JsonFilePath);

            if (GameSettings == null)
            {
                GameSettings = new GameSettings();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            GameSettings.SaveSettings(JsonFilePath);

            return RedirectToPage("/Confirmation");
        }

        public IActionResult OnGetJson()
        {
            GameSettings = GameSettings.LoadSettings(JsonFilePath);

            if (GameSettings == null)
            {
                GameSettings = new GameSettings();
            }

            return new JsonResult(GameSettings);
        }
    }
}
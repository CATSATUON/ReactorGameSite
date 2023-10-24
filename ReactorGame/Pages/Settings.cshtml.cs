using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Models;

namespace ReactorGame.Pages
{
    public class SettingsModel : PageModel
    {
        private readonly ILogger<ScenarioModel> _logger;

        private const string JsonFilePath = "GameSettings/gameSettings.json";

        [BindProperty]
        public ScenarioSet Settings { get; set; }

        public SettingsModel(ILogger<ScenarioModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            try {
                Settings = TryToLoadSettings();
            } catch {
                Settings = new ScenarioSet();
                Settings.SaveSettings(JsonFilePath);
            }

            return Page();
        }

        public IActionResult OnGetJson()
        {
            try {
                Settings = TryToLoadSettings();
            } catch {
                return new JsonResult(new { error = "Could not load settings" });
            }

            return new JsonResult(Settings);
        }

        private ScenarioSet TryToLoadSettings()
        {
            // Load the settings from a file
            ScenarioSet settings = ScenarioSet.LoadSettings(JsonFilePath);

            return settings;
        }
    }
}

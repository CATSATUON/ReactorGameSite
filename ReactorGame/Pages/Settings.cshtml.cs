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
            Settings = TryToLoadSettings();
        }

        public IActionResult OnGet()
        {
            Settings = TryToLoadSettings();

            return Page();
        }

        public IActionResult OnGetJson()
        {
            Settings = TryToLoadSettings();

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

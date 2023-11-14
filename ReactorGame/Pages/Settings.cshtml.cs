using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                Settings = TryToLoadSettings();
            }
            catch
            {
                Settings = new ScenarioSet();
                Settings.SaveSettings(JsonFilePath);
            }

            return Page();
        }

        public IActionResult OnGetJson()
        {
            try
            {
                Settings = TryToLoadSettings();
            }
            catch
            {
                return new JsonResult(new { error = "Could not load settings" });
            }

            return new JsonResult(Settings);
        }

        private ScenarioSet TryToLoadSettings()
        {
            // Load the settings from a file
            ScenarioSet settings = ScenarioSet.LoadSettingsFromFile(JsonFilePath);

            return settings;
        }

        public IActionResult OnPostReplace([FromBody] ScenarioSet settings)
        {
            if (settings == null)
            {
                return new BadRequestObjectResult("Settings are not valid");
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Settings are not valid");
            }

            try
            {
                settings.SaveSettings(JsonFilePath);
            }
            catch
            {
                return new BadRequestObjectResult("Could not save settings");
            }

            return new OkObjectResult("Settings saved");
        }

        public IActionResult OnPostAppend([FromBody] ScenarioSet settings)
        {
            if (settings == null)
            {
                return new BadRequestObjectResult("Settings are not valid");
            }

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Settings are not valid");
            }

            try
            {
                ScenarioSet currentSettings = TryToLoadSettings();
                currentSettings.Append(settings);
                currentSettings.SaveSettings(JsonFilePath);
                Settings = currentSettings;
            }
            catch
            {
                return new BadRequestObjectResult("Could not save settings");
            }

            return new OkObjectResult("Settings saved");
        }

        public IActionResult OnPostCreateScenario()
        {
            // Add a new empty scenario to the settings
            try
            {
                ScenarioSet currentSettings = TryToLoadSettings();
                currentSettings.Scenarios.Add(new GameScenario());
                currentSettings.SaveSettings(JsonFilePath);
                Settings = currentSettings;
            }
            catch
            {
                return new BadRequestObjectResult("Could not save settings");
            }

            return new OkObjectResult("Scenario created");
        }

        public IActionResult OnPostDeleteScenario(int scenarioId)
        {
            Console.WriteLine($"Deleting scenario {scenarioId}");
            // Check if the scenario exists
            try
            {
                ScenarioSet currentSettings = TryToLoadSettings();
                if (scenarioId < 0 || scenarioId >= currentSettings.Scenarios.Count)
                {
                    return new BadRequestObjectResult("Scenario does not exist");
                }

                // Remove the scenario
                currentSettings.Scenarios.RemoveAt(scenarioId);
                currentSettings.SaveSettings(JsonFilePath);
                Settings = currentSettings;
            }
            catch
            {
                return new BadRequestObjectResult("Could not save settings");
            }

            return new OkObjectResult("Scenario deleted");
        }
    }
}

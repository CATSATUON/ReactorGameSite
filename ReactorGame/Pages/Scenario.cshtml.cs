using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ReactorGame.Pages
{
    public class ScenarioModel : PageModel
    {
        private readonly ILogger<ScenarioModel> _logger;

        private const string JsonFilePath = "GameSettings/gameSettings.json";

        [BindProperty]
        [AllowNull]
        public GameScenario Scenario { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Flow temperatures are required")]
        [MinLength(1, ErrorMessage = "Flow temperatures must have at least one entry")]
        public List<FlowTemperature> FlowTemperatures { get; set; }


        public ScenarioModel(ILogger<ScenarioModel> logger)
        {
            _logger = logger;
            FlowTemperatures = new List<FlowTemperature>();
        }

        public IActionResult OnGet(int scenarioId)
        {
            TryToLoadScenario(scenarioId);

            return Page();
        }

        private void TryToLoadScenario(int scenarioId)
        {
            ScenarioSet? gameSettings = ScenarioSet.LoadSettings(JsonFilePath);

            if (gameSettings == null)
            {
                return;
            }

            // Set the scenario to the first one in the list
            Scenario = gameSettings.Scenarios[scenarioId];
            FlowTemperatures = Scenario.FlowTemperatures.Select(
                x => new FlowTemperature { Flow = x.Key, Temperature = x.Value }).ToList();
        }

        public IActionResult OnPost(int scenarioId)
        {
            Console.WriteLine(Scenario.ScenarioName);
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set flow temperatures from the list
            Scenario.FlowTemperatures.Clear();
            Scenario.FlowTemperatures = FlowTemperatures.ToDictionary(x => x.Flow, x => x.Temperature);
            // Load the settings and replace the scenario
            ScenarioSet gameSettings = ScenarioSet.LoadSettings(JsonFilePath);
            gameSettings.Scenarios[scenarioId] = Scenario;
            
            // Save the settings back to the file
            gameSettings.SaveSettings(JsonFilePath);

            return RedirectToPage("/Confirmation");
        }

        public IActionResult OnGetJson(int scenarioId)
        {
            TryToLoadScenario(scenarioId);

            return new JsonResult(Scenario);
        }
    }

    public class FlowTemperature
    {
        [Range(0, 1000, ErrorMessage = "Flow must be positive")]
        public int Flow { get; set; }
        public int Temperature { get; set; }
    }
}
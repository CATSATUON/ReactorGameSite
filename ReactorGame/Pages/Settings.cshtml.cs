using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Pages;
using ReactorGame.Models;
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
        [BindProperty]
        [AllowNull]
        [Required(ErrorMessage = "Flow temperatures are required")]
        [MinLength(1, ErrorMessage = "Flow temperatures must have at least one entry")]
        public List<FlowTemperature> FlowTemperatures { get; set; }


        public SettingsModel(ILogger<SettingsModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Load the settings from a file
            GameSettings = GameSettings.LoadSettings(JsonFilePath);
            FlowTemperatures = GameSettings.FlowTemperatures.Select(
                x => new FlowTemperature { Flow = x.Key, Temperature = x.Value }).ToList();

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

            // Set flow temperatures from the list
            GameSettings.FlowTemperatures.Clear();
            GameSettings.FlowTemperatures = FlowTemperatures.ToDictionary(x => x.Flow, x => x.Temperature);
            // Save the settings to the file
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

    public class FlowTemperature
    {
        [Range(0, 1000, ErrorMessage = "Flow must be positive")]
        public int Flow { get; set; }
        public int Temperature { get; set; }
    }
}
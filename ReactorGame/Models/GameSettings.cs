using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ReactorBuild.Pages
{

    [Serializable]
    public class GameSettings
    {
        [JsonProperty("cycleDuration")]
        [Required(ErrorMessage = "Cycle duration is required")]
        [Range(1, 1000, ErrorMessage = "Cycle duration must be 1 or greater")]
        public int CycleDuration { get; set; }

        [JsonProperty("breakTankOnOverflow")]
        public bool BreakTankOnOverflow { get; set; }

        [JsonProperty("flowTemperatures")]
        public Dictionary<int, int> FlowTemperatures { get; set; }

        [JsonProperty("tanks")]
        public Dictionary<string, TankSettings> Tanks { get; set; }

        [JsonProperty("targetTemperature")]
        [Required(ErrorMessage = "Target temperature is required")]
        [Range(0, 1000, ErrorMessage = "Target temperature must be positive")]
        public int TargetTemperature { get; set; }

        [JsonProperty("valves")]
        public Dictionary<string, ValveSettings> Valves { get; set; }

        public GameSettings()
        {
            CycleDuration = 120;
            BreakTankOnOverflow = true;
            FlowTemperatures = new Dictionary<int, int>();
            Tanks = new Dictionary<string, TankSettings>();
            TargetTemperature = 100;
            Valves = new Dictionary<string, ValveSettings>();
        }

        public static GameSettings LoadSettings(string fname)
        {
            if (!File.Exists(fname))
            {
                Console.WriteLine("File does not exist");
                return null;
            }
            string json = File.ReadAllText(fname);
            Console.WriteLine("JSON: " + json);
            GameSettings? gameSettings = JsonConvert.DeserializeObject<GameSettings>(json);
            if (gameSettings == null)
            {
                Console.WriteLine("Deserialization failed");
                return null;
            }
            return gameSettings;
        }

        public void SaveSettings(string fname)
        {
            string json = JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(fname, json);
        }
    }

    [Serializable]
    public class TankSettings
    {
        [JsonProperty("capacity")]
        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 20, ErrorMessage = "Capacity must be between 1 and 20")]
        public int Capacity { get; set; }

        [JsonProperty("startLevel")]
        [Required(ErrorMessage = "Start level is required")]
        [Range(0, 20, ErrorMessage = "Start level must be between 0 and 20")]
        public int StartLevel { get; set; }
    }

    [Serializable]
    public class ValveSettings
    {
        [JsonProperty("maxFlowDisplay")]
        [Required(ErrorMessage = "Max flow display is required")]
        [Range(1, 20, ErrorMessage = "Max flow display must be between 1 and 20")]
        public int MaxFlowDisplay { get; set; }

        [JsonProperty("flowStepSize")]
        [Required(ErrorMessage = "Flow step size is required")]
        [Range(1, 20, ErrorMessage = "Flow step size must be between 1 and 20")]
        public int FlowStepSize { get; set; }

        [JsonProperty("flowRatePerStep")]
        [Required(ErrorMessage = "Flow rate per step is required")]
        [Range(0, 1, ErrorMessage = "Flow rate per step must be between 0 and 1")]
        public float FlowRatePerStep { get; set; }
        
        [JsonProperty("isBroken")]
        public bool IsBroken { get; set; }
    }
}

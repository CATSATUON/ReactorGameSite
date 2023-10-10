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
        [Range(1, 1000, ErrorMessage = "Cycle duration must be positive")]
        public int CycleDuration { get; set; }

        [JsonProperty("breakTankOnOverflow")]
        public bool BreakTankOnOverflow { get; set; }

        [JsonProperty("flowTemperatures")]
        public Dictionary<int, int> FlowTemperatures { get; set; }

        [JsonProperty("tanks")]
        public Dictionary<string, TankSettings> Tanks { get; set; }

        [JsonProperty("targetTemperature")]
        [Required(ErrorMessage = "Target temperature is required")]
        [Range(1, 1000, ErrorMessage = "Target temperature must be positive")]
        public int TargetTemperature { get; set; }

        [JsonProperty("valves")]
        public Dictionary<string, ValveSettings> Valves { get; set; }

        public GameSettings()
        {
            this.CycleDuration = 120;
            this.BreakTankOnOverflow = true;
            this.FlowTemperatures = new Dictionary<int, int>();
            this.Tanks = new Dictionary<string, TankSettings>();
            this.TargetTemperature = 100;
            this.Valves = new Dictionary<string, ValveSettings>();
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
        public int Capacity { get; set; }
        [JsonProperty("startLevel")]
        public int StartLevel { get; set; }
    }

    [Serializable]
    public class ValveSettings
    {
        [JsonProperty("maxFlowDisplay")]
        public int MaxFlowDisplay { get; set; }
        [JsonProperty("flowStepSize")]
        public int FlowStepSize { get; set; }
        [JsonProperty("flowRatePerStep")]
        public float FlowRatePerStep { get; set; }
        [JsonProperty("isBroken")]
        public bool IsBroken { get; set; }
    }
}

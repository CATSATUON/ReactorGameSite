﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ReactorGame.Models
{
    [Serializable]
    public class GameScenario
    {
        private const int TankCount = 4;
        private const int ValveCount = 13;

        [JsonProperty("scenarioName")]
        [Required(ErrorMessage = "Scenario name is required")]
        public string ScenarioName { get; set; }

        [JsonProperty("totalCycles")]
        [Required(ErrorMessage = "Total cycles is required")]
        [Range(1, 1000, ErrorMessage = "Total cycles must be 1 or greater")]
        public int TotalCycles { get; set; }

        [JsonProperty("cycleDuration")]
        [Required(ErrorMessage = "Cycle duration is required")]
        [Range(1, 1000, ErrorMessage = "Cycle duration must be 1 or greater")]
        public int CycleDuration { get; set; }

        [JsonProperty("breakTankOnOverflow")]
        public bool BreakTankOnOverflow { get; set; }

        [JsonProperty("showOverflowWarning")]
        public bool ShowOverflowWarning { get; set; }

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

        public GameScenario()
        {
            ScenarioName = "New Scenario";
            TargetTemperature = 120;
            CycleDuration = 25;
            TotalCycles = 10;
            BreakTankOnOverflow = true;
            ShowOverflowWarning = true;
            FlowTemperatures = new Dictionary<int, int>();
            Tanks = new Dictionary<string, TankSettings>();
            Valves = new Dictionary<string, ValveSettings>();

            // Add the required tanks
            for (int i = 1; i <= TankCount; i++)
            {
                Tanks.Add($"tank{i}", new TankSettings());
            }

            // Add the required valves
            for (int i = 1; i <= ValveCount; i++)
            {
                Valves.Add($"valve{i}", new ValveSettings());
            }

            // Add some flow temperatures
            FlowTemperatures.Add(0, 200);
            FlowTemperatures.Add(4, 100);
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

        public TankSettings()
        {
            Capacity = 3;
            StartLevel = 0;
        }
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

        public ValveSettings()
        {
            MaxFlowDisplay = 2;
            FlowStepSize = 1;
            FlowRatePerStep = 1f;
            IsBroken = false;
        }
    }
}

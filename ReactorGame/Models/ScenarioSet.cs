using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ReactorGame.Models
{
    [Serializable]
    public class ScenarioSet
    {
        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("scenarios")]
        public List<GameScenario> Scenarios { get; set; }

        [Required]
        [JsonProperty("videoUrls")]
        public List<string> VideoUrls { get; set; }

        [Required]
        [JsonProperty("allowVideoSkipping")]
        public bool AllowVideoSkipping { get; set; }

        public ScenarioSet()
        {
            Scenarios = new List<GameScenario>();
            VideoUrls = new List<string>();
            Name = "New Settings";
        }

        public static ScenarioSet LoadSettingsFromFile(string fname)
        {
            if (!File.Exists(fname))
            {
                Console.WriteLine("File does not exist");
                throw new FileNotFoundException();
            }
            string json = File.ReadAllText(fname);
            return LoadSettingsFromJson(json);
        }

        public static ScenarioSet LoadSettingsFromJson(string json)
        {
            ScenarioSet? gameSettings = JsonConvert.DeserializeObject<ScenarioSet>(json);
            if (gameSettings == null)
            {
                Console.WriteLine("Deserialization failed");
                throw new JsonSerializationException();
            }
            return gameSettings;
        }   

        public void SaveSettings(string fname)
        {
            EnsureUniqueNames();

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(fname, json);
        }

        public void EnsureUniqueNames()
        {
            var nameCounts = new Dictionary<string, int>();

            foreach (var scenario in Scenarios)
            {
                string originalName = scenario.ScenarioName;
                while (nameCounts.ContainsKey(scenario.ScenarioName))
                {
                    nameCounts[originalName]++;
                    scenario.ScenarioName = originalName + $" ({nameCounts[originalName]})";
                }

                if (!nameCounts.ContainsKey(originalName))
                {
                    nameCounts[originalName] = 0;
                }

                if (!nameCounts.ContainsKey(scenario.ScenarioName))
                {
                    nameCounts[scenario.ScenarioName] = 0;
                }
            }
        }

        public void Append(ScenarioSet otherScenarios)
        {
            for (int i = 0; i < otherScenarios.Scenarios.Count; i++)
            {
                Scenarios.Add(otherScenarios.Scenarios[i]);
            }
        }
    }
}

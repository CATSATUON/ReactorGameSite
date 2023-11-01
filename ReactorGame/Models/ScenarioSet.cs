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

        public ScenarioSet()
        {
            Scenarios = new List<GameScenario>();
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
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(fname, json);
        }
    }
}

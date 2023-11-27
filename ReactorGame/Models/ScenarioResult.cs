using Newtonsoft.Json;
using System.Text;

namespace ReactorGame.Models
{
    [Serializable]
    public class ScenarioResult
    {
        [JsonProperty("records")]
        public List<CycleResult> Records;

        [JsonProperty("scenarioName")]
        public string ScenarioName;

        public ScenarioResult()
        {
            Records = new List<CycleResult>();
            ScenarioName = "Default";

        }

        public ScenarioResult(List<CycleResult> records, string name)
        {
            Records = records;
            ScenarioName = name;
        }

        public void AddRecord(CycleResult record)
        {
            Records.Add(record);
        }

        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();

            // Write the header
            sb.AppendLine(CycleResult.GetHeader());
            foreach (CycleResult record in Records)
            {
                sb.AppendLine(record.ToCSV());
            }
            return sb.ToString();
        }
    }
}

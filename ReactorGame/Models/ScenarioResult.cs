using Newtonsoft.Json;
using System.Text;

namespace ReactorGame.Models
{
    [Serializable]
    public class ScenarioResult
    {
        [JsonProperty("records")]
        private List<CycleResult> _records;

        public ScenarioResult()
        {
            _records = new List<CycleResult>();
        }

        public ScenarioResult(List<CycleResult> records)
        {
            _records = records;
        }

        public void AddRecord(CycleResult record)
        {
            _records.Add(record);
        }

        public string ToCSV()
        {
            StringBuilder sb = new StringBuilder();

            // Write the header
            sb.AppendLine(CycleResult.GetHeader());
            foreach (CycleResult record in _records)
            {
                sb.AppendLine(record.ToCSV());
            }
            return sb.ToString();
        }
    }
}

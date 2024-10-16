using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO.Compression;
using static NuGet.Packaging.PackagingConstants;

namespace ReactorGame.Pages
{
    [IgnoreAntiforgeryToken]
    public class ResultsModel : PageModel
    {
        private const string ResultsFolder = "GameResults";

        public async Task<IActionResult> OnPostAsync()
        {
            Console.Out.WriteLine("Received POST request to /Results");

            using StreamReader reader = new StreamReader(Request.Body);
            string body = await reader.ReadToEndAsync();

            GameResult? gameResult = JsonConvert.DeserializeObject<GameResult>(body);

            if (gameResult == null)
            {
                return BadRequest("Invalid JSON");
            }

            // Save the results to a file
            string fName = gameResult.EndTimestamp + ".csv";

            fName = string.Concat(fName.Split(Path.GetInvalidFileNameChars()));

            string folder = Path.Combine(Environment.CurrentDirectory, ResultsFolder);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            using StreamWriter file = new StreamWriter(Path.Combine(folder, fName));
            await file.WriteLineAsync(ResultToCSV(gameResult));
            
            return new OkResult();
        }

        private static string ResultToCSV(GameResult result)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("User ID:, " + result.UserId);
            sb.AppendLine("Game name:, " + result.GameName);
            sb.AppendLine("Completion code:, " + result.CompletionCode);
            sb.AppendLine("Video shown:, " + result.VideoShown);
            sb.AppendLine("Start timestamp:, " + result.StartTimestamp);
            sb.AppendLine("End timestamp:, " + result.EndTimestamp);
            sb.AppendLine();

            // Add each of the results
            foreach (ScenarioResult scenarioResult in result.ResultList)
            {
                sb.AppendLine("Scenario name:, " + scenarioResult.ScenarioName);
                sb.AppendLine(scenarioResult.ToCSV());
            }

            return sb.ToString();
        }

        public IActionResult OnGetZip()
        {
            string tempPath = Path.Combine(Environment.CurrentDirectory, "Temp");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            string zipName = Path.Combine(tempPath, "Results.zip");
            if (System.IO.File.Exists(zipName))
            {
                System.IO.File.Delete(zipName);
            }

            string resultsPath = Path.Combine(Environment.CurrentDirectory, ResultsFolder);
            if (!Directory.Exists(resultsPath))
            {
                Directory.CreateDirectory(resultsPath);
            }

            ZipFile.CreateFromDirectory(resultsPath, zipName);
            byte[] bytes = System.IO.File.ReadAllBytes(zipName);
            return File(bytes, "application/zip", "Results.zip");
        }

        public IActionResult OnPostDeleteAll()
        {
            // Delete the contents of the results folder
            foreach (string file in Directory.GetFiles(ResultsFolder))
            {
                System.IO.File.Delete(file);
            }

            return RedirectToPage("Results");
        }
    }
}

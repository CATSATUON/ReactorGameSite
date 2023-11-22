using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReactorGame.Models;
using Newtonsoft.Json;

namespace ReactorGame.Pages
{
    public class ResultsModel : PageModel
    {
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

            Console.WriteLine(gameResult.ResultList[0].ToCSV());

            return new OkResult();
        }
    }
}

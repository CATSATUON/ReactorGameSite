using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ReactorGame.Pages
{
    public class CodesModel : PageModel
    {
        private const string CodesFilePath = "GameSettings/completionCodes.json";
        [BindProperty]
        public List<string> Codes { get; set; }

        public CodesModel()
        {
            Codes = new List<string>();
        }

        public void OnGet()
        {
            try
            {
                // get codes from codes.json
                LoadCodes();
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public IActionResult OnGetRandomCode()
        {
            LoadCodes();

            if (Codes.Count == 0)
            {
                return new JsonResult(new { code = "000000" });
            }

            Random random = new Random();
            int index = random.Next(Codes.Count);
            string selectedCode = Codes[index];
            Codes.RemoveAt(index);

            SaveCodes();

            return new JsonResult(new { code = selectedCode });
        }


        public IActionResult OnPostUploadCodes(string codes)
        {
            if (string.IsNullOrEmpty(codes))
            {
                TempData["Message"] = "No codes were provided";
                return Page();
            }

            Codes = codes.Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(code => code.Trim())
                .ToList();
            
            // Write the codes to the file
            SaveCodes();

            return RedirectToPage();
        }

        public void LoadCodes()
        {
            // Ensure the file exists
            if (!System.IO.File.Exists(CodesFilePath))
            {
                Console.WriteLine("File does not exist");
                throw new FileNotFoundException();
            }

            // Read the codes from the file
            string json = System.IO.File.ReadAllText(CodesFilePath);
            List<string>? readCodes = JsonConvert.DeserializeObject<List<string>>(json);
            if (readCodes == null)
            {
                Console.WriteLine("Deserialization failed");
                throw new JsonSerializationException();
            }

            Codes = readCodes;
        }

        public void SaveCodes()
        {
            // Save the codes to the file
            string json = JsonConvert.SerializeObject(Codes, Formatting.Indented);
            System.IO.File.WriteAllText(CodesFilePath, json);
        }
    }
}

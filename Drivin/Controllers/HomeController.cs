using System.Diagnostics;
using System.Text;

using Microsoft.AspNetCore.Mvc;

using Drivin.Models;
using Drivin.Services;

namespace Drivin.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    /// <summary>
    /// Receives one file with emails filters and saves it in subfiles with max 5 emails each
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("/EmailsFile")]
    public async Task<IActionResult> UploadEmailsFile(
        FileUploadViewModel model
    )
    {
        try
        {
            // Validations
            if (model.File == null || model.File.Length == 0)
                throw new Exception("Arquivo não enviado.");

            // Only text files are accepted
            string contentType = model.File.ContentType;
            if (!(contentType.StartsWith("text/") || contentType == "application/octet-stream"))
                throw new Exception("Somente arquivos de texto são permitidos.");

            // Process file
            var service = new ProcessEmailFilesService(emailsPerFile: 5);
            var emailsSubfiles = await service.ProcessEmailsFile(model.File);

            return Ok(emailsSubfiles);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Returns a generated subfile with its content
    /// </summary>
    /// <returns></returns>
    [HttpGet("/EmailsSubfile")]
    public IActionResult DownloadEmailsSubFile(
        [FromQuery] string fileName,
        [FromQuery] List<string> emails
    )
    {
        try
        {
            // Validations
            if (emails is null || emails.Count == 0)
                throw new Exception("Lista de emails é obrigatória.");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Nome do arquivo é obrigatório.");

            // Turns list into a text file
            var content = string.Join("\r\n", emails);
            var byteArray = Encoding.UTF8.GetBytes(content);
            var stream = new MemoryStream(byteArray);

            // Download
            return File(stream, "text/plain", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

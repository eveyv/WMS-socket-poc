using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class TextFileController : ControllerBase
{
    [HttpGet("test.txt")]
    public async Task<IActionResult> GetTestFile()
    {
        try
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot","test", "test.txt");
            var fileContent = await System.IO.File.ReadAllTextAsync(filePath);
            Console.Write(fileContent,  filePath); // debug docPath
            return Ok(fileContent);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error - aint work : {ex.Message}");
        }
    }
}

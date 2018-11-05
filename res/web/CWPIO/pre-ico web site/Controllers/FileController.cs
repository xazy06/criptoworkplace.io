using Microsoft.AspNetCore.Mvc;
using pre_ico_web_site.Services;
using System.IO;
using System.Threading.Tasks;

namespace pre_ico_web_site.Controllers
{
    [Route("File")]
    public class FileController : Controller
    {
        private readonly IFileRepository _files;
        public FileController(IFileRepository files)
        {
            _files = files;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Download([FromRoute]string fileName)
        {
            var stream = new MemoryStream();

            var mime = await _files.GetFileByNameAsync(fileName, stream);

            if (stream == null || stream.Length == 0)
            {
                return NotFound();
            }
            stream.Seek(0, SeekOrigin.Begin);
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,
                Inline = true
            };
            Response.Headers.Add("Content-Disposition", cd.ToString());
            return File(stream, mime);
        }
    }
}
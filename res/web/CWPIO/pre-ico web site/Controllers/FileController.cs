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
            using (var stream = new MemoryStream())
            {

                var mime = await _files.GetFileByNameAsync(fileName, stream);

                if (stream == null)
                {
                    return NotFound();
                }

                return File(stream, mime, fileName);
            }
        }
    }
}
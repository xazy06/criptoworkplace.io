using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public class FileRepository : IFileRepository
    {
        private readonly ILogger _logger;
        private readonly DriveService _drive;

        public FileRepository(DriveService drive,
            ILogger<FileRepository> logger)
        {
            _logger = logger;
            _drive = drive;
        }

        public string GetFileByName(string fileName, Stream data)
        {
            var list = _drive.Files.List();
            list.Q = $"name = '{fileName}'";
            var searched = list.Execute();
            if (searched.Files.Count == 0)
            {
                throw new FileNotFoundException($"File {fileName} not found in google drive");
            }
            var file = searched.Files.First();
            var getRequest = _drive.Files.Get(file.Id);
            var status = getRequest.DownloadWithStatus(data);
            if (status.Status != DownloadStatus.Completed)
                return null;
            return file.MimeType;
        }

        public async Task<string> GetFileByNameAsync(string fileName, Stream data)
        {
            var list = _drive.Files.List();
            list.Q = $"name = '{fileName}'";
            var searched = await list.ExecuteAsync();
            if (searched.Files.Count == 0)
            {
                throw new FileNotFoundException($"File {fileName} not found in google drive");
            }
            var file = searched.Files.First();
            var getRequest = _drive.Files.Get(file.Id);
            var status = await getRequest.DownloadAsync(data);
            if (status.Status != DownloadStatus.Completed)
                return null;
            return file.MimeType;

        }
    }
}

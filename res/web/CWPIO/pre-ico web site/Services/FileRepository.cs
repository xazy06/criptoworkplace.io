using Google.Apis.Download;
using Google.Apis.Drive.v3;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using pre_ico_web_site.Data;
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
        private readonly IMemoryCache _memoryCache;

        public FileRepository(DriveService drive,
            ILogger<FileRepository> logger,
            IMemoryCache memoryCache)
        {
            _logger = logger;
            _drive = drive;
            _memoryCache = memoryCache;
        }

        public string GetFileByName(string fileName, Stream data)
        {
            if (_memoryCache.TryGetValue(fileName, out MemoryCacheFileItem item))
            {
                new MemoryStream(item.Data).CopyTo(data);
                return item.MimeType;
            }
            else
            {
                var newItem = new MemoryCacheFileItem();

                var list = _drive.Files.List();
                list.Q = $"name = '{fileName}'";
                var searched = list.Execute();
                if (searched.Files.Count == 0)
                {
                    throw new FileNotFoundException($"File {fileName} not found in google drive");
                }
                var file = searched.Files.First();
                var getRequest = _drive.Files.Get(file.Id);
                var str = new MemoryStream();
                var status = getRequest.DownloadWithStatus(str);
                if (status.Status != DownloadStatus.Completed)
                {
                    return null;
                }

                newItem.Data = str.ToArray();
                newItem.MimeType = file.MimeType;
                _memoryCache.Set(fileName, newItem, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(30) });

                str.Seek(0, SeekOrigin.Begin);
                str.CopyTo(data);

                return file.MimeType;
            }
        }

        public async Task<string> GetFileByNameAsync(string fileName, Stream data)
        {
            if (_memoryCache.TryGetValue(fileName, out MemoryCacheFileItem item))
            {
                await new MemoryStream(item.Data).CopyToAsync(data);
                return item.MimeType;
            }
            else
            {
                var newItem = new MemoryCacheFileItem();

                var list = _drive.Files.List();
                list.Q = $"name = '{fileName}'";
                var searched = await list.ExecuteAsync();
                if (searched.Files.Count == 0)
                {
                    throw new FileNotFoundException($"File {fileName} not found in google drive");
                }
                var file = searched.Files.First();
                var getRequest = _drive.Files.Get(file.Id);
                var str = new MemoryStream();
                var status = await getRequest.DownloadAsync(str);
                if (status.Status != DownloadStatus.Completed)
                {
                    return null;
                }

                newItem.Data = str.ToArray();
                newItem.MimeType = file.MimeType;
                _memoryCache.Set(fileName, newItem, new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(30) });

                str.Seek(0, SeekOrigin.Begin);
                await str.CopyToAsync(data);

                return file.MimeType;
            }
        }


    }
}

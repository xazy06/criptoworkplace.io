using Google.Apis.Download;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pre_ico_web_site.Services
{
    public interface IFileRepository
    {
        void GetFileByName(string fileName, Stream data);
        Task<DownloadStatus> GetFileByNameAsync(string fileName, Stream data);
    }
}

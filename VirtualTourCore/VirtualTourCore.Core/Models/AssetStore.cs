using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualTourCore.Common.DataAccess;
using VirtualTourCore.Common.Helper;

namespace VirtualTourCore.Core.Models
{
    public class AssetStore : EntityBase
    {
        public int ClientId { get; set; }
        public string Filename { get; set; }
        public string Nickname { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        public string FullPath()
        {
            string fullPath = string.Empty;
            if (!string.IsNullOrWhiteSpace(Path))
            {
                fullPath = string.Format("{0}{1}/{2}",
                CommonConfiguration.Configuration.Azure_BlobStorageBaseUrl,
                CommonConfiguration.Configuration.Azure_BlobStorageContainer,
                Path);
            }
            return fullPath;
        }
    }
}

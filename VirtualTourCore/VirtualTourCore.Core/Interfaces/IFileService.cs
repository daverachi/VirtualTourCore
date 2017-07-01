using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualTourCore.Core.Interfaces
{
    public interface IFileService
    {
        bool UploadFile(byte[] file, string fileName);
        void UploadZip(byte[] zipFile, int tourId, int areaId, int locationId, int locationTechnologyTypeId
            , out string tourFileStoreRelativePath, out string directoryFileStoreFileRelativePath);
    }
}

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
        string UploadZip(byte[] zipFile, int clientId, int tourId, int areaId, int locationId);
    }
}

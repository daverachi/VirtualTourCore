using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using VirtualTourCore.Common.Enums;
using VirtualTourCore.Common.Helper;
using VirtualTourCore.Common.Logging;
using VirtualTourCore.Core.Interfaces;

namespace VirtualTourCore.Core.Services
{
    public class FileService : IFileService
    {
        private INlogger _log;
        public FileService(INlogger log)
        {
            _log = log;
        }

        #region Upload

        public bool UploadFile(byte[] file, string fileNameRelativePath)
        {
            bool result = false;
            // Save file to storage
            if (file != null && !String.IsNullOrWhiteSpace(fileNameRelativePath))
            {
                result = UploadFileAzure(file, fileNameRelativePath);
            }
            return result;
        }

        private bool UploadFileAzure(byte[] file, string fileNameRelativePath)
        {
            CloudBlobContainer container = GetAzureContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileNameRelativePath);
            blockBlob.Properties.ContentType = GetMimeContentType(fileNameRelativePath);
            blockBlob.UploadFromByteArray(file, 0, file.Length);
            return true;
        }

        private bool UploadFileAzure(ZipArchiveEntry entry, string fileNameRelativePath, string contentType)
        {
            if (!String.IsNullOrWhiteSpace(fileNameRelativePath) && !fileNameRelativePath.EndsWith("\\")) // Azure does not need to create folders
            {
                CloudBlobContainer container = GetAzureContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(fileNameRelativePath);
                using (var stream = entry.Open())
                {
                    blob.Properties.ContentType = contentType;
                    blob.UploadFromStream(stream);
                }
            }
            return true;
        }

        public string UploadZip(byte[] zipFile, int clientId, int tourId, int areaId, int locationId)
        {
            string tourFileStoreRelativePath = string.Empty;

            string mainTourUrl = "tour.html";

            if (mainTourUrl != null)
            {
                string contentType = "application/octet-stream";
                using (Stream fs = new MemoryStream(zipFile))
                {
                    using (ZipArchive archive = new ZipArchive(fs))
                    {
                        // check if there's a root directory in zip
                        string rootDirectory = null;
                        int startIndex = 1;
                        foreach (var entry in archive.Entries)
                        {
                            string[] directoryLocation = entry.FullName.Split('/');
                            if (rootDirectory == null)
                            {
                                rootDirectory = directoryLocation[0];
                            }
                            else if (directoryLocation[0] != rootDirectory)
                            {
                                // meaning there's no root level directory in the zip file
                                startIndex = 0;
                                break;
                            }
                        }

                        foreach (var entry in archive.Entries)
                        {
                            var directoryLocation = entry.FullName.Split('/');
                            var updatedDirectoryLocation = (string.Format("{0}\\{1}\\{2}\\{3}", clientId, locationId, areaId, tourId));
                            for (int i = startIndex; i < directoryLocation.Count(); i++)
                            {
                                updatedDirectoryLocation += "\\" + directoryLocation[i];

                                if (i == directoryLocation.Count() - 1)
                                {
                                    contentType = GetMimeContentType(directoryLocation[i]);
                                    if (directoryLocation[i].ToLower() == mainTourUrl)
                                    {
                                        tourFileStoreRelativePath = updatedDirectoryLocation;
                                    }
                                }
                            }
                            UploadFileAzure(entry, updatedDirectoryLocation, contentType);
                        }
                    }
                }
            }
            return tourFileStoreRelativePath;
        }

        #endregion Upload

        #region Delete

        private void DeleteFileAzure(string fileNameRelativePath)
        {
            if (!String.IsNullOrWhiteSpace(fileNameRelativePath))
            {
                CloudBlobContainer container = GetAzureContainer();
                CloudBlockBlob oldBlock = container.GetBlockBlobReference(fileNameRelativePath);
                if (oldBlock != null && oldBlock.Exists())
                {
                    oldBlock.Delete();
                }
            }
        }

        /// <summary>
        /// Cascading delete
        /// </summary>
        /// <param name="fileRelativePath"></param>
        private void DeleteBlobDirectoryAzure(string fileRelativePath)
        {
            if (!String.IsNullOrWhiteSpace(fileRelativePath))
            {
                CloudBlobContainer container = GetAzureContainer();
                var blobs = container.ListBlobs(fileRelativePath, true);
                if (blobs != null)
                {
                    foreach (var blob in blobs)
                    {
                        CloudBlockBlob oldBlock = container.GetBlockBlobReference(((CloudBlockBlob)blob).Name);
                        if (oldBlock != null && oldBlock.Exists())
                        {
                            oldBlock.Delete();
                        }
                    }
                }
            }
        }

        #endregion Delete

        #region Azure

        private CloudBlobContainer GetAzureContainer()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CommonConfiguration.Configuration.Azure_BlobStorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(CommonConfiguration.Configuration.Azure_BlobStorageContainer);
            container.CreateIfNotExists();
            return container;
        }

        #endregion Azure Storage

        private string GetMimeContentType(string fileName)
        {
            string returnValue = null;
            if (!String.IsNullOrWhiteSpace(fileName))
            {
                fileName = fileName.ToLower();
                string[] parsedFilename = fileName.Split('.');
                switch (Path.GetExtension(fileName))
                {
                    case FileExtensions.HTM:
                    case FileExtensions.HTML:
                        returnValue = "text/html";
                        break;
                    case FileExtensions.JS:
                        returnValue = "application/x-javascript";
                        break;
                    case FileExtensions.JPEG:
                    case FileExtensions.JPG:
                    case FileExtensions.PNG:
                        returnValue = "image/" + parsedFilename.Last();
                        break;
                    case FileExtensions.CSS:
                        returnValue = "text/css";
                        break;
                    default:
                        returnValue = "application/octet-stream";
                        break;
                }
            }
            return returnValue;
        }
    }
}
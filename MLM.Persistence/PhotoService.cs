//
// Copyright (C) Microsoft Corporation.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using MLM.Logging;
using MLM.Persistence.Interfaces;

namespace MLM.Persistence
{
    public class PhotoService : IPhotoService
    {
        ILogger log = null;

        public PhotoService(ILogger logger)
        {
            log = logger;
        }

        async public void CreateAndConfigureAsync()
        {
            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("images/maps");

                // Create the "images/maps" container if it doesn't already exist.
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Blob
                        });

                    log.Information("Successfully created Blob Storage images/maps Container and made it public");
                }


                container = blobClient.GetContainerReference("images/maps/thumbs");
                // Create the "images/maps/thumbs" container if it doesn't already exist.
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess =
                                BlobContainerPublicAccessType.Off
                        });

                    log.Information("Successfully created Blob Storage images/maps/thumbs Container and made it private");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "Failure to Create or Configure images/maps or images/thumbs container in Blob Storage Service");
                throw;
            }
        }

        async public Task<string> UploadPhotoAsync(HttpPostedFileBase photoToUpload, PhotoType photoType)
        {
            if (photoToUpload == null || photoToUpload.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create the blob client and reference the container                
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(GetStoragePath(photoType));

                // Create a unique name for the images we are about to upload
                string imageName = String.Format("map-{0}{1}",
                    Guid.NewGuid().ToString(),
                    Path.GetExtension(photoToUpload.FileName));

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = photoToUpload.ContentType;
                await blockBlob.UploadFromStreamAsync(photoToUpload.InputStream);

                //Convert to be HTTP b ase URI (default storage path is HTTPS)
                var uriBuilder = new UriBuilder(blockBlob.Uri);
                uriBuilder.Scheme = "http";

                fullPath = uriBuilder.ToString();

                timespan.Stop();
                log.TraceApi("Blob Service", "PhotoService.UploadPhoto", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error upload photo blob to storage");
                throw;
            }

            return fullPath;
        }

        private string GetStoragePath(PhotoType photoType)
        {
            switch(photoType)
            {
                case PhotoType.MapMainImage:
                    return "images/maps";
                case PhotoType.MapThumb:
                    return "images/maps/thumbs";
                default:
                    return "images";
            }
        }
    }
}

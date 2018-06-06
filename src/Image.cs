using System;
using System.IO;
using System.Collections.Generic;
#if netcore
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.StaticFiles;
#else
using System.Web;
#endif

namespace FroalaEditor
{
    /// <summary>
    /// Image functionality.
    /// </summary>
    public static class Image
    {
        /// <summary>
        /// Content type string used in http multipart.
        /// </summary>

        public static ImageOptions defaultOptions = new ImageOptions();

        /// <summary>
        /// Uploads an image to disk.
        /// </summary>
        /// <param name="httpContext">The HttpContext object containing information about the request.</param>
        /// <param name="fileRoute">Server route where the file will be uploaded. This route must be public to be accesed by the editor.</param>
        /// <param name="options">File options.</param>
        /// <returns>Object with link.</returns>
        public static object Upload (HttpContext httpContext, string fileRoute, FileOptions options = null)
        {
            if (options == null)
            {
                options = defaultOptions;
            }

            return File.Upload(httpContext, fileRoute, options);
        }

        /// <summary>
        /// Delete an image from disk.
        /// </summary>
        /// <param name="src">Server image path.</param>
        public static void Delete(string filePath)
        {
            File.Delete(filePath);
        }

        /// <summary>
        /// List images from disk.
        /// </summary>
        /// <param name="folderPath">Server folder path.</param>
        /// <param name="thumbPath">Optional. Server thumb path.</param>
        /// <returns></returns>
        public static List<object> List(string folderPath, string thumbPath = null)
        {
            // Use thumbPath as folderPath.
            if (thumbPath == null)
            {
                thumbPath = folderPath;
            }

            // Array of image objects to return.
            List<object> response = new List<object>();

            string absolutePath = File.GetAbsoluteServerPath(folderPath);

            string[] imageMimetypes = ImageValidation.AllowedImageMimetypesDefault;

            // Check if path exists.
            if (!System.IO.Directory.Exists(absolutePath))
            {
                throw new Exception("Images folder does not exist!");
            }

            string[] fileEntries = System.IO.Directory.GetFiles(absolutePath);

            // Add images.
            foreach (string filePath in fileEntries)
            {
                string fileName = System.IO.Path.GetFileName(filePath);
                if (System.IO.File.Exists(filePath))
                {   
#if netcore
                    string mimeType;
                    new FileExtensionContentTypeProvider().TryGetContentType(filePath, out mimeType);
                    if (mimeType == null) {
                        mimeType = "application/octet-stream";
                    }
#else
                    string mimeType = System.Web.MimeMapping.GetMimeMapping(filePath);
#endif        

                    if (Array.IndexOf(imageMimetypes, mimeType) >= 0)
                    {
                        response.Add(new
                        {
                            url = folderPath.Replace("wwwroot/", "") + fileName,
                            thumb = thumbPath.Replace("wwwroot/", "") + fileName,
                            name = fileName
                        });
                    }
                }
            }
            return response;
        }
    }
}

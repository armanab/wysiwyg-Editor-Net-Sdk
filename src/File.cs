using ImageMagick;
using System;
using System.IO;
#if netcore
using Microsoft.AspNetCore.Http;
#else
using System.Web;
#endif

namespace FroalaEditor
{
    /// <summary>
    /// File functionality.
    /// </summary>
    public static class File
    {
        /// <summary>
        /// Content type string used in http multipart.
        /// </summary>
        public static string MultipartContentType = "multipart/form-data";

        /// <summary>
        /// File default options.
        /// </summary>
        public static FileOptions defaultOptions = new FileOptions();

        /// <summary>
        /// Check http request content type.
        /// </summary>
        /// <returns>true if content type is multipart.</returns>
        public static bool CheckContentType(HttpContext httpContext)
        {       
            bool isMultipart = httpContext.Request.ContentType.StartsWith(MultipartContentType);

            return isMultipart;
        }

        /// <summary>
        /// Uploads a file to disk.
        /// </summary>
        /// <param name="httpContext">The HttpContext object containing information about the request.</param>
        /// <param name="fileRoute">Server route where the file will be uploaded. This route must be public to be accesed by the editor.</param>
        /// <param name="options">File options.</param>
        /// <returns>Object with link.</returns>
        public static object Upload(HttpContext httpContext, string fileRoute, FileOptions options = null)
        {
            // Use default file options.
            if (options == null)
            {
                options = defaultOptions;
            }

            if (!CheckContentType(httpContext))
            {
                throw new Exception("Invalid contentType. It must be " + MultipartContentType);
            }

            var httpRequest = httpContext.Request;

            int filesCount = 0;
#if netcore
            filesCount = httpRequest.Form.Files.Count;
#else
            filesCount = httpRequest.Files.Count;
#endif

            if (filesCount == 0)
            {
                throw new Exception("No file found");
            }

            // Get HTTP posted file based on the fieldname. 
#if netcore
            var file = httpRequest.Form.Files.GetFile(options.Fieldname);
#else
            var file = httpRequest.Files.Get(options.Fieldname);
#endif

            if (file == null)
            {
                throw new Exception("Fieldname is not correct. It must be: " + options.Fieldname);
            }

            // Generate Random name.
            string extension = Utils.GetFileExtension(file.FileName);
            string name = Utils.GenerateUniqueString() + "." + extension;

            string link = fileRoute + name; 

            // Create directory if it doesn't exist.
            FileInfo dir = new FileInfo(fileRoute);
            dir.Directory.Create();

            // Copy contents to memory stream.
            Stream stream;
#if netcore
            stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;
#else
            stream = file.InputStream;
#endif

            String serverPath = File.GetAbsoluteServerPath(link);

            // Save file to disk.
            Save(stream, serverPath, options);

            // Check if the file is valid.
            if (options.Validation != null && !options.Validation.Check(serverPath, file.ContentType))
            {
                // Delete file.
                Delete(serverPath);
                throw new Exception("File does not meet the validation.");
            }

            // Make sure it is compatible with ASP.NET Core.
            return new { link = link.Replace("wwwroot/", "") };
        }

        /// <summary>
        /// Get absolute server path.
        /// </summary>
        /// <param name="path">Relative path.</param>
        /// <returns>Absolute path.</returns>
        public static String GetAbsoluteServerPath(string path) 
        {
#if netcore
            return path;
#else 
            return HttpContext.Current.Server.MapPath(path);
#endif
        }

        /// <summary>
        /// Save an input file stream to disk.
        /// </summary>
        /// <param name="fileStream">Input file stream</param>
        /// <param name="link">Server file path.</param>
        /// <param name="options">File options.</param>
        public static void Save(Stream fileStream, string filePath, FileOptions options)
        {
            if (options is ImageOptions && ((ImageOptions)options).ResizeGeometry != null)
            {
                // Resize file and save it..
                MagickImage image = new MagickImage(fileStream);
                image.Resize(((ImageOptions)options).ResizeGeometry);
                image.Write(filePath);
            }
            else
            {
                // Save file to disk.
                Console.WriteLine(filePath);
                FileStream writerfileStream = System.IO.File.Create(filePath);
                fileStream.CopyTo(writerfileStream);
                writerfileStream.Dispose();
            }
        }

        /// <summary>
        /// Delete a file from disk.
        /// </summary>
        /// <param name="src">Server file path.</param>
        public static void Delete(string filePath)
        {
#if netcore
            filePath = "wwwroot/" + filePath;
#else
            filePath = File.GetAbsoluteServerPath(filePath);
#endif
            // Will throw an exception if an error occurs.
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}

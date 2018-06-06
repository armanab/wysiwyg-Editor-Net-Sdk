using System;
using System.Linq;

namespace FroalaEditor
{
    /// <summary>
    /// File Validation.
    /// </summary>
    public class FileValidation
    {
        /// <summary>
        /// Allowed file validation default extensions.
        /// </summary>
        public static string[] AllowedFileExtsDefault = new string[] { "txt", "pdf", "doc" };

        /// <summary>
        /// Allowed file validation default mimetypes.
        /// </summary>
        public static string[] AllowedFileMimetypesDefault = new string[] { "text/plain", "application/msword", "application/x-pdf", "application/pdf" };

        /// <summary>
        /// Allowed validation extensions.
        /// </summary>
        protected string[] AllowedExts { get; set; }

        /// <summary>
        /// Allowed validation mimetypes.
        /// </summary>
        protected string[] AllowedMimeTypes { get; set; }

        /// <summary>
        /// Custom function used or validation.
        /// </summary>
        protected Func<string, string, bool> Function { get; set; }

        /// <summary>
        /// Init default file validation settings.
        /// </summary>
        protected virtual void initDefault()
        {
            AllowedExts = AllowedFileExtsDefault;
            AllowedMimeTypes = AllowedFileMimetypesDefault;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileValidation()
        {
            initDefault();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="allowedExts">Allowed validation file extensions.</param>
        /// <param name="allowedMimeTypes">Allowed validation file mimetypes.</param>
        public FileValidation(string[] allowedExts, string[] allowedMimeTypes)
        {
            initDefault();

            if (allowedExts != null)
            {
                AllowedExts = allowedExts;
            }
            
            if (allowedMimeTypes != null)
            {
                AllowedMimeTypes = allowedMimeTypes;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="function">Custom function used for validation.</param>
        public FileValidation(Func<string, string, bool> function)
        {
            initDefault();

            this.Function = function;
        }

        /// <summary>
        /// Check if file is valid.
        /// Use only the custom function if provided. Else check if the file has an allowed extension and mimetype.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="mimeType">File mimetype.</param>
        /// <returns></returns>
        public bool Check(string filePath, string mimeType)
        {
            if (Function != null)
            {
                return Function(filePath, mimeType);
            }

            return AllowedExts.Contains(Utils.GetFileExtension(filePath)) && AllowedMimeTypes.Contains(mimeType.ToLower());
        }
    }
}

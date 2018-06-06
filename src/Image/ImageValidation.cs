using System;

namespace FroalaEditor
{
    /// <summary>
    /// Image Validation.
    /// </summary>
    public class ImageValidation:FileValidation
    {
        /// <summary>
        /// Allowed image validation default extensions.
        /// </summary>
        public static string[] AllowedImageExtsDefault = new string[] { "gif", "jpeg", "jpg", "png", "svg", "blob" };

        /// <summary>
        /// Allowed image validation default mimetypes.
        /// </summary>
        public static string[] AllowedImageMimetypesDefault = new string[] { "image/gif", "image/jpeg", "image/pjpeg", "image/x-png", "image/png", "image/svg+xml" };

        /// <summary>
        /// Init default image validation settings.
        /// </summary>
        protected override void initDefault()
        {
            AllowedExts = AllowedImageExtsDefault;
            AllowedMimeTypes = AllowedImageMimetypesDefault;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageValidation(): base()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="allowedExts">Allowed validation image extensions.</param>
        /// <param name="allowedMimeTypes">Allowed validation image mimetypes.</param>
        public ImageValidation(string[] allowedExts, string[] allowedMimeTypes): base(allowedExts, allowedMimeTypes)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="function">Custom function used for validation.</param>
        public ImageValidation(Func<string, string, bool> function): base(function)
        {
        }
    }
}

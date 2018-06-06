using ImageMagick;

namespace FroalaEditor
{
    /// <summary>
    /// Image Options used for uploading.
    /// </summary>
    public class ImageOptions:FileOptions
    {
        /// <summary>
        /// Parameter for resizing an image.
        /// </summary>
        public MagickGeometry ResizeGeometry { get; set; }

        /// <summary>
        /// Init default image upload settings.
        /// </summary>
        protected override void initDefault()
        {
            Validation = new ImageValidation();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImageOptions(): base() { }
    }
}

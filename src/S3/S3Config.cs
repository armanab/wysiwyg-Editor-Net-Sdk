
namespace FroalaEditor
{
    /// <summary>
    /// Amazon S3 config string.
    /// </summary>
    public class S3Config
    {
        /// <summary>
        /// Bucket name.
        /// </summary>
        public string Bucket { get; set; }

        /// <summary>
        /// Region. Use region from here: http://docs.aws.amazon.com/general/latest/gr/rande.html#s3_region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Starting foldername where the files will be saved under the bucket.
        /// </summary>
        public string KeyStart { get; set; }

        /// <summary>
        /// Amazon Canned Access Control List: http://docs.aws.amazon.com/AmazonS3/latest/dev/acl-overview.html
        /// </summary>
        public string Acl { get; set; }

        /// <summary>
        /// Amazon Public key. Can be found on your Account page, under Security Credentials > Access Keys.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Amazon Secret key. Can be found on your Account page, under Security Credentials > Access Keys.
        /// </summary>
        public string SecretKey { get; set; }
    }
}

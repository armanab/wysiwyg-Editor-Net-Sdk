using System;
using System.Web.Mvc;
using ImageMagick;

namespace demo.Controllers
{
    public class FroalaApiController : Controller
    {
        public ActionResult UploadImage()
        {
            string uploadPath = "/Public/";

            try
            {
                return Json(FroalaEditor.Image.Upload(System.Web.HttpContext.Current, uploadPath));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult UploadFile()
        {
            string uploadPath = "/Public/";

            try
            {
                return Json(FroalaEditor.File.Upload(System.Web.HttpContext.Current, uploadPath));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult LoadImages()
        {
            string uploadPath = "/Public/";

            try
            {
                return Json(FroalaEditor.Image.List(uploadPath));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult UploadImageResize()
        {
            string fileRoute = "/Public/";

            MagickGeometry resizeGeometry = new MagickGeometry(300, 300);
            resizeGeometry.IgnoreAspectRatio = true;

            FroalaEditor.ImageOptions options = new FroalaEditor.ImageOptions
            {
                ResizeGeometry = resizeGeometry
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(System.Web.HttpContext.Current, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult UploadImageValidation()
        {
            string fileRoute = "/Public/";

            Func<string, string, bool> validationFunction = (filePath, mimeType) => {

                MagickImageInfo info = new MagickImageInfo(filePath);

                if (info.Width != info.Height)
                {
                    return false;
                }

                return true;
            };

            FroalaEditor.ImageOptions options = new FroalaEditor.ImageOptions
            {
                Fieldname = "myImage",
                Validation = new FroalaEditor.ImageValidation(validationFunction)
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(System.Web.HttpContext.Current, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult UploadFileValidation()
        {
            string fileRoute = "/Public/";

            Func<string, string, bool> validationFunction = (filePath, mimeType) => {

                long size = new System.IO.FileInfo(filePath).Length;
                if (size > 10 * 1024 * 1024)
                {
                    return false;
                }

                return true;
            };

            FroalaEditor.FileOptions options = new FroalaEditor.FileOptions
            {
                Fieldname = "myFile",
                Validation = new FroalaEditor.FileValidation(validationFunction)
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(System.Web.HttpContext.Current, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult DeleteFile()
        {
            try
            {
                FroalaEditor.File.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public ActionResult DeleteImage()
        {
            try
            {
                FroalaEditor.Image.Delete("/Public/" + HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public object S3Signature()
        {
            FroalaEditor.S3Config config = new FroalaEditor.S3Config
            {
                Bucket = Environment.GetEnvironmentVariable("AWS_BUCKET"),
                Region = Environment.GetEnvironmentVariable("AWS_REGION"),
                KeyStart = Environment.GetEnvironmentVariable("AWS_KEY_START"),
                Acl = Environment.GetEnvironmentVariable("AWS_ACL"),
                AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY"),
                SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY")
            };

            return Json(FroalaEditor.S3.GetHash(config), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}

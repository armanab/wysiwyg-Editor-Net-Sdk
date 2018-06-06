using System;
using Microsoft.AspNetCore.Mvc;
using ImageMagick;

namespace demo.Controllers
{
    public class FroalaApiController : Controller
    {
        public IActionResult UploadImage()
        {
            string uploadPath = "wwwroot/uploads/";

            try
            {  
                return Json(FroalaEditor.Image.Upload(HttpContext, uploadPath));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadFile () {
            string uploadPath = "wwwroot/uploads/";

            object response;
            try
            {  
                response = FroalaEditor.File.Upload(HttpContext, uploadPath);
                return Json(response);
            }
            catch (Exception e)
            {
               return Json(e);
            }
        }

        public IActionResult LoadImages()
        {
            string uploadPath = "wwwroot/uploads/";

            try
            {  
                return Json(FroalaEditor.Image.List(uploadPath));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadImageResize()
        {
            string fileRoute = "wwwroot/uploads/";

            MagickGeometry resizeGeometry = new MagickGeometry(300, 300);
            resizeGeometry.IgnoreAspectRatio = true;

            FroalaEditor.ImageOptions options = new FroalaEditor.ImageOptions
            {
                ResizeGeometry = resizeGeometry
            };

            try
            {
                return Json(FroalaEditor.Image.Upload(HttpContext, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadImageValidation ()
        {
            string fileRoute = "wwwroot/uploads/";

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
                return Json(FroalaEditor.Image.Upload(HttpContext, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult UploadFileValidation ()
        {
            string fileRoute = "wwwroot/";

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
                return Json(FroalaEditor.Image.Upload(HttpContext, fileRoute, options));
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult DeleteFile ()
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

        public IActionResult DeleteImage()
        {
            try
            {
                FroalaEditor.Image.Delete(HttpContext.Request.Form["src"]);
                return Json(true);
            }
            catch (Exception e)
            {
                return Json(e);
            }
        }

        public IActionResult S3Signature ()
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

            return Json(FroalaEditor.S3.GetHash(config));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

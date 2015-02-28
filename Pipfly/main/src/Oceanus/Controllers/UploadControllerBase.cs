using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Oceanus.Attributes;
using Oceanus.Logging;
using Oceanus.Utilities;

namespace Oceanus.Controllers
{
    public class UploadControllerBase : ControllerBase
    {
        protected virtual string RelativeUploadPath
        {
            get { return "Uploads"; }
        }

        protected virtual string AbsolutePathFormat
        {
            get { return @"{0}\{1}\{2}\{3}"; }
        }


        protected void GenerateThumbnails(string fileName, Dictionary<string, int> sizes)
        {
            string nameOnly = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName).ToLower();
            string folder = Path.GetDirectoryName(fileName);
            string newNamePrefix = Path.Combine(folder, nameOnly);

            // image.FromFile locks: http://danbystrom.se/2009/01/05/imagegetthumbnailimage-and-beyond/
            //using (Image img = Image.FromFile(fileName))
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                Bitmap img = (Bitmap)Bitmap.FromStream(fs);

                // crop the image before producing thumbnails
                //var croppedImage = ImageUtilities.GetCroppedImage(img);

                foreach (var thumbSize in sizes)
                {
                    var fullThumbPath = string.Concat(newNamePrefix, ".thumb.", thumbSize.Key, extension);

                    // if the image is smaller than thumbnail size, don't resize, only compress and save
                    if (img.Width < thumbSize.Value)
                    {
                        ImageUtilities.SaveImage(fullThumbPath, img);
                    }
                    // resize, compress and save
                    else
                    {
                        using (Image thumbImg = img.GetThumbnailImage(thumbSize.Value, (int)(((thumbSize.Value * 1.0) / img.Width) * img.Height), null, new IntPtr()))
                        {
                            ImageUtilities.SaveImage(fullThumbPath, thumbImg);
                        }
                    }
                }

                img.Dispose();
                img = null;
            }
        }

        [TokenizedAuthorize]
        [RequiresAuthentication]
        [AcceptVerbs(HttpVerbs.Post)]
        internal ContentResult CheckExisting(int contentId, string filename, string storeFolder)
        {
            string result = "0";

            string fullPath;
            if (CheckFileExists(contentId, filename, storeFolder, out fullPath))
            {
                result = "1";
            }

            return new ContentResult() { Content = result };
        }

        [TokenizedAuthorize]
        [RequiresAuthentication]
        internal virtual ContentResult Upload(int contentId, HttpPostedFileBase fileData, string storeFolder)
        {
            // save file and return 1 on success
            string result = "1";
            try
            {
                var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, contentId);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                fileData.SaveAs(Path.Combine(folder, fileData.FileName));
            }
            catch (Exception ex)
            {
                string message = string.Format("{0}: {1}: Could not save {2} to {3}",
                                               "UploadControllerBase", "Upload", fileData.FileName, storeFolder);
                
                Logger.Instance.Warn(message);

                result = "0";
            }

            return new ContentResult() { Content = result };
        }

        #region helper methods

        public string EnsureOriginalFileIsJpeg(int vendorId, string storeFolder, string fileName)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, vendorId);
            bool deleteOriginal = false;
            string tempName = string.Empty;
            string fullPath = string.Format(@"{0}\{1}", folder, fileName);

            using (FileStream fs = new FileStream(fullPath, FileMode.Open))
            {
                Image img = Image.FromStream(fs);

                deleteOriginal = !ImageUtilities.IsJpeg(img);

                if (deleteOriginal)
                {
                    tempName = Path.GetTempFileName();
                    ImageUtilities.SaveImage(tempName, img);
                }
            }

            if (deleteOriginal)
            {
                var oldName = Path.GetFileNameWithoutExtension(fileName);
                var oldPath = Path.GetDirectoryName(fullPath);
                var newName = string.Concat(Path.Combine(oldPath, oldName), ".jpg");
                try
                {
                    System.IO.File.Delete(fullPath);
                    System.IO.File.Delete(newName);
                }
                catch
                {
                }
                System.IO.File.Move(tempName, newName);

                return newName;
            }

            return fullPath;
        }

        protected bool CheckFileExists(int contentId, string fileName, string storeFolder, out string fullPath)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, contentId);
            fullPath = string.Format(@"{0}\{1}", folder, fileName);
            return System.IO.File.Exists(fullPath);
        }

        /// <summary>
        /// Confirms a file for use in the vendor portfolio - removes the pending suffix.
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="file"></param>
        /// <param name="replacedName"></param>
        /// <param name="suffix"></param>
        protected string Confirm(string storeFolder, int contentId, string file, string replacedName, string suffix)
        {
            string path = null;

            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, contentId);

            var fullFilePath = Path.Combine(folder, file);

            string nameOnly = Path.GetFileNameWithoutExtension(fullFilePath).Replace(" ", ".");
            string extension = Path.GetExtension(fullFilePath).ToLower();

            var usedName = string.IsNullOrEmpty(replacedName) ? nameOnly : replacedName;
            string newNamePrefix = string.Concat(usedName, suffix);
            string newName = string.Concat(newNamePrefix, extension);

            var fullNewPath = Path.Combine(folder, newName);
            if (CheckFileExists(contentId, file, storeFolder, out path))
            {
                if (System.IO.File.Exists(fullNewPath))
                {
                    System.IO.File.Delete(fullNewPath);
                }

                System.IO.File.Move(path, fullNewPath);
            }

            return newName;
        }

        protected void Delete(string storeFolder, int contentId, string file)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, contentId);
            var fullPath = Path.Combine(folder, file);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        protected void Sweep(string storeFolder, int contentId, string suffix)
        {
            var folder = string.Format(AbsolutePathFormat, Request.PhysicalApplicationPath, RelativeUploadPath, storeFolder, contentId);
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                if (!file.Contains(suffix))
                {
                    System.IO.File.Delete(Path.Combine(folder, file));
                }
            }
        }

        #endregion
    }
}
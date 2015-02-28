using System;
using System.Diagnostics;
using System.IO;
using Oceanus.Controllers;
using Oceanus.Data;
using System.Drawing;

namespace Oceanus.Utilities
{
    public static class ImageThumbnailer
    {
        private const string RelativeVendorPath = "Uploads";
        private const string AbsolutePathFormat = @"{0}\{1}\{2}";

        private const double MinRatio = 1d;
        private const double MaxRatio = 1.8;
        private const double TargetRatio = 1.48;

        //public static void UpdateImageNames()
        //{
        //    VendorImageController vic = new VendorImageController();

        //    using (OceanusEntities entities = new OceanusEntities())
        //    {
        //        foreach (var image in entities.VendorImages)
        //        {
        //            var name = image.ImageUrl;
        //            var vendorId = image.vendorId;

        //            var folder = string.Format(AbsolutePathFormat, @"D:\sixloonies\projects\main\src\Oceanus", RelativeVendorPath, vendorId);
        //            var existingFile = string.Format("{0}\\{1}", folder, name);
                    
        //            try
        //            {
        //                existingFile = vic.EnsureOriginalFileIsJpeg(vendorId, existingFile);
        //                name = Path.GetFileName(existingFile);

        //                var nameOnly = Path.GetFileNameWithoutExtension(name).Replace(".PNG", "").Replace(".JPG", "").Replace(" ", ".").Replace("__confirmed", ".final");
        //                var ext = Path.GetExtension(name).ToLower();
        //                var scrubbedFile = string.Concat(nameOnly, ext);

        //                image.ImageUrl = scrubbedFile;

        //                var newFile = Path.Combine(folder, scrubbedFile);
        //                if (File.Exists(existingFile))
        //                {
        //                    File.Move(existingFile, newFile);
        //                    //Console.WriteLine(string.Format("Moving {0} to {1}", existingFile, scrubbedFile));
        //                }

        //                var existingThumbFile = existingFile.Replace("__confirmed", "__confirmed_thumb");
        //                var nameOnlyThumb = Path.GetFileNameWithoutExtension(existingThumbFile).Replace(" ", ".").Replace("__confirmed_thumb", ".final.thumb.profile");
        //                var scrubbedThumbFile = string.Concat(nameOnlyThumb, ext);
                        
        //                if (File.Exists(existingThumbFile))
        //                {
        //                    var newThumbFile = Path.Combine(folder, scrubbedThumbFile);
        //                    File.Move(existingThumbFile, newThumbFile);
        //                    //Console.WriteLine(string.Format("Moving thumb {0} to {1}", existingThumbFile, scrubbedThumbFile));
        //                }

        //                vic.GenerateThumbnails(newFile);
                        
        //            }
        //            catch (IOException)
        //            {
                        
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(string.Format("Error with file {0}: {1}", existingFile, ex.Message));
        //                //Debug.WriteLine(string.Format("Error with file {0}: {1}", existingFile, ex.Message));
        //            }
        //        }

        //        entities.SaveChanges();
        //    }

        //    Console.WriteLine("Complete");
        //}

        //public static void RemoveBadAspectRatios()
        //{
        //    using (OceanusEntities entities = new OceanusEntities())
        //    {
        //        foreach (var image in entities.VendorImages)
        //        {
        //            var name = image.ImageUrl;
        //            var vendorId = image.vendorId;

        //            var folder = string.Format(AbsolutePathFormat, @"D:\sixloonies\projects\main\src\Oceanus",
        //                                       RelativeVendorPath, vendorId);
        //            var existingFile = string.Format("{0}\\{1}", folder, name);

        //            try
        //            {
        //                bool delete = false;
        //                using (FileStream fs = new FileStream(existingFile, FileMode.Open))
        //                {
        //                    Image img = Image.FromStream(fs);

        //                    int width = img.Width;
        //                    int height = img.Height;

        //                    // get the aspect ratio of the major axis (> 1)
        //                    double ratio = width > height
        //                                       ? (double)width / (double)height
        //                                       : (double)height / (double)width;

        //                    if (ratio < MinRatio || ratio > MaxRatio)
        //                    {
        //                        delete = true;
        //                    }
        //                }

        //                if (delete)
        //                {
        //                    File.Delete(existingFile);
        //                    entities.VendorImages.DeleteObject(image);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("Error: " + ex.Message);
        //            }
        //        }

        //        entities.SaveChanges();
        //    }

        //    Console.WriteLine("Complete");
        //}

        public static void StripExtentions()
        {
            //using (OceanusEntities entities = new OceanusEntities())
            //{
            //    foreach (var image in entities.VendorImages)
            //    {
            //        image.ImageUrl = Path.GetFileNameWithoutExtension(image.ImageUrl);
            //    }

            //    entities.SaveChanges();
            //}

            //Console.WriteLine("Complete");
        }
    }
}
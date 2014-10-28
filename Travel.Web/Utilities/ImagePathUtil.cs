using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web.Hosting;
using Travel.Web.Enums;
using Travel.Web.ViewModels;
using Travel.Web.Properties;
using Travel.Web.Models;
using System.Diagnostics;
using Travel.Web.Exceptions;


namespace Travel.Web.Utilities
{
    /// <summary>
    /// Generates paths and urls to the destination images and thumbnails.
    /// 
    /// 
    /// </summary>
    public class ImagePathUtil
    {
        /// <summary>
        /// Example: c:\travel\Content\images\destinations
        /// </summary>
        private string imagePhysicalRootDir;

        /// <summary>
        /// Example: c:\travel\Content\images\destinations\_thumbs
        /// </summary>
        private string thumbPhysicalRootDir;

        public ImagePathUtil()
        {

            imagePhysicalRootDir = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                             @"Content\images\destinations"));

            thumbPhysicalRootDir = Path.Combine(imagePhysicalRootDir, "_thumbs");

            //File.WriteAllLines("c:\\test\\test.txt", new string[] { AppDomain.CurrentDomain.BaseDirectory, imagePhysicalRootDir, thumbPhysicalRootDir });

            //this.imagePhysicalRootDir = HostingEnvironment.MapPath(Settings.Default.DestinationImageVirtualRootDir);
            //this.thumbPhysicalRootDir = HostingEnvironment.MapPath(Settings.Default.DestinationThumbVirtualRootDir);

            //if (imagePhysicalRootDir == null) {
                //imagePhysicalRootDir = Properties.Settings.Default.ImagePhysicalRootDir;
                //thumbPhysicalRootDir = Properties.Settings.Default.ThumbPhysicalRootDir;
            //}
        }

        //public ImagePathUtil()
        //{
        //    this.imagePhysicalRootDir = HostingEnvironment.MapPath(Settings.Default.DestinationImageVirtualRootDir);
        //    this.thumbPhysicalRootDir = HostingEnvironment.MapPath(Settings.Default.DestinationThumbVirtualRootDir);
        //}

        public string CreateDestinationPath(Destination destination, string filename, ImageType imageType=ImageType.Image)
        {
            return Path.Combine(
                imageType == ImageType.Image ? imagePhysicalRootDir : thumbPhysicalRootDir, 
                destination.Country.Slug,
                destination.Slug, 
                filename);
        }

      

        public ImagePaths CreatePaths(Destination destination, string imageFilename)
        {
            var common = Path.Combine(destination.Country.Slug, destination.Slug, imageFilename);

            return new ImagePaths(
                image: Path.Combine(imagePhysicalRootDir, common), 
                thumb: Path.Combine(thumbPhysicalRootDir, common));
        }

       

        internal string CreatePhysicalSearchPath(Destination destination, ImageType imageType = ImageType.Image)
        {
            return Path.Combine(
                imageType == ImageType.Image ? imagePhysicalRootDir : thumbPhysicalRootDir,
                destination.Country.Slug,
                destination.Slug);
        }

        

        public static string PlaceHolderUrl(HttpContextBase context, ImageType imageType = ImageType.Image)
        {
            return UrlHelper.GenerateContentUrl(imageType == ImageType.Image
                    ? Settings.Default.ImagePlaceholderPath
                    : Settings.Default.ThumbPlaceholderPath, context);
        }

       

      

        public IEnumerable<string> DestinationImageUrls(Destination destination,
            HttpContextBase context, ImageType imageType = ImageType.Image)
        {
            // the physical directory that the destinations images are stored in
            // example c:\website\content\images\destinations\NZ\auckland
            string searchPath = CreatePhysicalSearchPath(destination);

            if (! Directory.Exists(searchPath))
            {
                throw new TravelException(
                    string.Format("Destination {0} has no image directory.", destination.Name));
            }

            // the physical paths to the images filtered by image extension
            var paths = Settings.Default.AllowedImageExtensions.Split(' ')
                                .SelectMany(imageExtn => Directory.EnumerateFiles(searchPath, "*." + imageExtn));
            
            // create the image content urls from the virtual paths
            
            foreach (var path in paths)
            {
                // create virtual path from physical path
                // eg c:\site\content\images\pic.jpg => ~\Content\images\pic.jpg
                var virtualPath = string.Format("{0}/{1}/{2}/{3}",
                        Settings.Default.DestinationImageVirtualRootDir, 
                        destination.Country.Slug, 
                        destination.Slug, 
                        Path.GetFileName(path));

                // Create relative url from the virtual path.
                // eg ~\Content\images\pic.jpg => Content/images/pic.jpg
                yield return UrlHelper.GenerateContentUrl(virtualPath, context);
            }
        }

        public static string DestinationImageUrl(RequestContext requestContext, DestinationVm dvm, ImageType imageType = ImageType.Image, int index = 0)
        {
            // warning: Currently the image index isn't validated against the number of image files.

            // create the url to the destination image directory
            string imgDirUrl = new UrlHelper(requestContext).Content(string.Format("{0}/{1}/{2}",
                
                imageType == ImageType.Image 
                    ? Settings.Default.DestinationImageVirtualRootDir 
                    : Settings.Default.DestinationThumbVirtualRootDir, 
                    
                dvm.Destination.Country.Slug,
                dvm.Destination.Slug
            ));

            // get the physical path to the destination's image directory
            // so the image can be checked and selected
            string imgDirPath = requestContext.HttpContext.Request.MapPath(imgDirUrl);

            if (Directory.Exists(imgDirPath))
            {

                // get the image filename
                var imageFilename = Directory.EnumerateFiles(imgDirPath)
                                             .OrderBy(p => p)
                                             .Skip(index)
                                             .Select(p => Path.GetFileName(p))
                                             .FirstOrDefault();

                // return the image url 
                if (imageFilename != null)
                {
                    return imgDirUrl + "/" + imageFilename;
                }
            }

            // There was no image directory or no image file for this destination
            // so show a placeholder image.

            return ImagePathUtil.PlaceHolderUrl(requestContext.HttpContext, imageType);
        }



        

        internal void CreateDirectories(string countrySlug, string destinationSlug)
        {
          
            var imageDir = Path.Combine(imagePhysicalRootDir, countrySlug, destinationSlug);
            var thumbDir = Path.Combine(thumbPhysicalRootDir, countrySlug, destinationSlug);

            if (!Directory.Exists(imageDir))
            {
                Directory.CreateDirectory(imageDir);
            }

            if (!Directory.Exists(thumbDir))
            {
                Directory.CreateDirectory(thumbDir);
            }
        }

        /// <summary>
        /// Renames destination image and thumb directories. 
        /// Necessary when a destination is renamed.
        /// </summary>
        /// <param name="countrySlug"></param>
        /// <param name="originalDestinationSlug"></param>
        /// <param name="currentDestinationSlug"></param>
        internal void RenameDestinationDirectories(string countrySlug, string originalDestinationSlug, string currentDestinationSlug)
        {
            RenameDirectories(                
               Path.Combine(imagePhysicalRootDir, countrySlug, originalDestinationSlug), // images
               Path.Combine(imagePhysicalRootDir, countrySlug, currentDestinationSlug),                
               Path.Combine(thumbPhysicalRootDir, countrySlug, originalDestinationSlug), // thumbs
               Path.Combine(thumbPhysicalRootDir, countrySlug, currentDestinationSlug));
        }

        /// <summary>
        /// Renames a destination's country directory. Necessary when a country is renamed.
        /// </summary>
        /// <param name="originalCountrySlug"></param>
        /// <param name="currentCountrySlug"></param>
        internal void RenameCountryDirectories(string originalCountrySlug, string currentCountrySlug)
        {
            RenameDirectories(
                Path.Combine(imagePhysicalRootDir, originalCountrySlug), // images
                Path.Combine(imagePhysicalRootDir, currentCountrySlug),
                Path.Combine(thumbPhysicalRootDir, originalCountrySlug), // thumbs
                Path.Combine(thumbPhysicalRootDir, currentCountrySlug));
        }

        /// <summary>
        /// Renames directories unless the source doesn't exist or the destination does.
        /// </summary>
        /// <param name="paths">{ "source1", "dest1", "source2", "dest2" ...}</param>
        private void RenameDirectories(params string[] paths)
        {
            for (int source = 0; source < paths.Length; source += 2)
            {
                var sourceDir = paths[source];
                var destDir = paths[source + 1];
                if (Directory.Exists(sourceDir) && (! Directory.Exists(destDir)))
                {
                    Directory.Move(sourceDir, destDir);
                }
            }
        }

        
    }

    
}

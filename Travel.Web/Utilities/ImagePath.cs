using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Web.Utilities
{
    /// <summary>
    /// The image and thumbnail paths of a destination
    /// </summary>
    public class ImagePaths
    {
        public string Image { get; private set; }
        public string Thumb { get; private set; }

        public ImagePaths(string image, string thumb)
        {
            Image = image;
            Thumb = thumb;
        }
    }
}
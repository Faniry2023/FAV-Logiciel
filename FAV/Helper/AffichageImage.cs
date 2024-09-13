using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FAV.Helper
{
    public class AffichageImage
    {
        public static BitmapImage GetBitmapImage(byte[]? iPhoto)
        {
            var img = new BitmapImage();
            if (iPhoto is not null || iPhoto?.Length is not 0)
            {
                if (iPhoto != null)
                {
                    using (var stream = new MemoryStream(iPhoto))
                    {
                        stream.Position = 0;
                        img.BeginInit();
                        img.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.StreamSource = stream;
                        img.EndInit();
                    }
                }
            }
            return img;
        }
    }
}

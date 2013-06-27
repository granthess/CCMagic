using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using S3ToolKit.MagicEngine.Core;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using S3ToolKit.Utils.Registry;
using S3ToolKit.Utils.Logging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;


namespace CCMagic.ViewModel
{
    public class GameVersionDisplayEntry : GameVersionEntry
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());

        public BitmapImage Image { get; set; }
        public Image Content { get { return GetContent(); } }

        private Image GetContent()
        {
            System.Windows.Controls.Image Img = new Image();

            Img.Source = this.Image;
            Img.ToolTip = this.DisplayValue;

            return Img;
        }

        public GameVersionDisplayEntry(GameVersionEntry Entry) : base (Entry.baseEntry)
        {
            GenerateBitmap();

        }

        [DllImport("shell32.dll")]
        static extern IntPtr ExtractIcon(
            IntPtr hInst,
            [MarshalAs(UnmanagedType.LPStr)] string lpszExeFileName,
            uint nIconIndex);

        private void GenerateBitmap()
        {
            string FileName = GetBinDirectory();

            if (FileName == "")
            {
                log.Error("Cannot Load Icon from file: " + FileName);
                return;
            }

            var inst = Process.GetCurrentProcess().Handle;
            var x = ExtractIcon(inst, FileName, 0);

            System.Drawing.Icon myIcon = System.Drawing.Icon.FromHandle(x);
            System.Drawing.Bitmap bmp = myIcon.ToBitmap();

            MemoryStream temp = new MemoryStream();
            bmp.Save(temp, System.Drawing.Imaging.ImageFormat.Png);

            BitmapImage bmpImage = new BitmapImage();


            bmpImage.BeginInit();

            temp.Seek(0, SeekOrigin.Begin);

            bmpImage.StreamSource = temp;

            bmpImage.EndInit();

            Image = bmpImage;
            Image.CacheOption = BitmapCacheOption.Default;

            //temp.Close();
        }
    }
}

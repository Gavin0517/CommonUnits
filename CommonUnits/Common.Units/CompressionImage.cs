using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace Common.Units
{
    /// <summary>
    /// 图片压缩类
    /// </summary>
    public class CompressionImage
    {
        /// <summary>
        /// Gavin Add 压缩图片文件
        /// 2016-05-09
        /// </summary>
        /// <param name="imageStream">源文件数据流</param>
        /// <param name="oldWidth">源文件的长度</param>
        /// <param name="oldHeight">源文件的高度</param>
        /// <param name="imageFormat">图片类型(jpg,png,bnp)</param>
        /// <returns>返回内存流</returns>
        public static MemoryStream BuildImageByFixedSize(Stream imageStream, int oldWidth, int oldHeight, string imageType)
        {
            ImageFormat imageFormat = GetImageType(imageType);
            MemoryStream outStream = new MemoryStream();
            try
            {
                Int32 targetWidth = (Int32)(oldWidth * 0.4);
                Int32 targetHeight = (Int32)(oldHeight * 0.4);
                System.Drawing.Image image = Image.FromStream(imageStream);
                using (Bitmap bitmap = new Bitmap(image, targetWidth, targetHeight))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Clear(Color.Transparent);
                        graphics.DrawImage(image, new Rectangle(0, 0, targetWidth, targetHeight));
                        image.Dispose();
                        bitmap.Save(outStream, imageFormat);
                        return outStream;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Units.Error.WriteErrorXml("图片压缩出错:"+ex.Message);
                return outStream;
            }
        }

        private static ImageFormat GetImageType(string imageType)
        {
            try
            {
                if (imageType.ToLower().Equals("png"))
                    return System.Drawing.Imaging.ImageFormat.Png;
                else if (imageType.ToLower().Equals("bmp"))
                    return System.Drawing.Imaging.ImageFormat.Bmp;
                else
                    return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            catch (Exception ex)
            {
                Common.Units.Error.WriteErrorXml("获取图片类型出错:"+ex.Message);
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
        }
    }
}

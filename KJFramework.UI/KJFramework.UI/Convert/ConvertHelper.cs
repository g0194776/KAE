using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;

namespace KJFramework.UI.Convert
{
    /// <summary>
    ///     图形图像类型转换帮助器, 提供了相关的辅助操作。
    /// </summary>
    public class ConvertHelper
    {
        /// <summary>
        ///     将图标类型转换成WPF程序可认的BitmapImage
        /// </summary>
        /// <param name="ico" type="System.Drawing.Icon">
        ///     <para>
        ///         要转换的图标
        ///     </para>
        /// </param>
        /// <returns>
        ///     返回null , 表示转换失败
        /// </returns>
        public static BitmapImage ConvertToWPF(Icon ico)
        {
            if (ico != null)
            {
                MemoryStream Ms = new MemoryStream();
                ico.ToBitmap().Save(Ms, System.Drawing.Imaging.ImageFormat.Png);
                BitmapImage BImage = new BitmapImage();
                BImage.BeginInit();
                BImage.StreamSource = Ms;
                BImage.EndInit();
                return BImage;
            }
            else
            {
                return null;
            }
        }
    }
}

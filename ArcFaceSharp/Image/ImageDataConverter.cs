using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	ImageDataConverter   
*│  项    目：     	ArcFaceSharp.ArcFaceImage                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 15:43:11                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Image
{
    public  class ImageDataConverter
    {
        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr destination, IntPtr source, int length);

        /// <summary>
        /// Bitmap转ImageData同时将宽度不为4的倍数的图像进行调整，注意ImageData在用完之后要用Dispose释放掉
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static ImageData ConvertToImageData(Bitmap bitmap)
        {
            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            int width = (bitmap.Width + 3) / 4 * 4;
            int bytesCount = bmpData.Height * width * 3;
            IntPtr pImageData = Marshal.AllocCoTaskMem(bytesCount);
            if (width == bitmap.Width)
                CopyMemory(pImageData, bmpData.Scan0, bytesCount);
            else
                for (int i = 0; i < bitmap.Width; i++)
                    CopyMemory(IntPtr.Add(pImageData, i * width * 3), IntPtr.Add(bmpData.Scan0, i * bmpData.Stride), bmpData.Stride);
            bitmap.UnlockBits(bmpData);
            return new ImageData(width, bitmap.Height, pImageData);
        }

    }


}

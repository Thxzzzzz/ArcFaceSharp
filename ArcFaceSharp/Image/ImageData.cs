using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ArcFaceSharp.ArcFace;

#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	ImageData   
*│  项    目：     	ArcFaceSharp.ArcFaceImage                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 15:40:44                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Image
{
    public class ImageData:IDisposable
    {
        public ImageData() { }

        public ImageData(int width, int height, IntPtr pImageData, int format  = ArcFacePixelFormat.ASVL_PAF_RGB24_B8G8R8)
        {
            this.Width = width;
            this.Height = height;
            this.PImageData = pImageData;
            this.Format = format;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int Format { get; set; } = ArcFacePixelFormat.ASVL_PAF_RGB24_B8G8R8;

        public IntPtr PImageData { get; set; }

        public void Dispose()
        {
            Marshal.FreeCoTaskMem(PImageData);
        }

    }
}

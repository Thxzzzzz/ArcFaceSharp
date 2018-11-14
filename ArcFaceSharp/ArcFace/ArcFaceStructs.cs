using System;
using System.Drawing;
using System.Runtime.InteropServices;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	ArcFaceStructs   
*│  项    目：     	ArcFaceSharp.ArcFace                                                 
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/6 17:20:06                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.ArcFace
{
     
    /// <summary>
    /// 版本和授权信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential )]
    public struct AsfVersion
    {   
        /// <summary>
        /// 版本号
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string Version;

        /// <summary>
        /// 构建日期 
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string BuildDate;

        /// <summary>
        ///  版权说明 
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string CopyRight;
    }


    /// <summary>
    /// 单人脸信息
    /// </summary>
    public struct AsfSingleFaceInfo
    {
        /// <summary>
        /// 人脸框
        /// </summary>
        public Mrect faceRect;

        /// <summary>
        /// 人脸角度
        /// </summary>
        public int faceOrient;

    }

    /// <summary>
    /// 多人脸信息
    /// </summary>
    public struct AsfMultiFaceInfo
    {
        /// <summary>
        /// 人脸框数组
        /// </summary>
        public IntPtr faceRect;

        /// <summary>
        /// 人脸角度数组
        /// </summary>
        public IntPtr faceOrient;


        /// <summary>
        /// 检测到的人脸个数
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int faceNum;
    }

    /// <summary>
    /// 年龄信息
    /// </summary>
    public struct AsfAgeInfo
    {
        /// <summary>
        /// 年龄结果
        /// </summary>
        public IntPtr ageArray;

        /// <summary>
        /// 检测到人脸的个数
        /// </summary>
        public int num;


    }

    /// <summary>
    /// 性别信息
    /// </summary>
    public struct AsfGenderInfo
    {
        /// <summary>
        /// 0男，1女，-1未知
        /// </summary>
        public IntPtr genderArray;

        /// <summary>
        /// 检测到的人脸的个数
        /// </summary>
        public int num;
    }


    /// <summary>
    /// 人脸特征信息
    /// </summary>
    public struct AsfFaceFeature 
    {
        /// <summary>
        /// 特征信息
        /// </summary>
        public IntPtr feature;

        /// <summary>
        /// 人脸特征的长度
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public int featureSize;

//        public void Dispose()
//        {
//            Marshal.FreeCoTaskMem(feature); ;
//        }
    }

    /// <summary>
    /// 3D角度信息
    /// </summary>
    public struct AsfFace3DAngle
    {
        /// <summary>
        /// 横滚角度
        /// </summary>
        public IntPtr roll;

        /// <summary>
        /// 偏航角度
        /// </summary>
        public IntPtr yaw;

        /// <summary>
        /// 俯仰角度
        /// </summary>
        public IntPtr pitch;

        /// <summary>
        /// 0为正常
        /// </summary>
        public IntPtr status;

        /// <summary>
        /// 检测到人脸的个数
        /// </summary>
        public int num;
    }

    /// <summary>
    /// 人脸框信息
    /// </summary>
    public struct Mrect
    {
        /// <summary>
        /// 左距离
        /// </summary>
        public int left;

        /// <summary>
        /// 上距离
        /// </summary>
        public int top;

        /// <summary>
        /// 右距离
        /// </summary>
        public int right;

        /// <summary>
        /// 下距离
        /// </summary>
        public int bottom;

       
        public Rectangle Rectangle => new Rectangle(left, top, right - left, bottom - top);

    }
 

}

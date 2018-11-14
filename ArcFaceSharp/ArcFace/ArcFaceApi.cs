using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	API   
*│  项    目：     	ArcFaceSharp.ArcFace                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/6 17:32:38                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.ArcFace
{
    public static class ArcFaceApi
    {
        #region Wrapper

        /// <summary>
        /// 激活SDK
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="sdkKey"></param>
        /// <returns>0:激活成功，0x16002表示已经激活</returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFActivation", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFActivation(string appId, string sdkKey);


        /// <summary>
        /// 初始化引擎（每次使用虹软只调用一次即可）
        /// </summary>
        /// <param name="detectMode">video模式或者image模式</param>
        /// <param name="detectFaceOrientPriority">检测脸部较低的优先值</param>
        /// <param name="detectFaceScaleVal">数值化的最小人脸尺寸，视频[2,16]/图片[2,32]，推荐值16</param>
        /// <param name="detectFaceMaxNum">最大检测人脸的个数[1,50]</param>
        /// <param name="combinedMask">要用到的引擎组合</param>
        /// <param name="pEngine">初始化返回的引擎handle</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFInitEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFInitEngine(uint detectMode, int detectFaceOrientPriority, int detectFaceScaleVal, int detectFaceMaxNum, uint combinedMask, out IntPtr pEngine);

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <param name="hEngine">引擎 handle </param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFGetVersion", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr ASFGetVersion(IntPtr hEngine);


        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图片宽度4的倍数，大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="faceInfo">检测到的人脸信息</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFDetectFaces", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFDetectFaces(IntPtr pEngine, int width, int height, int format, IntPtr pImageData, out AsfMultiFaceInfo faceInfo);



        /// <summary>
        /// 单人脸特征提取
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图片宽度为4的倍数且大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="faceInfo">单张人脸位置和角度信息</param>
        /// <param name="faceFeature">人脸特征</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFFaceFeatureExtract", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFFaceFeatureExtract(IntPtr pEngine, int width, int height, int format, IntPtr pImageData, ref AsfSingleFaceInfo faceInfo, out AsfFaceFeature faceFeature);




        /// <summary>
        /// 人脸特征比对
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="faceFeature1">待比对的人脸特征</param>
        /// <param name="faceFeature2">待比对的人脸特征</param>
        /// <param name="result">比对结果，置信度数值</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFFaceFeatureCompare", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFFaceFeatureCompare(IntPtr pEngine, ref AsfFaceFeature faceFeature1, ref AsfFaceFeature faceFeature2, out float result);

 

        /// <summary>
        /// 销毁引擎
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFUninitEngine", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFUninitEngine(IntPtr engine);


        #endregion



        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="width">图片宽度为4的倍数且大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="combinedMask">初始化中参数combinedMask与ASF_AGE| ASF_GENDER| ASF_FACE3DANGLE的交集的子集</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFProcess", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFProcess(IntPtr pEngine, int width, int height, int format, IntPtr pImageData, ref AsfMultiFaceInfo detectedFaces, uint combinedMask);

        /// <summary>
        /// 获取年龄信息
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="ageInfo">检测到的年龄信息</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFGetAge", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFGetAge(IntPtr pEngine, ref AsfAgeInfo ageInfo);

        /// <summary>
        /// 获取性别信息
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="genderInfo">检测到的性别信息</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFGetGender", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFGetGender(IntPtr pEngine, ref AsfGenderInfo genderInfo);

        /// <summary>
        /// 获取3D角度信息
        /// </summary>
        /// <param name="pEngine">引擎handle</param>
        /// <param name="p3DAngleInfo">检测到脸部3D 角度信息</param>
        /// <returns></returns>
        [DllImport("libarcsoft_face_engine.dll", EntryPoint = "ASFGetFace3DAngle", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ASFGetFace3DAngle(IntPtr pEngine, ref AsfFace3DAngle p3DAngleInfo);
    }
}

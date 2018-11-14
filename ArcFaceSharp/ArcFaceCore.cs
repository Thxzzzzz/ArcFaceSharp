using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using ArcFaceSharp.ArcFace;
using ArcFaceSharp.Exceptions;
using ArcFaceSharp.Extensions;
using ArcFaceSharp.Image;
using ArcFaceSharp.Model;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	ArcFaceCore   
*│  项    目：     	ArcFaceSharp                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 15:46:29                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp
{
    public class ArcFaceCore :IDisposable
    {
        #region Value

        /// <summary>
        /// ArcFace引擎句柄
        /// </summary>
        public IntPtr EngineHandle { get; private set; } 

        /// <summary>
        /// APP_ID
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// SDK_KEY
        /// </summary>
        public string SdkKey { get; private set; }

        /// <summary>
        /// 版本信息
        /// </summary>
        public AsfVersion VersionInfo { get; private set; }

        /// <summary>
        /// 人脸检测模式 Video or Image
        /// </summary>
        public string DetectMode { get; private set; }

        /// <summary>
        /// 用到的引擎组合
        /// </summary>
        public uint CombinedMask { get; private set; }

        /// <summary>
        /// 检测脸部角度的优先值
        /// </summary>
        public DetectionOrientPriority DetectFaceOrientPriority { get; private set; }

        #endregion

        #region DllImport

        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr Destination, IntPtr Source, int Length);

        #endregion

        #region Constructor & Init

        /// <summary>
        /// ArcFace构造函数，在这里将完成ArcFace引擎的初始化
        /// </summary>
        /// <param name="appId">APP_ID</param>
        /// <param name="sdkKey">SDK_KEY</param>
        /// <param name="detectMode">video模式或者image模式</param>
        /// <param name="combinedMask">要用到的引擎组合 定义的常量在ArcFaceFunction下</param>
        /// <param name="detectFaceOrientPriority">检测脸部角度的优先值 默认仅0度</param>
        /// <param name="detectFaceMaxNum">最大检测人脸的个数[1,50] 默认25</param>
        /// <param name="detectFaceScaleVal">数值化的最小人脸尺寸，视频[2,16]/图片[2,32]，推荐值16 默认16</param>
        public ArcFaceCore(string appId, string sdkKey, uint detectMode, uint combinedMask,
            DetectionOrientPriority detectFaceOrientPriority = DetectionOrientPriority.ASF_OP_0_ONLY, int detectFaceMaxNum = 25, int detectFaceScaleVal = 16)
        {
            this.Init(appId, sdkKey, detectMode, combinedMask, (int)detectFaceOrientPriority, detectFaceMaxNum,detectFaceScaleVal);
            this.AppId = appId;
            this.SdkKey = sdkKey;
            switch (detectMode)
            {
                case ArcFaceDetectMode.VIDEO: this.DetectMode = "Video"; break;
                case ArcFaceDetectMode.IMAGE: this.DetectMode = "Image"; break; ;
            }
            this.DetectFaceOrientPriority = detectFaceOrientPriority;
            this.CombinedMask = combinedMask;
        }


        /// <summary>
        /// 引擎激活及初始化
        /// </summary>
        /// <param name="appId">APP_ID</param>
        /// <param name="sdkKey">SDK_KEY</param>
        /// <param name="detectMode">video模式或者image模式</param>
        /// <param name="combinedMask">要用到的引擎组合 定义的常量在ArcFaceFunction下</param>
        /// <param name="detectFaceOrientPriority">检测脸部角度的优先值 默认仅0度</param>
        /// <param name="detectFaceMaxNum">最大检测人脸的个数[1,50] 默认25</param>
        /// <param name="detectFaceScaleVal">数值化的最小人脸尺寸，视频[2,16]/图片[2,32]，推荐值16 默认16</param>
        private void Init(string appId, string sdkKey, uint detectMode, uint combinedMask, 
            int detectFaceOrientPriority = (int)DetectionOrientPriority.ASF_OP_0_ONLY, int detectFaceMaxNum = 25, int detectFaceScaleVal = 16)
        {
            //TODO:引擎激活，只需要第一次运行的时候激活？
            ResultCode activeResult = (ResultCode)ArcFaceApi.ASFActivation(appId, sdkKey);

            if (activeResult == ResultCode.设备不匹配)
            {
                if (File.Exists("asf_install.dat"))
                {
                    File.Delete("asf_install.dat");
                }

                if (File.Exists("freesdk_132000.dat"))
                {
                    File.Delete("freesdk_132000.dat");
                }
                activeResult = (ResultCode)ArcFaceApi.ASFActivation(appId, sdkKey);
            }

            if (activeResult != ResultCode.成功 && activeResult!= ResultCode.SDK已激活)
            {
                throw new ResultCodeException(activeResult);
            }

            ResultCode initResult = (ResultCode)ArcFaceApi.ASFInitEngine(detectMode,
                detectFaceOrientPriority,
                detectFaceScaleVal, detectFaceMaxNum,
                combinedMask,
                out IntPtr pEngine);

            if (initResult != ResultCode.成功)
            {
                throw new ResultCodeException(initResult);
            }

            EngineHandle = pEngine;

            this.VersionInfo = AsfGetVersion(pEngine);

        }

        #endregion

        #region FaceDetection

        /// <summary>
        /// 人脸检测 后续如需要人脸识别则不推荐使用这个接口，建议用 ImageDataConverter 转换成 ImageData 再使用别的接口
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns></returns>
        public MultiFaceModel FaceDetection(Bitmap bitmap, bool deepCopy = true)
        {
            MultiFaceModel result;
            using (ImageData imgData = ImageDataConverter.ConvertToImageData(bitmap))
            {
                result = FaceDetection(imgData, deepCopy);
            }
            return result;
        }

        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="imgData">图像数据 可用 ImageDataConverter 转换</param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns></returns>
        public MultiFaceModel FaceDetection(ImageData imgData, bool deepCopy = true)
        {
            return FaceDetection(imgData.Width, imgData.Height, imgData.Format, imgData.PImageData, deepCopy);
        }


        /// <summary>
        /// 人脸检测
        /// </summary>
        /// <param name="width">图片宽度,必须为4的倍数</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns></returns>
        public MultiFaceModel FaceDetection(int width, int height, int format, IntPtr pImageData ,bool deepCopy = true)
        {
            ResultCode resultCode =
                (ResultCode)ArcFaceApi.ASFDetectFaces(EngineHandle,
                    width, height, format, pImageData,
                    out AsfMultiFaceInfo multiFaceInfo);

            if (resultCode != ResultCode.成功)
            {
                throw new ResultCodeException(resultCode);
            }

            if (!deepCopy) return new MultiFaceModel(multiFaceInfo);
            int faceRectSize = Marshal.SizeOf<Mrect>() * multiFaceInfo.faceNum;
            int faceOrientSize = Marshal.SizeOf<int>() * multiFaceInfo.faceNum; ;
            AsfMultiFaceInfo multiFaceInfoCopy = new AsfMultiFaceInfo()
            {
                faceRect = Marshal.AllocCoTaskMem(faceRectSize),
                faceOrient = Marshal.AllocCoTaskMem(faceOrientSize),
                faceNum = multiFaceInfo.faceNum
             };
 
            CopyMemory(multiFaceInfoCopy.faceRect,multiFaceInfo.faceRect, faceRectSize);
            CopyMemory(multiFaceInfoCopy.faceOrient, multiFaceInfo.faceOrient, faceOrientSize);
            return new MultiFaceModel(multiFaceInfoCopy);

        }

        #endregion

        #region FaceRecognition

        /// <summary>
        /// 单人脸特征提取
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="faceInfo">单张人脸位置和角度信息</param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns>人脸特征信息</returns>
        public AsfFaceFeature FaceFeatureExtract(Bitmap bitmap, ref AsfSingleFaceInfo faceInfo, bool deepCopy = true)
        {
            AsfFaceFeature result;
            using (ImageData imgData = ImageDataConverter.ConvertToImageData(bitmap))
            {
                result= FaceFeatureExtract(imgData, ref faceInfo, deepCopy);
            }
            return result;
        }


        /// <summary>
        /// 单人脸特征提取
        /// </summary>
        /// <param name="imgData">图像数据 可用 ImageDataConverter 转换</param>
        /// <param name="faceInfo">单张人脸位置和角度信息</param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns>人脸特征信息</returns>
        public AsfFaceFeature FaceFeatureExtract(ImageData imgData, ref AsfSingleFaceInfo faceInfo, bool deepCopy = true)
        {
            return FaceFeatureExtract(imgData.Width, imgData.Height, imgData.Format, imgData.PImageData,ref faceInfo, deepCopy);
        }


        /// <summary>
        /// 单人脸特征提取
        /// </summary>
        /// <param name="width">图片宽度为4的倍数且大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="faceInfo">单张人脸位置和角度信息</param>
        /// <param name="deepCopy">返回结果是否进行深拷贝,默认为true,若设置为false则每次调用会覆盖上一次的结果(内存覆盖)</param>
        /// <returns>人脸特征信息</returns>
        public AsfFaceFeature FaceFeatureExtract(int width, int height, int format, IntPtr pImageData, ref AsfSingleFaceInfo faceInfo,bool deepCopy = true)
        {
            ResultCode result = (ResultCode) ArcFaceApi.ASFFaceFeatureExtract(EngineHandle, width, height, format,
                pImageData, ref faceInfo,
                out AsfFaceFeature faceFeature);
            if (result != ResultCode.成功)
            {
                throw new ResultCodeException(result);
            }

            if (!deepCopy) return faceFeature;

            AsfFaceFeature faceFeatureCopy = new AsfFaceFeature()
            {
                feature = Marshal.AllocCoTaskMem(faceFeature.featureSize),
                featureSize = faceFeature.featureSize
            };
            CopyMemory(faceFeatureCopy.feature, faceFeature.feature,faceFeature.featureSize);
            return faceFeatureCopy;
        }


        /// <summary>
        /// 人脸特征比对
        /// </summary>
        /// <param name="faceFeature1">待比对的人脸特征</param>
        /// <param name="faceFeature2">待比对的人脸特征</param>
        /// <returns>人脸对结果值，为 0-1 之间的浮点数</returns>
        public float FaceCompare(AsfFaceFeature faceFeature1,AsfFaceFeature faceFeature2)
        {
            ResultCode result = (ResultCode)ArcFaceApi.ASFFaceFeatureCompare(this.EngineHandle, ref faceFeature1, ref faceFeature2, out float score);

            if (result != ResultCode.成功)
            {
                throw new ResultCodeException(result);
            }
            return score;
        }






        #endregion

        #region Age Gender Face3DAngle

        #region FaceProcess


        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测，超过部分返回未知
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="combinedMask">初始化中参数combinedMask与ASF_AGE| ASF_GENDER| ASF_FACE3DANGLE的交集的子集</param>
        public void FaceProcess(Bitmap bitmap, MultiFaceModel detectedFaces, uint combinedMask = ArcFaceFunction.AGE | ArcFaceFunction.FACE_3DANGLE | ArcFaceFunction.GENDER)
        {
            using (ImageData imgData = ImageDataConverter.ConvertToImageData(bitmap))
            {
                FaceProcess(imgData.Width, imgData.Height, imgData.Format, imgData.PImageData, detectedFaces, combinedMask);
            }
        }


        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测，超过部分返回未知
        /// </summary>
        /// <param name="imgData">图像数据 可用 ImageDataConverter 转换</param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="combinedMask">初始化中参数combinedMask与ASF_AGE| ASF_GENDER| ASF_FACE3DANGLE的交集的子集</param>
        public void FaceProcess(ImageData imgData, MultiFaceModel detectedFaces, uint combinedMask = ArcFaceFunction.AGE | ArcFaceFunction.FACE_3DANGLE | ArcFaceFunction.GENDER)
        {
            FaceProcess(imgData.Width, imgData.Height, imgData.Format, imgData.PImageData, detectedFaces, combinedMask);
        }


        /// <summary>
        /// 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测，超过部分返回未知
        /// </summary>
        /// <param name="width">图片宽度为4的倍数且大于0</param>
        /// <param name="height">YUYV/I420/NV21/NV12格式的图片高度为2的倍数，BGR24格式的图片高度不限制</param>
        /// <param name="format">颜色空间格式</param>
        /// <param name="pImageData">图片数据</param>
        /// <param name="detectedFaces">检测到的人脸信息</param>
        /// <param name="combinedMask">初始化中参数combinedMask与ASF_AGE| ASF_GENDER| ASF_FACE3DANGLE的交集的子集</param>
        public void FaceProcess(int width, int height, int format, IntPtr pImageData, MultiFaceModel detectedFaces, uint combinedMask = ArcFaceFunction.AGE | ArcFaceFunction.FACE_3DANGLE | ArcFaceFunction.GENDER)
        {
            //取交集
            combinedMask = combinedMask & this.CombinedMask;
            AsfMultiFaceInfo multiFaceInfo = detectedFaces.MultiFaceInfo;
            ResultCode result = (ResultCode) ArcFaceApi.ASFProcess(EngineHandle, width, height, format, pImageData,
                ref multiFaceInfo, combinedMask);
            if (result != ResultCode.成功)
            {
                throw new ResultCodeException(result);
            }
        }

        #endregion

        /// <summary>
        /// 获取年龄信息
        /// </summary>
        /// <returns>年龄信息列表</returns>
        public List<int> GetAge()
        {
            AsfAgeInfo ageInfo = new AsfAgeInfo();
            
            ArcFaceApi.ASFGetAge(EngineHandle, ref ageInfo);
            return ageInfo.ageArray.ToStructArray<int>(ageInfo.num).ToList(); 
        }


        /// <summary>
        /// 获取性别信息
        /// </summary>
        /// <returns>性别信息列表 0男，1女，-1未知</returns>
        public List<int> GetGender()
        {
            AsfGenderInfo genderInfo = new AsfGenderInfo();
            ResultCode result = (ResultCode) ArcFaceApi.ASFGetGender(EngineHandle, ref genderInfo);
            if (result != ResultCode.成功)
            {
                throw new ResultCodeException(result);
            }
            return genderInfo.genderArray.ToStructArray<int>(genderInfo.num).ToList();
        }

        /// <summary>
        /// 获取3D角度信息
        /// </summary>
        /// <returns>3D角度信息列表</returns>
        public List<Face3DAngleModel> GetFace3DAngle()
        {
            AsfFace3DAngle p3DAngleInfo = new AsfFace3DAngle();
            ResultCode result = (ResultCode) ArcFaceApi.ASFGetFace3DAngle(EngineHandle,ref p3DAngleInfo);
            if (result != ResultCode.成功)
            {
                throw new ResultCodeException(result);
            }
           float[] roll = p3DAngleInfo.roll.ToStructArray<float>(p3DAngleInfo.num);
           float[] yaw =  p3DAngleInfo.yaw.ToStructArray<float>(p3DAngleInfo.num);
           float[] pitch =  p3DAngleInfo.pitch.ToStructArray<float>(p3DAngleInfo.num);
           int[] status = p3DAngleInfo.status.ToStructArray<int>(p3DAngleInfo.num);
           List<Face3DAngleModel> face3DAngleList = new List<Face3DAngleModel>();
            for (int i = 0; i < p3DAngleInfo.num; i++)
            {
                Face3DAngleModel face3DAngle = new Face3DAngleModel();
                face3DAngle.roll = roll[i];
                face3DAngle.yaw = yaw[i];
                face3DAngle.pitch = pitch[i];
                face3DAngle.status = status[i];
                face3DAngleList.Add(face3DAngle);
            }
            return face3DAngleList;
        }


        #endregion

        #region Version

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public AsfVersion AsfGetVersion()
        {
            return AsfGetVersion(this.EngineHandle);
        }


        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <param name="hEngine">引擎 handle </param>
        /// <returns></returns>
        public static AsfVersion AsfGetVersion(IntPtr pEngine)
        {
            return (AsfVersion)Marshal.PtrToStructure(ArcFaceApi.ASFGetVersion(pEngine), typeof(AsfVersion));
        }



        #endregion

        #region UnInit & Dispose

        /// <summary>
        ///  销毁引擎
        /// </summary>
        public void UnInit()
        {
            ArcFaceApi.ASFUninitEngine(EngineHandle);
        }


        public void Dispose()
        {
            UnInit();
        }

        #endregion

    }
}

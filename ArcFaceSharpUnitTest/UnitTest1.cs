using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ArcFaceSharp;
using ArcFaceSharp.ArcFace;
using ArcFaceSharp.Image;
using ArcFaceSharp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArcFaceSharpUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            // SDK对应的 APP_ID SDK_KEY
            string APP_ID = @"7NK7KSpfgxdqb74r8nvy36kDwH3wVGstr2LHGHBxQ8LY";

            string SDK_KEY = @"3fD8vKYMNfPzKHMoqppjA9chGh2aGkWzUQNFiAj7Yq63";

            // 加载图片
            Bitmap heying = new Bitmap(@"heying.jpg");

            Bitmap face1 = new Bitmap(@"ldh0.jpg");
            Bitmap face2 = new Bitmap(@"ldh1.jpg");

            Bitmap face3 = new Bitmap(@"zxy0.jpg");

            // 创建 ArcFaceCore 对象，向构造函数传入相关参数进行 ArcFace 引擎的初始化
            ArcFaceCore arcFace = new ArcFaceCore(APP_ID,SDK_KEY,ArcFaceDetectMode.IMAGE,
                ArcFaceFunction.FACE_DETECT | ArcFaceFunction.FACE_RECOGNITION | ArcFaceFunction.AGE | ArcFaceFunction.FACE_3DANGLE | ArcFaceFunction.GENDER,DetectionOrientPriority.ASF_OP_0_ONLY,50,32);

            // 将 Bitmap 转换成 ImageData
            ImageData heyingImgData = ImageDataConverter.ConvertToImageData(heying);

            // 人脸检测
            // 也可直接传入 Bitmap 来调用相关接口 会自动转换成 ImageData，但这里推荐用 ImageData
            MultiFaceModel multiFaceB = arcFace.FaceDetection(heying);
            // 传入 ImageData ，推荐使用这个接口
            MultiFaceModel multiFace = arcFace.FaceDetection(heyingImgData);

            // 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测，超过部分返回未知 这是官方文档的说明
            arcFace.FaceProcess(heyingImgData, multiFace);

            // 获取年龄信息
            List<int> ageList = arcFace.GetAge();
            // 获取性别信息
            List<int> genderList = arcFace.GetGender();
            // 获取人脸角度信息
            List<Face3DAngleModel> face3DAngleList = arcFace.GetFace3DAngle();


            // 将第一张图片的 Bitmap 转换成 ImageData
            ImageData faceData1 = ImageDataConverter.ConvertToImageData(face1);

            // 检测第一张图片中的人脸
            MultiFaceModel multiFace1 =  arcFace.FaceDetection(faceData1);
             
            // 取第一张图片中返回的第一个人脸信息
            AsfSingleFaceInfo faceInfo1 = multiFace1.FaceInfoList.First();

            // 提第一张图片中返回的第一个人脸的特征
            AsfFaceFeature faceFeature1 = arcFace.FaceFeatureExtract(faceData1, ref faceInfo1);



            ImageData faceData2 = ImageDataConverter.ConvertToImageData(face2);

            // 检测第二张图片中的人脸
            MultiFaceModel multiFace2 = arcFace.FaceDetection(faceData2);

            // 取第二张图片中返回的第一个人脸信息
            AsfSingleFaceInfo faceInfo2 = multiFace2.FaceInfoList.First();

            // 提第二张图片中返回的第一个人脸的特征
            AsfFaceFeature faceFeature2 = arcFace.FaceFeatureExtract(faceData2, ref faceInfo2);



            // face1 face2 人脸对比，将会返回一个 0-1 之间的浮点数值
            float result = arcFace.FaceCompare(faceFeature1, faceFeature2);



            ImageData faceData3 = ImageDataConverter.ConvertToImageData(face3);

            // 检测第二张图片中的人脸
            MultiFaceModel multiFace3 = arcFace.FaceDetection(faceData3);

            // 取第二张图片中返回的第一个人脸信息
            AsfSingleFaceInfo faceInfo3 = multiFace3.FaceInfoList.First();

            // 提第二张图片中返回的第一个人脸的特征
            AsfFaceFeature faceFeature3 = arcFace.FaceFeatureExtract(faceData3, ref faceInfo3);

            // face1 face3 人脸对比，将会返回一个 0-1 之间的浮点数值
            float result2 = arcFace.FaceCompare(faceFeature1, faceFeature3);


            // 释放销毁引擎
            arcFace.Dispose();
            // ImageData使用完之后记得要 Dispose 否则会导致内存溢出 
            faceData1.Dispose();
            faceData2.Dispose();
            // BItmap也要记得 Dispose
            face1.Dispose();
            face2.Dispose();

           
         
        }

     
    }
}

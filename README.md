### ArcFaceSharp

[![NuGet](https://img.shields.io/badge/nuget-1.0.3-green.svg)](https://www.nuget.org/packages/ArcFaceSharp/)



ArcFaceSharp 是 ArcSoft 虹软 ArcFace 2.0 SDK （http://ai.arcsoft.com.cn/product/arcface.html） 的一个 C# 封装库，为方便进行 C# 开发而封装。欢迎 Start & Fork。

### 使用

在 Nuget 搜索 ArcFaceSharp 安装。

```powershell
PM> Install-Package ArcFaceSharp -Version 1.0.3
```

或者下载dll导入。

导入 ArcFaceSharp 后，将自己申请到的 ArcFace2.0 SDK 的 dll 文件 （libarcsoft_face.dll 和 libarcsoft_face_engine.dll）放在程序的运行目录下。

接口调用的流程可参考官方文档的流程图（http://ai.arcsoft.com.cn/manual/arcface_windows_guideV2.html 2.1.5调用流程）
- ![lct](/lct.jpg)

### Linux版本 
测试环境center os 7,netcore-sdk-5.1

下载编译netcore分支，将自己申请到的 ArcFace2.0 SDK 的 so 文件（libarcsoft_face.so 和 libarcsoft_face_engine.so）拷到/lib64目录下

运行lld libarcsoft_face_engine.so 查看依赖

  库依赖 GLIBC 2.17 及以上
  
  库依赖 GLIBCXX 3.4.19 及以上
  
  编译器 GCC 4.8.2 及以上

##### 主要 API

   具体参数和含义可以自行查看方法的注释

- 激活及初始化  

  创建 ArcFaceCore对象即可

```C#
ArcFaceCore arcFaceCore = ArcFaceCore(appId, sdkKey, detectMode, combinedMask,detectFaceOrientPriority, detectFaceMaxNum,detectFaceScaleVal)；
```

-  将 Bitmap 转换成 ImageData

```C#
ImageData imageData = ImageDataConverter.ConvertToImageData(bitmap)；
```



---



以下方法都是 ArcFaceCore 中的方法

-  人脸检测

```C#
MultiFaceModel multiFaceModel = arcFaceCore.FaceDetection(imageData);
```

- 人脸信息检测（年龄/性别/人脸3D角度）最多支持4张人脸信息检测，超过部分返回未知
```C#
// 人脸信息检测 先调用这个接口才能获取以下三个信息
arcFaceCore.FaceProcess(imageData,multiFaceModel);
//获取年龄信息
List<int> ageList = arcFaceCore.GetAge();
// 获取性别信息
List<int> genderList = arcFace.GetGender();
// 获取人脸角度信息
List<Face3DAngleModel> face3DAngleList = arcFace.GetFace3DAngle();
```

- 人脸特征值提取

  asfSingleFaceInfo 为人脸检测接口返回的人脸信息中的其中一个人脸信息
```C#
AsfFaceFeature asfFaceFeature = arcFace.FaceFeatureExtract(imageData, ref asfSingleFaceInfo);
```

- 人脸对比

```C#
 float result = arcFace.FaceCompare(asfFaceFeature1, asfFaceFeature2);
```

- 异常捕获

  以人脸特征提取为例,当接口返回值不为 0(成功)时，则会抛出 ResultCodeException 异常。

 ```C#
try
{
	AsfFaceFeature asfFaceFeature = arcFace.FaceFeatureExtract(imageData, ref asfSingleFaceInfo);
}
catch (ResultCodeException e)
{
    Console.WriteLine(e.ResultCode);
    throw;
}
 ```



---



代码示例：

> \ArcFaceSharpUnitTest\UnitTest1.cs

```C#
      public void TestMethod1()
        {

            // SDK对应的 APP_ID SDK_KEY
            string APP_ID = @"7NK7KSpfgxdqb74r8nvy36kDwH3wVGstr2LHGHBxQ8LY";
 
            string SDK_KEY =  @"3fD8vKYMNfPzKHMoqppjA9chGh2aGkWzUQNFiAj7Yq63";

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

            // 检测第三张图片中的人脸
            MultiFaceModel multiFace3 = arcFace.FaceDetection(faceData3);

            // 取第三张图片中返回的第一个人脸信息
            AsfSingleFaceInfo faceInfo3 = multiFace3.FaceInfoList.First();

            // 提第三张图片中返回的第一个人脸的特征
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
```





### 感谢
本项目参考了以下开发者的一些思路和代码，在此表示感谢。
> C#_Demo_摄像头实时_4线程人脸识别_注册 - Demo 分享 - 虹软人工智能引擎开发者论坛 - Powered by Discuz!
> https://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=673&extra=page%3D1

> 虹软2.0版本人脸检测C#类库分享 - 第2页 - ArcFace - 虹软人工智能引擎开发者论坛 - Powered by Discuz!
> https://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=1274&extra=page%3D1&page=2

> C#人脸检测与动态人脸识别显示坐标 视频人脸识别WINFORM - ArcFace - 虹软人工智能引擎开发者论坛 - Powered by Discuz!
> https://ai.arcsoft.com.cn/bbs/forum.php?mod=viewthread&tid=648&extra=page%3D1
> 



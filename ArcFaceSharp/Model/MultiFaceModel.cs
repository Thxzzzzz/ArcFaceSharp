using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ArcFaceSharp.ArcFace;
using ArcFaceSharp.Extensions;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	MultiFaceModel   
*│  项    目：     	ArcFaceSharp.Model                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 17:02:20                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Model
{
    /// <summary>
    /// 多人脸信息
    /// </summary>
    public class MultiFaceModel :IDisposable
    {
        /// <summary>
        ///  多人脸信息
        /// </summary>
        public AsfMultiFaceInfo MultiFaceInfo { get; private set; }

        /// <summary>
        /// 单人脸信息List
        /// </summary>
        public List<AsfSingleFaceInfo> FaceInfoList { get; private set; }

        public MultiFaceModel(){}

        public MultiFaceModel(AsfMultiFaceInfo multiFaceInfo)
        {
            this.MultiFaceInfo = multiFaceInfo;
            this.FaceInfoList = new List<AsfSingleFaceInfo>();
            Mrect[] faceRects = multiFaceInfo.faceRect.ToStructArray<Mrect>(multiFaceInfo.faceNum);
            int[] faceOrient = multiFaceInfo.faceOrient.ToStructArray<int>(multiFaceInfo.faceNum);
            for (int i = 0; i < multiFaceInfo.faceNum; i++)
            {
                AsfSingleFaceInfo faceInfo = new AsfSingleFaceInfo
                {
                    faceOrient = faceOrient[i],
                    faceRect = faceRects[i]
                };
                FaceInfoList.Add(faceInfo);
            }
        }


        public void Dispose()
        {
            Marshal.FreeCoTaskMem(MultiFaceInfo.faceRect);
            Marshal.FreeCoTaskMem(MultiFaceInfo.faceOrient);
        }

    }

}

using System.Drawing;
using ArcFaceSharp.ArcFace;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	SingleFaceModel   
*│  项    目：     	ArcFaceSharp.Model                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 17:02:34                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Model
{
    /// <summary>
    /// 单人脸信息
    /// </summary>
    public class SingleFaceModel
    {
        /// <summary>
        /// 人脸框
        /// </summary>
        public Rectangle FaceRect { get; set; }

        /// <summary>
        /// 人相角度
        /// </summary>
        public int FaceOrient { get; set; }


        public SingleFaceModel(){}

        public SingleFaceModel(AsfSingleFaceInfo singleFaceInfo)
        {
            this.FaceRect = singleFaceInfo.faceRect.Rectangle;
            this.FaceOrient = singleFaceInfo.faceOrient;
        }

    }

}

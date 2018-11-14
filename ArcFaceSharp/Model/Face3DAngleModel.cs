using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	Face3DAngleModel   
*│  项    目：     	ArcFaceSharp.Model                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/14 15:52:23                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Model
{
    public class Face3DAngleModel
    {
        /// <summary>
        /// 横滚角度
        /// </summary>
        public float roll { get; set; }

        /// <summary>
        /// 偏航角度
        /// </summary>
        public float yaw { get; set; }

        /// <summary>
        /// 俯仰角度
        /// </summary>
        public float pitch { get; set; }

        /// <summary>
        /// 0为正常
        /// </summary>
        public int status { get; set; }
    }
}

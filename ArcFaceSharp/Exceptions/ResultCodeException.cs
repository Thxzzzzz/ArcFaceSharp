using System;
using ArcFaceSharp.ArcFace;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	ResultCodeException   
*│  项    目：     	ArcFaceSharp.Exceptions                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 18:56:38                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.Exceptions
{
    /// <summary>
    /// ResultCode 错误码类型的异常
    /// </summary>
    public class ResultCodeException : Exception
    {
        public ResultCode ResultCode { get; }

        public ResultCodeException(ResultCode resultCode) : base(resultCode.ToString())
        {
            this.ResultCode = resultCode;
        }
    }
}

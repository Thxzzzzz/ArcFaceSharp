using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region FileInfo
/**
*┌──────────────────────────────────────────────────────────────┐
*│　类    名：      	Constants   
*│  项    目：     	ArcFaceSharp.ArcFace                                                    
*│　作	   者：       	tanghx                                           
*│　创建时间：        	2018/11/7 10:57:24                                          
*│　描	   述：                           
*└──────────────────────────────────────────────────────────────┘
*/
#endregion
namespace ArcFaceSharp.ArcFace
{
    public class ArcFaceDetectMode
    {
        /// <summary>
        /// Video模式，一般用于多帧连续检测
        /// </summary>
        public const uint VIDEO = 0x00000000;

        /// <summary>
        /// Image模式，一般用于静态图的单次检测
        /// </summary>
        public const uint IMAGE = 0xFFFFFFFF;  
    }

   public class ArcFaceFunction
    {
      

        public const uint NONE = 0x00000000;
        /// <summary>
        /// 人脸检测 此处detect可以是tracking或者detection两个引擎之一，具体的选择由detect mode 确定
        /// </summary>
        public const uint FACE_DETECT = 0x00000001; 

        /// <summary>
        /// 人脸识别
        /// </summary>
        public const uint FACE_RECOGNITION = 0x00000004;

        /// <summary>
        /// 年龄检测
        /// </summary>
        public const uint AGE = 0x00000008;

        /// <summary>
        /// 性别检测
        /// </summary>
        public const uint GENDER = 0x00000010;

        /// <summary>
        /// 人脸角度检测
        /// </summary>
        public const uint FACE_3DANGLE = 0x00000020;
    }


    public class ArcFacePixelFormat
    {
        /// ASVL_PAF_RGB16_B5G6R5 -> 0x101
        public const int ASVL_PAF_RGB16_B5G6R5 = 257;

        /// ASVL_PAF_RGB16_B5G5R5 -> 0x102
        public const int ASVL_PAF_RGB16_B5G5R5 = 258;

        /// ASVL_PAF_RGB16_B4G4R4 -> 0x103
        public const int ASVL_PAF_RGB16_B4G4R4 = 259;

        /// ASVL_PAF_RGB16_B5G5R5T -> 0x104
        public const int ASVL_PAF_RGB16_B5G5R5T = 260;

        /// ASVL_PAF_RGB16_R5G6B5 -> 0x105
        public const int ASVL_PAF_RGB16_R5G6B5 = 261;

        /// ASVL_PAF_RGB16_R5G5B5 -> 0x106
        public const int ASVL_PAF_RGB16_R5G5B5 = 262;

        /// ASVL_PAF_RGB16_R4G4B4 -> 0x107
        public const int ASVL_PAF_RGB16_R4G4B4 = 263;



        /// ASVL_PAF_RGB24_B8G8R8 -> 0x201
        public const int ASVL_PAF_RGB24_B8G8R8 = 513;

        /// ASVL_PAF_RGB24_B6G6R6 -> 0x202
        public const int ASVL_PAF_RGB24_B6G6R6 = 514;

        /// ASVL_PAF_RGB24_B6G6R6T -> 0x203
        public const int ASVL_PAF_RGB24_B6G6R6T = 515;

        /// ASVL_PAF_RGB24_R8G8B8 -> 0x204
        public const int ASVL_PAF_RGB24_R8G8B8 = 516;

        /// ASVL_PAF_RGB24_R6G6B6 -> 0x205
        public const int ASVL_PAF_RGB24_R6G6B6 = 517;





        /// ASVL_PAF_RGB32_B8G8R8 -> 0x301
        public const int ASVL_PAF_RGB32_B8G8R8 = 769;

        /// ASVL_PAF_RGB32_B8G8R8A8 -> 0x302
        public const int ASVL_PAF_RGB32_B8G8R8A8 = 770;

        /// ASVL_PAF_RGB32_R8G8B8 -> 0x303
        public const int ASVL_PAF_RGB32_R8G8B8 = 771;

        /// ASVL_PAF_RGB32_A8R8G8B8 -> 0x304
        public const int ASVL_PAF_RGB32_A8R8G8B8 = 772;

        /// ASVL_PAF_RGB32_R8G8B8A8 -> 0x305
        public const int ASVL_PAF_RGB32_R8G8B8A8 = 773;




        /// ASVL_PAF_YUV -> 0x401
        public const int ASVL_PAF_YUV = 1025;

        /// ASVL_PAF_YVU -> 0x402
        public const int ASVL_PAF_YVU = 1026;

        /// ASVL_PAF_UVY -> 0x403
        public const int ASVL_PAF_UVY = 1027;

        /// ASVL_PAF_VUY -> 0x404
        public const int ASVL_PAF_VUY = 1028;




        /// ASVL_PAF_YUYV -> 0x501
        public const int ASVL_PAF_YUYV = 1281;

        /// ASVL_PAF_YVYU -> 0x502
        public const int ASVL_PAF_YVYU = 1282;

        /// ASVL_PAF_UYVY -> 0x503
        public const int ASVL_PAF_UYVY = 1283;

        /// ASVL_PAF_VYUY -> 0x504
        public const int ASVL_PAF_VYUY = 1284;

        /// ASVL_PAF_YUYV2 -> 0x505
        public const int ASVL_PAF_YUYV2 = 1285;

        /// ASVL_PAF_YVYU2 -> 0x506
        public const int ASVL_PAF_YVYU2 = 1286;

        /// ASVL_PAF_UYVY2 -> 0x507
        public const int ASVL_PAF_UYVY2 = 1287;

        /// ASVL_PAF_VYUY2 -> 0x508
        public const int ASVL_PAF_VYUY2 = 1288;

        /// ASVL_PAF_YYUV -> 0x509
        public const int ASVL_PAF_YYUV = 1289;



        /// ASVL_PAF_I420 -> 0x601
        public const int ASVL_PAF_I420 = 1537;

        /// ASVL_PAF_I422V -> 0x602
        public const int ASVL_PAF_I422V = 1538;

        /// ASVL_PAF_I422H -> 0x603
        public const int ASVL_PAF_I422H = 1539;

        /// ASVL_PAF_I444 -> 0x604
        public const int ASVL_PAF_I444 = 1540;

        /// ASVL_PAF_YV12 -> 0x605
        public const int ASVL_PAF_YV12 = 1541;

        /// ASVL_PAF_YV16V -> 0x606
        public const int ASVL_PAF_YV16V = 1542;

        /// ASVL_PAF_YV16H -> 0x607
        public const int ASVL_PAF_YV16H = 1543;

        /// ASVL_PAF_YV24 -> 0x608
        public const int ASVL_PAF_YV24 = 1544;

        /// ASVL_PAF_GRAY -> 0x701
        public const int ASVL_PAF_GRAY = 1793;




        /// ASVL_PAF_NV12 -> 0x801
        public const int ASVL_PAF_NV12 = 2049;

        /// ASVL_PAF_NV21 -> 0x802
        public const int ASVL_PAF_NV21 = 2050;

        /// ASVL_PAF_LPI422H -> 0x803
        public const int ASVL_PAF_LPI422H = 2051;

        /// ASVL_PAF_LPI422H2 -> 0x804
        public const int ASVL_PAF_LPI422H2 = 2052;



        /// ASVL_PAF_NV41 -> 0x805
        public const int ASVL_PAF_NV41 = 2053;




        /// ASVL_PAF_NEG_UYVY -> 0x901
        public const int ASVL_PAF_NEG_UYVY = 2305;

        /// ASVL_PAF_NEG_I420 -> 0x902
        public const int ASVL_PAF_NEG_I420 = 2306;





        /// ASVL_PAF_MONO_UYVY -> 0xa01
        public const int ASVL_PAF_MONO_UYVY = 2561;

        /// ASVL_PAF_MONO_I420 -> 0xa02
        public const int ASVL_PAF_MONO_I420 = 2562;




        /// ASVL_PAF_P8_YUYV -> 0xb03
        public const int ASVL_PAF_P8_YUYV = 2819;



        /// ASVL_PAF_SP16UNIT -> 0xc01
        public const int ASVL_PAF_SP16UNIT = 3073;

        /// ASVL_PAF_DEPTH_U16 -> 0xc02
        public const int ASVL_PAF_DEPTH_U16 = 3074;





        /// ASVL_PAF_RAW10_RGGB_10B -> 0xd01
        public const int ASVL_PAF_RAW10_RGGB_10B = 3329;

        /// ASVL_PAF_RAW10_GRBG_10B -> 0xd02
        public const int ASVL_PAF_RAW10_GRBG_10B = 3330;

        /// ASVL_PAF_RAW10_GBRG_10B -> 0xd03
        public const int ASVL_PAF_RAW10_GBRG_10B = 3331;

        /// ASVL_PAF_RAW10_BGGR_10B -> 0xd04
        public const int ASVL_PAF_RAW10_BGGR_10B = 3332;

        /// ASVL_PAF_RAW12_RGGB_12B -> 0xd05
        public const int ASVL_PAF_RAW12_RGGB_12B = 3333;

        /// ASVL_PAF_RAW12_GRBG_12B -> 0xd06
        public const int ASVL_PAF_RAW12_GRBG_12B = 3334;

        /// ASVL_PAF_RAW12_GBRG_12B -> 0xd07
        public const int ASVL_PAF_RAW12_GBRG_12B = 3335;

        /// ASVL_PAF_RAW12_BGGR_12B -> 0xd08
        public const int ASVL_PAF_RAW12_BGGR_12B = 3336;

        /// ASVL_PAF_RAW10_RGGB_16B -> 0xd09
        public const int ASVL_PAF_RAW10_RGGB_16B = 3337;

        /// ASVL_PAF_RAW10_GRBG_16B -> 0xd0A
        public const int ASVL_PAF_RAW10_GRBG_16B = 3338;

        /// ASVL_PAF_RAW10_GBRG_16B -> 0xd0B
        public const int ASVL_PAF_RAW10_GBRG_16B = 3339;

        /// ASVL_PAF_RAW10_BGGR_16B -> 0xd0C
        public const int ASVL_PAF_RAW10_BGGR_16B = 3340;



        /// ASVL_PAF_RAW10_GRAY_10B -> 0xe01
        public const int ASVL_PAF_RAW10_GRAY_10B = 3585;


        /// ASVL_PAF_RAW10_GRAY_16B -> 0xe81
        public const int ASVL_PAF_RAW10_GRAY_16B = 3713;



    }



}

﻿using System;
using System.Runtime.InteropServices;

//引用 https://github.com/steponteam/FaceRecognization/blob/master/Stepon.FaceRecognizationCore/Stepon.FaceRecognizationCore/Extensions/IntPtrExtensions.cs
namespace ArcFaceSharp.Extensions
{
    public static class IntPtrExtensions
    {
        /// <summary>
        ///     将指针转换为结构体数组
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="self">指针</param>
        /// <param name="length">数组长度</param>
        /// <returns>结构体数组</returns>
        public static T[] ToStructArray<T>(this IntPtr self, int length)
        {
            var size = Marshal.SizeOf<T>();
            var array = new T[length];

            for (var i = 0; i < length; i++)
            {
                var iPtr = new IntPtr(self.ToInt64() + i * size);
                array[i] = iPtr.ToStruct<T>();
            }
            return array;
        }

        /// <summary>
        ///     将指针转换为结构体
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="self">指针</param>
        /// <returns>结构体实例</returns>
        public static T ToStruct<T>(this IntPtr self)
        {
            try
            {
                var result = Marshal.PtrToStructure<T>(self);
                return result;
            }
            finally
            {
                Marshal.DestroyStructure<T>(self);
            }
        }

        /// <summary>
        /// 将结构体数组转换为指针，指针需要释放
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>结构体的非托管指针</returns>
        public static IntPtr StructArrayToPtr<T>(T[] array)
        {
            var size = Marshal.SizeOf<T>();
            var swap = Marshal.AllocHGlobal(size * array.Length);
            var result = new IntPtr(swap.ToInt64());
            foreach (T single in array)
            {
                Marshal.StructureToPtr(single, swap, false);
                swap += size;
            }
            return result;
        }
    }
}
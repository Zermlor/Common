using System;
using System.Collections.Generic;
namespace zermlor
{
    /// <summary>
    /// 数组助手类
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// 对象数组的升序排列，如:Enemy[]
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型,如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性,如：int float</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="handler">排序的依据  e.HP e.ATK </param>
        public static void OrderBy<T, TKey>(T[] array, Func<T, TKey> handler) where TKey : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (handler(array[i]).CompareTo(handler(array[j])) > 0)
                    {
                        T temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 对象数组的降序排列，如:Enemy[]
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型,如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性,如：int float</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">排序的依据  e.HP e.ATK </param>
        public static void OrderByDescending<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    if (condition(array[i]).CompareTo(condition(array[j])) < 0)
                    {
                        T temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
        }
        /// <summary>
        /// 获取对象数组中最大元素
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型,如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性,如：int float</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">排序的依据  e.HP e.ATK </param>
        /// <returns></returns>
        public static T GetMax<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                //if (max < array[i])
                //if (max.CompareTo(array[i]) < 0)
                //if (Fun1(max).CompareTo(Fun1(array[i])))
                if (condition(max).CompareTo(condition(array[i])) < 0)
                    max = array[i];
            }
            return max;
        }
        /// <summary>
        /// 获取对象数组中最小元素
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型,如：Enemy</typeparam>
        /// <typeparam name="TKey">对象的属性,如：int float</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">排序的依据  e.HP e.ATK </param>
        /// <returns></returns>
        public static T GetMin<T, TKey>(T[] array, Func<T, TKey> condition) where TKey : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(condition(array[i])) > 0)
                    max = array[i];
            }
            return max;
        }
        //private static int Fun1(Enemy e)
        //{
        //    return e.HP;
        //}
        //private static int Fun2(Enemy e)
        //{
        //    return e.ATK;
        //}
        /// <summary>
        /// 查找所有满足条件的对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T[] FindAll<T>(T[] array, Func<T, bool> condition)
        {
            List<T> result = new List<T>(array.Length);
            foreach (var item in array)
            {
                if (condition(item))
                    result.Add(item);
            }
            return result.ToArray();
        }
        //private bool Fun1(Enemy e)
        //{
        //    return e.HP > 10;
        //}
        /// <summary>
        /// 筛选对象数组
        /// </summary>
        /// <typeparam name="T">对象数组的元素类型</typeparam>
        /// <typeparam name="TKey">筛选目标的类型</typeparam>
        /// <param name="array">对象数组</param>
        /// <param name="handler">筛选策略</param>
        /// <returns></returns>
        public static TKey[] Select<T, TKey>(T[] array, Func<T, TKey> handler)
        {
            TKey[] result = new TKey[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                //result[i] = array[i].transform;
                result[i] = handler(array[i]);
            }
            return result;
        }
        //private Transform Fun1(GameObject go)
        //{
        //    return go.transform;
        //}
        /*
         委托当做参数传递，传递的是逻辑。
         ArrayHelper
         练习1：任意类型数组的降序排列方法
         练习2：获取指定条件的最大元素
                    Enemy[] array;    e.ATK  / e.AtkDistance
         练习3：获取指定条件的最小元素
         练习4：查找满足条件的所有对象
                    Enemy[]    e.HP > 0
         练习5: 筛选对象
                    GameObject[]   ==>   Transform[]
         */
    }
}
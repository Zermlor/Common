using UnityEngine;
using System.Collections.Generic;
namespace zermlor
{
    /// <summary>
    /// 变换组件助手类
    /// </summary>
    public static class TransformHelper
    {
        //递归：将问题转移给范围缩小的子问题。
        //通俗的讲：方法内部调用自身的过程。
        //注意：范围缩小、防止死循环。
        //优点：将非常复杂的问题，简单化。
        //缺点：每次调用自身，都需要在栈中分配空间，如果次数过多可能造成内存泄漏。
        //备注：如果出现异常(StackOverflowException),表示递归过程出现死循环，或者递归次数过多。
        /// <summary>
        /// 未知层级关系查找后代元素
        /// </summary>
        /// <param name="parentTF">父物体变换组件</param>
        /// <param name="childName">子物体名称</param>
        /// <returns></returns> 
        public static Transform FindChild(Transform parentTF, string childName)
        {
            //在子物体中查找
            Transform childTF = parentTF.Find(childName);
            if (childTF != null) return childTF;
            //如果没有找到，将任务交给子物体
            for (int i = 0; i < parentTF.childCount; i++)
            {
                childTF = FindChild(parentTF.GetChild(i), childName);
                if (childTF != null) return childTF;
            }
            return null;
        }
        /// <summary>
        /// 缓动注视方向旋转
        /// </summary>
        /// <param name="currentTF">当前物体的变换组件</param>
        /// <param name="targetDir">目标方向</param>
        /// <param name="rotateSpeed">旋转速度</param>
        public static void LookDirection(Transform currentTF, Vector3 targetDir, float rotateSpeed)
        {
            Quaternion dir = Quaternion.LookRotation(targetDir);
            currentTF.rotation = Quaternion.Lerp(currentTF.rotation, dir, Time.deltaTime * rotateSpeed);
        }
        /// <summary>
        /// 缓动注视位置旋转
        /// </summary>
        /// <param name="currentTF">当前物体的变换组件</param>
        /// <param name="targetDir">目标点世界坐标</param>
        /// <param name="rotateSpeed">旋转速度</param>
        public static void LookPosition(Transform currentTF, Vector3 targetPos, float rotateSpeed)
        {
            Vector3 dir = targetPos - currentTF.position;
            LookDirection(currentTF, dir, rotateSpeed);
        }
        /// <summary>
        /// 获取周围物体
        /// </summary>
        /// <param name="currentTF">当前物体</param>
        /// <param name="tags">目标物体的标签</param>
        /// <param name="distance">搜索半径</param>
        /// <param name="angle">搜索角度</param>
        /// <returns></returns>
        public static Transform[] CalculateAroundObjects(Transform currentTF, string[] tags, float distance, float angle)
        {
            List<Transform> result = new List<Transform>();
            //1.根据标签获取所有物体
            foreach (var tag in tags)
            {
                GameObject[] allGO = GameObject.FindGameObjectsWithTag(tag);
                //如果场景中不存在当前标签物体，则跳过
                if (allGO.Length == 0) continue;
                Transform[] allTF = ArrayHelper.Select(allGO, o => o.transform);
                result.AddRange(allTF);
            }
            //2.筛选，条件：距离、角度
            result = result.FindAll(tf =>
                Vector3.Distance(currentTF.position, tf.position) < distance &&
                Vector3.Angle(currentTF.forward, tf.position - currentTF.position) <= angle / 2
            );
            return result.ToArray();
        }
    }
}
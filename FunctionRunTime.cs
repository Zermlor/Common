using UnityEngine;
using System.Diagnostics;
using UnityEngine.Profiling;
namespace zermlor {
    /// <summary>
    /// 获取某个方法执行时间
    /// </summary>
    public static class FunctionRunTime 
    {
        public delegate void Helper();
        /// <summary>
        /// 检测
        /// </summary>
        /// <param name="helper">方法</param>
        public static void Detection(Helper helper)
        {
            float t = Time.time;
            helper.Invoke();
            UnityEngine.Debug.Log(string.Format("total: {0} ms", Time.time - t));
            Stopwatch sw = new Stopwatch();
            sw.Start();
            helper.Invoke();
            sw.Stop();
            UnityEngine.Debug.Log(string.Format("total: {0} ms", sw.ElapsedMilliseconds));
            Profiler.BeginSample("helper");
            helper.Invoke();
            Profiler.EndSample();
        }
    }
}
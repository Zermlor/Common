using UnityEngine;
using System.IO;
using System.Collections.Generic;
namespace zermlor
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResourceManager
    {
        /*
             定义ResourceManager类
             --读取配置文件
             --加载配置文件  字典集合key:文件名    value:目录/文件名
             --提供加载资源方法(string resName)
     */
        private static Dictionary<string, string> map;
        static ResourceManager()
        {
            map = new Dictionary<string, string>();
            LoadConfig();
        }
        private static string ReadConfig()
        {
            //Assets/StreamingAssets/ResConfig.txt" 
            string path = Application.streamingAssetsPath + "/ResConfig.txt";
            if (Application.platform != RuntimePlatform.Android)
            {//非安卓平台，路径需要添加 file://
                path = "file://" + path;
            }
            WWW www = new WWW(path);
            while (true)
            {
                //如果读取完成
                if (www.isDone)
                    return www.text;
            }
        }
        private static void LoadConfig()
        {
            string strConfig = ReadConfig();
            //字符串读取器
            using (StringReader reader = new StringReader(strConfig))
            {
                string line;
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    //解析
                    string[] keyValue = line.Split('=');
                    map.Add(keyValue[0], keyValue[1]);
                }
            }
        }
        /// <summary>
        /// 加载配置文件中资源
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="resName">资源名称</param>
        /// <returns></returns>
        public static T Load<T>(string resName) where T : Object
        {
            return Resources.Load<T>(map[resName]);
        }
    }
}

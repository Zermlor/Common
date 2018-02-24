using UnityEngine;
namespace zermlor
{
    /// <summary>
    /// 脚本单例类
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;
        private static object lockObj=new object();
        private const string SINGLETON_PREFIX = "Singleton of ";
        public static T Instance
        {
            //按需加载
            get{
                if (instance == null){
                    lock (lockObj){//return this as T;
                        if (instance == null){//在场景中查找对象
                            instance = FindObjectOfType<T>();
                            if (instance == null){ //说明脚本没有附加到游戏对象
                                instance = new GameObject(SINGLETON_PREFIX + typeof(T).Name).AddComponent<T>();
                                instance.Init();
                            }
                        }
                    }
                }
                return instance;
            }
        }
        public virtual void Init()
        {
            DontDestroyOnLoad(instance);
        }
    }
}

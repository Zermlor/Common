using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace zermlor
{
    /// <summary>
    /// 游戏对象池
    /// </summary>
    public class GameObjectPool : MonoSingleton<GameObjectPool>
    {
        private Dictionary<string, List<GameObject>> cache;
        private void Awake()
        {
            cache = new Dictionary<string, List<GameObject>>();
        }
        /// <summary>
        /// 通过对象池创建对象
        /// </summary>
        /// <param name="key">对象的种类</param>
        /// <param name="go">对象预制件</param>
        /// <param name="pos">位置</param>
        /// <param name="dir">方向</param>
        /// <returns></returns>
        public GameObject CreateObject(string key, GameObject go, Vector3 pos, Quaternion dir)
        {
            //现在池中查找是否存在可用对象 
            GameObject targetObject = FindUsableObject(key);
            //如果不存在，则创建新对象，加入池中  
            if (targetObject == null)
            {
                targetObject = Instantiate(go);
                AddObject(key, targetObject);
            }
            //使用对象
            UseObject(pos, dir, targetObject);
            return targetObject;
        }
        private static void UseObject(Vector3 pos, Quaternion dir, GameObject targetObject)
        {
            targetObject.transform.position = pos;
            targetObject.transform.rotation = dir;
            targetObject.SetActive(true);
            //重置状态
            //行为的抽象：接口方法
            //获取游戏对象中所有需要重置的对象
            foreach (var item in targetObject.GetComponents<IResetable>())
            {
                item.OnReset();
            }
        }
        private void AddObject(string key, GameObject targetObject)
        {
            //如果不存在当前键，则添加记录
            if (!cache.ContainsKey(key)) cache.Add(key, new List<GameObject>());
            cache[key].Add(targetObject);
        }
        private GameObject FindUsableObject(string key)
        {
            return cache.ContainsKey(key) ? cache[key].Find(o => !o.activeInHierarchy) : null;
        }
        /// <summary>
        /// 即时回收
        /// </summary>
        /// <param name="go"></param>
        public void CollectObject(GameObject go)
        {
            go.SetActive(false);
        }
        /// <summary>
        /// 延迟回收
        /// </summary>
        /// <param name="go"></param>
        /// <param name="delay"></param>
        public void CollectObject(GameObject go, float delay)
        {
            //DelayCollet(go, delay);
            StartCoroutine(DelayCollet(go, delay));
        }
        private IEnumerator DelayCollet(GameObject go, float delay)
        {
            yield return new WaitForSeconds(delay);//暂时退出
            if (go) CollectObject(go);
        }
        public GameObject target;
        /// <summary>
        /// 清空一类游戏对象
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            //销毁列表中的游戏对象
            foreach (var item in cache[key])
            {
                Destroy(item);//没有立即销毁
            }
            //移除记录
            cache.Remove(key);
        }
        /// <summary>
        /// 清空所有
        /// </summary>
        public void ClearAll()
        {
            //遍历字典   删除字典
            //foreach (string item in cache.Keys)
            //{
            //    Clear(item);
            //}
            //只能foreach获取元素
            //string str = cache.Keys[0];//string
            //将字典所有键存入列表集合
            List<string> keys = new List<string>(cache.Keys);
            //遍历列表
            foreach (string item in keys)
            {
                //删除字典记录 
                Clear(item);
            }
        }
    }
}
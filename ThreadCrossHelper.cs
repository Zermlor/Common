using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// 线程交叉访问助手类
/// </summary>
public class ThreadCrossHelper : MonoSingleton<ThreadCrossHelper>
{
    public struct DelayedItem//延迟数据结构
    {
        public Action DelayedAction { get; set; }

        public DateTime DelayedTime { get; set; }
    }

    private Action currentAction;//无参委托

    private List<DelayedItem> delayedActionList;//延迟激活列表

    private object currentActionLocker;//锁

    protected override void Init()
    {
        base.Init();
        delayedActionList = new List<DelayedItem>();
        currentActionLocker = new object();
    }

    /// <summary>
    /// 在主线程中执行
    /// </summary>
    /// <param name="action">行为</param>
    public void ExecuteOnMainThread(Action action)
    {
        lock (currentActionLocker)
        {
            if (currentAction == null)
                currentAction = action;
            else
                currentAction += action;//向委托链条添加委托实例
        }


    }

    public void ExecuteOnMainThread(Action action, float time)
    {
        lock (delayedActionList)
        {
            delayedActionList.Add(new DelayedItem() { DelayedAction = action, DelayedTime = DateTime.Now.AddSeconds(time) });
        }
    }

    private void Update()
    {
        CheckCurrentAction();

        CheckDelayedAction();
    }

    private void CheckCurrentAction()
    {
        lock (currentActionLocker)
        {
            if (currentAction != null)
            {
                currentAction();
                currentAction = null;
            }
        }
    }

    private void CheckDelayedAction()
    {
        lock (delayedActionList)
        {
            //判断每项是否达到执行时间
            for (int i = delayedActionList.Count - 1; i >= 0; i--)
            {
                //如果没有到达 判断下一项
                if (delayedActionList[i].DelayedTime > DateTime.Now) continue;
                //执行当前项 并从列表中移除
                delayedActionList[i].DelayedAction();//执行
                delayedActionList.RemoveAt(i);//从列表中移除
            }
        }
    }
}

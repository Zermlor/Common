using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;
namespace zermlor
{
    /// <summary>
    /// 动画事件行为类：附加到模型上
    /// </summary>
    public class AnimationEventBehaviour : MonoBehaviour
    {
        //public class AttackHandler : UnityEvent { }
        public event Action AttackHandler;
        private Animator anim;
        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
        }
        //由Unity动画事件调用 
        public void OnCancelAnim(string animPara)
        {
            anim.SetBool(animPara, false);
        }
        //由Unity动画事件调用
        public void OnAttack()
        {
            if (AttackHandler != null)
            {
                AttackHandler();
            }
        }
    }
}

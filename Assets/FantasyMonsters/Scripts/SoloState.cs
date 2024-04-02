using System;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts
{
    public class SoloState : StateMachineBehaviour
    {
        public bool Active;
        public Action Continue;

        private bool _enter;
        private float _nextAttackTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool("Action", true);
            Active = true;
           _nextAttackTime = Time.time + UnityEngine.Random.Range(1f, 5f);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Time.time >= _nextAttackTime)
            {
                animator.SetBool("Action", true); // Zapnutí animace "Attack"
                _nextAttackTime = Time.time + UnityEngine.Random.Range(1f, 5f);
            }

            if (stateInfo.normalizedTime >= 1)
            {
                OnStateExit(animator, stateInfo, layerIndex);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Active) return;

            Active = false;

            if (Continue == null)
            {
                animator.SetBool("Action", false);
            }
            else
            {
                Continue();
                Continue = null;
            }
        }
    }
}
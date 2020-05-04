using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UniRx;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "AnimatorBehaviour", menuName = "Behaviour/AnimatorBehaviour")]

    public class AnimatorBehaviour : BehaviourBase, IAwake
    {

        private DataUnit dataUnit { get; set; }
        public void OnAwake()
        {
            if (actor is Unit)
            {
                dataUnit = actor.GetData<DataUnit>();
                dataUnit.m_animator = actor.GetComponent<Animator>();
                dataUnit.OnStateChange.Subscribe(OnAnimationStateChange).AddTo(actor);
            }
            else
            {
                Debug.LogError($"{GetType()} only works with actor Unit");
               
            }
        }

        public void OnAnimationStateChange(DataUnit dataUnit)
        {
            //Debug.Log($"{GetType()}     {dataUnit.stateUnit.Value.ToString()}");

            switch (dataUnit.stateUnit.Value)
            {
                case StateUnit.Idle:
                    dataUnit.m_animator.SetInteger("AnimState", 0);
                    break;
                case StateUnit.Run:
                    dataUnit.m_animator.SetInteger("AnimState", 2);
                    break;
                case StateUnit.Attack:
                    dataUnit.m_animator.SetTrigger("Attack");
                    break;
                case StateUnit.AttackIdle:
                    dataUnit.m_animator.SetInteger("AnimState", 1);
                    break;
                case StateUnit.Die:
                    dataUnit.m_animator.SetTrigger("Death");
                    break;
                case StateUnit.Recover:
                    dataUnit.m_animator.SetTrigger("Recover");
                    break;
                case StateUnit.Hurt:
                    //Debug.Log("Hurt not finde");
                    break;
                default:
                    break;
            }
        }

    }
}
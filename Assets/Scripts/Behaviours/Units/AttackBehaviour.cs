using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UniRx;
namespace TowerFight
{
    public class AttackBehaviour : BehaviourBase, IAwake
    {
        
        public void OnAwake()
        {
            DataUnit dataUnit;
            if (actor is Unit)
                dataUnit = actor.GetData<DataUnit>();
            else
            {
                Debug.LogError($"{GetType()} only works with actor Unit");
                return;
            }
            dataUnit.attackTimer = 0f;
            dataUnit?.OnAttackStart.Subscribe(StartAttack).AddTo(actor);
            dataUnit?.OnAttackEvent.Subscribe(TakeDamage).AddTo(actor);

            OnStart(dataUnit);
        }

        protected virtual void OnStart(DataUnit dataUnit)
        {

        }

        protected virtual void StartAttack(Unit unit)
        {
            var data = unit?.GetData<DataUnit>();

            if (Time.time - data.attackTimer > (1f / data.stats.attackSpeed))
            {
                data.attackTimer = Time.time;
                data.stateUnit.Value = StateUnit.Attack;
            }
            else
            {
                data.stateUnit.Value = StateUnit.AttackIdle;
            }
        }

        protected virtual void TakeDamage(Unit obj) { }

    }
}
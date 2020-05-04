using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UniRx;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "TargetAttckBehaviour", menuName = "Behaviour/TargetAttckBehaviour")]
    public class TargetAttckBehaviour : AttackBehaviour, IAwake
    {
          
        protected override void TakeDamage(Unit unit)
        {
            var data = unit.GetData<DataUnit>();
            if (data.target)
            {
                data.target.SetDamage(data.stats.Damage);
                data.target = null;
            }
        }
      
    }
}
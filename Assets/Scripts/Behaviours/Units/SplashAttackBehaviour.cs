using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;
using System;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "SplashAttackBehaviour", menuName = "Behaviour/SplashAttackBehaviour")]

    public class SplashAttackBehaviour : AttackBehaviour
    {
        protected override void TakeDamage(Unit unit)
        {           
            var data = unit.GetData<DataUnit>();
            var direction = (data.stats.side == Side.right) ? Vector2.left : Vector2.right;
            LayerMask layerMask = data.stats.side == Side.right ? ManagerUnits.instans.leftUnitMask : ManagerUnits.instans.rightUnitMask;
            RaycastHit2D[] hit = Physics2D.RaycastAll(data.raycast.position, direction, data.stats.attackDistance*2, layerMask);
            foreach (var item in hit)
            {
                if(item.collider)
                    if (item.collider.tag == "Unit")
                    {
                        item.collider.GetComponent<Unit>().SetDamage(data.stats.Damage);
                    }
            }
            data.target = null;
        }
    }
}
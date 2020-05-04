using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "TowerHealthBarBehavhiour", menuName = "Behaviour/TowerHealthBarBehavhiour")]

    public class TowerHealthBarBehavhiour : BehaviourBase, IAwake
    {
        public void OnAwake()
        {
            if (actor is Unit)
            {
                var data = actor.GetData<DataUnit>();
                if (data)
                {
                    var managerUi = Toolbox.Get<ManagerUI>();
                    var healthBar = data.stats.side == Side.left ? managerUi.heathLeftTowerBar : managerUi.heathRightTowerBar;
                    data.OnSetDamage.Subscribe(
                    delegate (Unit unit)
                    {
                        healthBar.health = HealthBar.OnHitChange(data.stats.MaxHitPoint, data.stats.hitPoint);
                    }).AddTo(actor);
                } 
            }
        }
    }
}
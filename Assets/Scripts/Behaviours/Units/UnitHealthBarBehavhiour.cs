using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "UnitHealthBarBehavhiour", menuName = "Behaviour/UnitHealthBarBehavhiour")]

    public class UnitHealthBarBehavhiour : BehaviourBase, IAwake
    {
        public void OnAwake()
        {

            if (actor is Unit)
            {
                var data = actor.GetData<DataHealthBar>();
                var spawnPoint = (actor as Unit).healthBarSpawnPoint;

                if (data && spawnPoint)
                {
                    var healthBar = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);
                    healthBar.transform.SetParent(actor.transform);
                    healthBar.gameObject.SetActive(false);
                    var dataUnit = actor.GetData<DataUnit>();
                    dataUnit.OnSetDamage.Subscribe(
                        delegate (Unit unit) 
                        {
                            if (!healthBar.gameObject.activeSelf)
                                healthBar.gameObject.SetActive(true);
                            healthBar.health = HealthBar.OnHitChange(dataUnit.stats.MaxHitPoint, dataUnit.stats.hitPoint);
                        }).AddTo(actor);
                    healthBar.OnAwake();
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "RangeAttackBehaviour", menuName = "Behaviour/RangeAttackBehaviour")]

    public class RangeAttackBehaviour : AttackBehaviour, IAwake
    {
        protected override void TakeDamage(Unit unit)
        {
            if(debug)
                Debug.Log($"TakeDamage {GetType()}");

            var data = unit.GetData<DataUnit>();
            var dataBullet = unit.GetData<DataProjectile>();

            if (data.target)
            {
                var position = (unit as RangeUnit).spawnPoint;
                var bullet = Instantiate(dataBullet.prefab.gameObject, position, Quaternion.identity).GetComponent<Projectile>();
                bullet.AddTo(data);
                bullet.AddTo(dataBullet);
                bullet.target = data.target;
                bullet.sideTurn = data.stats.side == Side.right ? Side.left : Side.right;
                bullet.OnAwake();
            }
        }
    }
}
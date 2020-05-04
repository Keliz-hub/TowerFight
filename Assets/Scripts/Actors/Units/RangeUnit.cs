using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerFight
{
    public class RangeUnit : Unit
    {
        [SerializeField]
        private Transform pointProjectileSpawn;

        public Vector2 spawnPoint
        {
            get
            {
                Vector2 vector;
                var delta = pointProjectileSpawn.position.x - transform.position.x;

                if (GetData<DataUnit>().stats.side == Side.right)
                {                   
                    vector = new Vector2(transform.position.x - delta, pointProjectileSpawn.position.y);
                }
                else
                {
                    vector = new Vector2(transform.position.x + delta, pointProjectileSpawn.position.y);

                }
                return vector;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataProjectile", menuName = "Data/DataProjectile")]

    public class DataProjectile : DataBase
    {
        public GameObject prefab;
        public float Speed;
        public Subject<Unit> OnHit { get; private set; } = new Subject<Unit>();
      
    }
}
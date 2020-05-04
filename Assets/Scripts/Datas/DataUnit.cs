using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataUnit", menuName = "Data/DataUnit")]
    [SerializeField]
    [System.Serializable]

    public class DataUnit : DataBase
    {
        #region Public
        public Sprite icon;
        public GameObject prefab;

        public bool CanAttack = true;

        public Stats stats;
        public Unit target { get; set; }
        public Collider2D collider2D { get; set; }
        public Animator m_animator { get; set; }        
        public LayerMask layer { get; set; }
        public float attackTimer { get; set; }
        public Transform raycast { get; set; }

        public bool justStand { get; set; } = false;
        #endregion


        #region Subjects
        public Subject<Unit> OnAttackStart { get; private set; } = new Subject<Unit>();
        public Subject<Unit> OnAttackEvent { get; private set; } = new Subject<Unit>();

        public Subject<Unit> OnSetDamage { get; private set; } = new Subject<Unit>();

        public Subject<RangeUnit> OnSpellCast { get; private set; } = new Subject<RangeUnit>();

        public ReactiveProperty<StateUnit> stateUnit { get; private set; } = new ReactiveProperty<StateUnit>(StateUnit.Idle);
        public Subject<DataUnit> OnStateChange { get; private set; } = new Subject<DataUnit>();
        public Subject<Unit> OnDie { get; private set; } = new Subject<Unit>();
        #endregion

        public void SetData(DataUnit dataUnit, Stats stats)
        {
            icon = dataUnit.icon;
            prefab = dataUnit.prefab;
            
            CanAttack = dataUnit.CanAttack;

            this.stats = stats;

        }


        public static DataUnit Copy(DataUnit dataUnit) 
        {
            var newdata = CreateInstance(typeof(DataUnit)) as DataUnit;
            newdata.icon = dataUnit.icon;
            newdata.prefab = dataUnit.prefab;
            newdata.CanAttack = dataUnit.CanAttack;
            newdata.stats = dataUnit.stats;
            newdata.layer = dataUnit.layer;
            return newdata;
        }

    }
    [System.Serializable]
    public struct Stats
    {
        public Stats(Stats stats, Side side)
        {
            this = stats;
            this.side = side;
        }

        public Side side;

        public AttackType attackType;

        public float Damage { get => damage + (float)(level - 1) * 0.1f * damage; }

        public string name;

        public float speed;

        public int manaCost;

        public float spawnCountdown;

        public float MaxHitPoint { get => maxHitPoint + maxHitPoint * 0.1f * (float)(level - 1); }

        [SerializeField]
        private float maxHitPoint;

        public float hitPoint;
        [SerializeField]
        private float damage;

        [SerializeField]
        private int upgradeCost;

        public int UpgradeCost => upgradeCost + upgradeCost * (level-1);

        public float attackSpeed;
        
        public float attackDistance;

        public int level;
    }
 
 

    public enum Side
    {
        left,
        right
    }

    public enum AttackType 
    {
        melee,
        range    
    }

}
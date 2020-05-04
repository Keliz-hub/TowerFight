using Homebrew;
using System.Collections;
using UnityEngine;
using UniRx;

namespace TowerFight
{
    public class Unit : ActorBase, ITick
    {
        [SerializeField]
        private Transform raycast;


        public Transform healthBarSpawnPoint;

        private SpriteRenderer render;
        protected DataUnit dataUnit;
        private GameManager gameManager;
        
        public Side sideTurn
        {
            get
            {
                return dataUnit.stats.side == Side.right ? Side.left : Side.right;
            }
            set
            {
                if (value == Side.right)
                {
                    render.flipX = render.flipX == true ? false : true;
                }
            }

        }

        public bool isAlive
        {
            get
            {
                return dataUnit.stats.hitPoint > 0.0f;
            }
        }
        protected override void OnStart()
        {

            dataUnit = GetData<DataUnit>();
            render = GetComponent<SpriteRenderer>();
            gameManager = Toolbox.Get<GameManager>();

            if (raycast)
                dataUnit.raycast = raycast; 


            dataUnit.collider2D = GetComponent<Collider2D>();

            dataUnit.stateUnit.Value = StateUnit.Idle;
            sideTurn = dataUnit.stats.side;

            dataUnit.stateUnit.Subscribe(delegate (StateUnit x) { dataUnit.OnStateChange.OnNext(dataUnit); }).AddTo(this);
            dataUnit.stats.hitPoint = dataUnit.stats.MaxHitPoint;


        }

        public void Tick()
        {
            if(dataUnit.justStand)
                return;
            if (!gameManager.battle)
            {
                dataUnit.stateUnit.Value = StateUnit.Idle;
                return;
            }


            if (dataUnit.stats.hitPoint > 0)
                if (!Searching(raycast.position,
                               (dataUnit.stats.side == Side.right) ? Vector2.left : Vector2.right))
                {
                    Going();
                }
                else 
                {

                }
            else
            {
            
            }


        }

        private void Going()
        {
           
            transform.position = new Vector3(transform.position.x +
                ((dataUnit.stats.side == Side.right) ? -dataUnit.stats.speed : dataUnit.stats.speed) * Time.deltaTime, transform.position.y, transform.position.z);
            dataUnit.stateUnit.Value = StateUnit.Run;
        }



        public void AnimatorEvent(StateUnit stateUnit)
        {          
            if(stateUnit == StateUnit.Attack)
                dataUnit.OnAttackEvent.OnNext(this);
        }

        public bool SetDamage(float damage)
        {
            if (isAlive)
            {
                if (dataUnit.stats.hitPoint - damage <= 0f)
                {
                    dataUnit.stats.hitPoint = 0.0f;
                    Die();
                }
                else
                {
                    dataUnit.stateUnit.Value = StateUnit.Hurt;
                    dataUnit.stats.hitPoint -= damage;

                }
                dataUnit.OnSetDamage.OnNext(this);
            }
            return dataUnit.stats.hitPoint - damage <= 0f;

        }



        private bool Searching(Vector2 origin, Vector2 direction)
        {
            LayerMask layerMask = dataUnit.stats.side == Side.right ? ManagerUnits.instans.leftUnitMask : ManagerUnits.instans.rightUnitMask;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, dataUnit.stats.attackDistance, layerMask);
            
            if (debug)
                Debug.DrawRay(origin, direction * dataUnit.stats.attackDistance, Color.green);
             
            if (!hit)
                return false;           

            if (hit.collider.tag == "Unit")
            {
                var unit = hit.collider.GetComponent<Unit>();
                if (unit.GetData<DataUnit>().stats.side != dataUnit.stats.side)
                {
                    dataUnit.target = unit;
                    if(dataUnit.CanAttack)
                        dataUnit.OnAttackStart.OnNext(this);
                    return true;
                }
            }
            return false;
        }



        public void Die()
        {
            if (dataUnit.stateUnit.Value != StateUnit.Die)
            {
                dataUnit.justStand = true;
                dataUnit.collider2D.enabled = false;
                dataUnit.stateUnit.Value = StateUnit.Die;
                dataUnit.OnDie.OnNext(this);
            }
        }
       

        public void OnDestroy()
        {
            if (Toolbox.Instance)
            {                
                ManagerUpdate.RemoveFrom(this);
                ManagerUnits.Remove(this);
            }
        }

    }
    public enum StateUnit
    {
        Idle,
        Run,
        Attack,
        AttackIdle,
        Die,
        Recover,
        Hurt
    }
}
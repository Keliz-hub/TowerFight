using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerFight
{
    public class Projectile : ActorBase , ITick
    {
        [SerializeField]
        private Side turn = Side.right;
        private DataUnit dataUnit;
        private DataProjectile dataProjectile;
        public Unit target { get; set; }

        public float distansToTrigger = 0.4f;
       

        public Side sideTurn
        {
            get
            {
                return turn;

            }
            set
            {
                if (value != turn)
                {
                    var render = GetComponent<SpriteRenderer>();
                    render.flipX = !render.flipX;
                    turn = value;
                }

            }

        }

        protected override void OnStart()
        {
            dataUnit = GetData<DataUnit>();
            dataProjectile = GetData<DataProjectile>();

        }

        private void MoveToTarget()
        {
            if(target)
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), dataProjectile.Speed * Time.deltaTime);
         
        }
        
        public bool TargetIsClose(Unit target)
        {
            if(target)
                return Vector2.Distance(new Vector2(target.transform.position.x, 0), new Vector2(transform.position.x, 0)) > distansToTrigger;
            return target;
        }


        public void Tick()
        {
            if (target.isAlive) {
                if (TargetIsClose(target))
                    MoveToTarget();
                else
                {
                    target.SetDamage(dataUnit.stats.Damage);
                    dataProjectile.OnHit.OnNext(target);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
       
    }
    
}
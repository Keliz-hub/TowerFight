using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;

namespace TowerFight
{
    public class Fireball : ActorBase, ITick
    {
        private DataUnit dataUnit;
        private Animator animator;
        private DataProjectile dataProjectile;

        [SerializeField]
        private float DamageRadius = 2f;   

        [SerializeField]
        private string triggerName;

        [SerializeField]
        private float distansToTrigger = 0.4f;

        [SerializeField]
        private Side turn = Side.right;

        private bool isExplosion = false;

        private GameManager gameManager;

        private DataSpell dataSpell;

        protected override void OnStart()
        {
            dataUnit = GetData<DataUnit>();
            dataProjectile = GetData<DataProjectile>();
            animator = GetComponent<Animator>();
            gameManager = Toolbox.Get<GameManager>();
            dataSpell = GetData<DataSpell>();
            sideTurn = dataUnit.stats.side == Side.right ? Side.left : Side.right;            
        }

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


        private void Going()
        {            
            var vector = new Vector3(transform.position.x + ((dataUnit.stats.side == Side.right) ? -1 : 1) * dataProjectile.Speed * Time.deltaTime
                , transform.position.y, transform.position.z);
            transform.position = vector;
        }
        private bool Searching(Vector2 origin, Vector2 direction)
        {
            LayerMask layerMask = dataUnit.stats.side == Side.right ? ManagerUnits.instans.leftUnitMask : ManagerUnits.instans.rightUnitMask;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, distansToTrigger, layerMask);

            if (debug)
                Debug.DrawRay(origin, direction * distansToTrigger, Color.green);


            if (!hit)
                return false;

            if (hit.collider.tag == "Unit")
            {
                if (debug)
                    Debug.Log(hit.collider.name);
                Collider2D[] collider2D = Physics2D.OverlapCircleAll(hit.collider.transform.position, DamageRadius, layerMask);

                foreach (var item in collider2D)
                {
                    item.GetComponent<Unit>().SetDamage(dataUnit.stats.Damage * dataSpell.power);
                }
                animator.SetTrigger(triggerName);
                return isExplosion = true;
            }
            return false;
        }

        private void OnExpotionEnd()
        {
            Destroy(this.gameObject);
        }

        public void Tick()
        {
            if(!gameManager.battle)
                Destroy(this.gameObject);


            if (!isExplosion)
                if (!Searching(transform.position,
                               ((dataUnit.stats.side == Side.right) ? Vector2.left : Vector2.right)))
                {

                    Going();
                }
        }
    }
}
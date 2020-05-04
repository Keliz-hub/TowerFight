using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerFight
{
    public class ToxicRain : ActorBase, ITick
    {
        private GameManager gameManager;
        private ManagerUnits managerUnits;
        private DataUnit tower;
        private DataSpell dataSpell;
        

        private float timer = 0f;

        [SerializeField]
        private Side turn = Side.right;

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
            gameManager = Toolbox.Get<GameManager>();
            dataSpell = GetData<DataSpell>();
            tower = GetData<DataUnit>();
            managerUnits = Toolbox.Get<ManagerUnits>();
            StartCoroutine(RainLive(dataSpell.duratation));
            transform.position = new Vector2(0.0f,3.0f);
            sideTurn = tower.stats.side == Side.right ? Side.left : Side.right;

        }

        public void Tick()
        {
            if (!gameManager.battle)
                Destroy(gameObject);

            if (Time.time - timer >= 0f)
            {
                var enumyUnits = (tower.stats.side == Side.right) ? managerUnits.left : managerUnits.right;
                enumyUnits.ForEach(x => x.SetDamage(tower.stats.Damage * dataSpell.power));

                timer = Time.time + 1f;
            }
        }


        private IEnumerator RainLive(float duratation)
        {
            yield return new WaitForSeconds(duratation);
            Destroy(gameObject);
        }
    }
}
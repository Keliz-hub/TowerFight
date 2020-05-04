using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "SpellProjectileBehaviour", menuName = "Behaviour/SpellProjectileBehaviour")]
    public class SpellProjectileBehaviour : BehaviourBase, IAwake
    {
        bool countDown = false;

        public void OnAwake()
        {
            var managerUI = Toolbox.Get<ManagerUI>();
            if (actor is RangeUnit)
            {

                var data = (actor as RangeUnit).GetData<DataUnit>();
                if (data)
                {
                    actor.StartCoroutine(StartCountdown(actor.GetData<DataSpell>().countdown));

                    data.OnSpellCast.Subscribe(OnButtonDown).AddTo(actor);                   
                }
            }
            else
            {
                if (debug)
                    Debug.Log($"{GetType()} actor is not Unit");
            }
        }

        public void OnButtonDown(RangeUnit unit)
        {

            if (countDown)
                return;
            else
            {
                var dataSpell = unit.GetData<DataSpell>();
                actor.StartCoroutine(StartCountdown(dataSpell.countdown));

                var data = unit.GetData<DataUnit>();
                var projectile = dataSpell.spellActor as DataProjectile;
                if (data)
                {

                    var obj = Instantiate(projectile.prefab, unit.spawnPoint, Quaternion.identity);

                    var ball = obj.GetComponent<ActorBase>();
                    ball.AddTo(projectile);
                    ball.AddTo(data);
                    ball.AddTo(dataSpell);

                    ball.OnAwake();
                }
            }
        }

        private IEnumerator StartCountdown(float spawnCountdown)
        {
            countDown = true;
            yield return new WaitForSeconds(spawnCountdown);
            countDown = false;

        }
    }
}
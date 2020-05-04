using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using Unity;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "DieBehaviour", menuName = "Behaviour/DieBehaviour")]

    public class DieBehaviour : BehaviourBase, IAwake
    {
        public float time = 1f;
        public void OnAwake()
        {
            actor.GetData<DataUnit>().OnDie.Subscribe(delegate (Unit unit) { if (unit) unit.StartCoroutine(CorpsesLive(time, unit.gameObject)); }).AddTo(actor);

        }

        private IEnumerator CorpsesLive(float timer, GameObject corpses)
        {
            yield return new WaitForSeconds(timer);
            if (corpses)
                Destroy(corpses);
        }
    }
}
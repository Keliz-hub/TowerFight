using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "TowerDieBehaviour", menuName = "Behaviour/TowerDieBehaviour")]

    public class TowerDieBehaviour : BehaviourBase, IAwake
    {
        public void OnAwake()
        {
            var data = actor.GetData<DataUnit>();

            data.OnDie.Subscribe(OnDie).AddTo(actor);

        }

        public void OnDie(Unit unit)
        {
            Toolbox.Get<GameManager>().EndGame(unit.GetData<DataUnit>().stats.side);
        }
    }
}
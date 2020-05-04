using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "GetDamageBehaviour", menuName = "Behaviour/GetDamageBehaviour")]

    public class GetDamageBehaviour : BehaviourBase, IAwake
    {
        [SerializeField]
        private Color onHitColor = Color.red;
        [SerializeField]
        private float timeColor = 0.05f;

        private Color color;

        public void OnAwake()
        {
            var dataUnit = actor.GetData<DataUnit>();
            color = actor.GetComponent<SpriteRenderer>().color;
            dataUnit.OnSetDamage.Subscribe(
                delegate (Unit unit) 
                { 
                    OnSetDamage(unit);
                }).AddTo(actor);
        }
        private void OnSetDamage(Unit unit)
        {
            if(unit && actor)
                actor.StartCoroutine(StartColor(unit));
        }
        

        private IEnumerator StartColor(Unit unit)
        {
            if (unit)
            {
                var renderer = unit.GetComponent<SpriteRenderer>();                
                var newColor = onHitColor;
                renderer.color = newColor;
                yield return new WaitForSeconds(timeColor);                   
                renderer.color = color;
            }
        }
    }
}
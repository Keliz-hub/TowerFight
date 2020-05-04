using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "ExploutionAttackBehaviour", menuName = "Behaviour/ExploutionAttackBehaviour")]

    public class ExploutionAttackBehaviour : AttackBehaviour
    {    
        [SerializeField]
        private float Radius = 2f;

        private bool explotion = false;
   
        protected override void TakeDamage(Unit obj)
        {
            if (!explotion)
            {
                explotion = true;
                DataUnit data = obj.GetData<DataUnit>();
                LayerMask layerMask = data.stats.side == Side.right ? ManagerUnits.instans.leftUnitMask : ManagerUnits.instans.rightUnitMask;
                Collider2D[] coliders = Physics2D.OverlapCircleAll(data.raycast.position, Radius, layerMask);
                

                foreach (var item in coliders)
                {                  
                    if(item.tag == "Unit")
                    item.GetComponent<Unit>().SetDamage(data.stats.Damage);
                }
                data.justStand = true;
                actor.tag = "Untagged";
                actor.StartCoroutine(Die());
            }
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(0.6f);
            Destroy(actor.gameObject);
        }
    }
}
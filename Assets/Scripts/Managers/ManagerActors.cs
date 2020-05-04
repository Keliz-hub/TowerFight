using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerFight;

namespace Homebrew
{
    [CreateAssetMenu(fileName = "ManagerActors", menuName = "Managers/ManagerActors")]
    public class ManagerActors : ManagerBase, IAwake
    {        
        private List<ActorBase> actors { get; set; } = new List<ActorBase>();

        public void OnAwake()
        {
            if(debug)
                Debug.Log($"{GetType()}");

            actors.AddRange(FindObjectsOfType<ActorBase>());        

            foreach (var actor in actors)
            {
                if (actor is IAwake)
                {
                    if(debug)
                        Debug.Log(actor.GetType());
                    (actor as IAwake).OnAwake();
                }
            }
           
        }

        public static void AddTo(object awakeble)
        {

            var mngUpdate = Toolbox.Get<ManagerActors>();
            mngUpdate.actors.Add(awakeble as ActorBase);
            if (awakeble is IAwake)
            {
                (awakeble as IAwake).OnAwake();
            }

        }

        public static void RemoveFrom(object awakeble)
        {
            var mngUpdate = Toolbox.Get<ManagerActors>();
            mngUpdate.actors.Remove(awakeble as ActorBase);
        }

        public static T Get<T>()
        {
            object resolve = null;
            var actors = Toolbox.Get<ManagerActors>().actors;
            foreach (var actor in actors)
            {
                if (actor is T)
                {
                    resolve = actor;
                }
            }
            return (T)resolve;
        }
    }
}
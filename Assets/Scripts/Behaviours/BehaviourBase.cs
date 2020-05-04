using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerFight
{
    public class BehaviourBase : ScriptableObject
    {
        protected ActorBase actor;
        public bool debug = false;
        public void AddActor(ActorBase actor)
        {
            this.actor = actor;
        }
    }
}
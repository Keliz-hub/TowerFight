using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerFight
{
    public class StandPoint : ActorBase
    {
        public DataUnit dataUnit { get; set; }
        public UnityEvent<DataUnit,Transform> OnTabsUnit { get; private set; } = new UnityEvent<DataUnit, Transform>();
                     
        private void OnMouseDown()
        {
            OnTabsUnit.Invoke(dataUnit,transform);
        }
        
    }
}
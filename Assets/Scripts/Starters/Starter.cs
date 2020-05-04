using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;

namespace TowerFight
{
    public class Starter : MonoBehaviour
    {
        public List<ManagerBase> managers = new List<ManagerBase>();


        void Awake()
        {
            foreach (var managerBase in managers)
            {                
                Toolbox.Add(managerBase);
            }
        }
    }
}
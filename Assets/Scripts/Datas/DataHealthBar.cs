using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataHealthBar", menuName = "Data/DataHealthBar")]

    public class DataHealthBar : DataBase
    {
        public HealthBar prefab;        
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataStandPoint", menuName = "Data/DataStandPoint")]

    public class DataStandPoint : DataBase
    {
        public List<Transform> standPoints;
    }
}
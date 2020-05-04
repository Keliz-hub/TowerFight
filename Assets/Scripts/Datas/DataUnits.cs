using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataUnits", menuName = "Data/DataUnits")]
    public class DataUnits : DataBase
    {
        public List<DataUnit> units;
        public List<DataUnit> towers;

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TowerFight
{
    public class DataAi : DataBase
    {
        public Difficult difficult;
    }

    public enum Difficult
    {
        easy ,
        normal ,
        hard 
    }
}
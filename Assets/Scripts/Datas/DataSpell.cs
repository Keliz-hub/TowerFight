using System.Collections;
using System.Collections.Generic;
using TowerFight;
using UnityEngine;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataSpell", menuName = "Data/DataSpell")]

    public class DataSpell : DataBase
    {
        public SpellType spellType;

        public Sprite icon;
        [Space(10)]
        public float power;
        [Space(10)]
        public float countdown;
        [Space(10)]
        public float duratation;

        public DataBase spellActor;
               
    }
    public enum SpellType
    {
        active,
        passive
    }

}
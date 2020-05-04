using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataPlayer", menuName = "Data/DataPlayer")]

    [System.Serializable]
    public class DataPlayer : DataBase
    {


        public bool Player = false;

        public ReactiveProperty<int> Gold { get; private set; } = new ReactiveProperty<int>(0);
        [Space(20)]
        public bool manaFlows = true;
        public ReactiveProperty<float> mana { get; private set; } = new ReactiveProperty<float>(0f);
        public float startMaxMana;
        public float manaMax { get; set; }

        public float upgradeMana;
        public float upgradeManaCost;

        public RangeUnit Tower { get; set; }

        public Side side;

        public DataUnit dataTower;
        public List<DataUnit> playerTowers;
        public List<DataUnit> playerUnits;
        public List<DataUnit> squad;
        public static DataPlayer Copy(DataPlayer data)
        {
            var newdata = (CreateInstance(typeof(DataPlayer)) as DataPlayer);
            newdata.Player = data.Player;
            newdata.manaFlows = data.manaFlows;
            newdata.startMaxMana = data.startMaxMana;
            newdata.manaMax = data.manaMax;
            newdata.upgradeMana = data.upgradeMana;
            newdata.upgradeManaCost = data.upgradeManaCost;
            newdata.dataTower = data.dataTower;

            newdata.playerTowers = data.playerTowers;
            newdata.squad = data.squad;
            newdata.playerUnits = data.playerUnits;
            newdata.side = data.side;
            return newdata;
        }
    }
}
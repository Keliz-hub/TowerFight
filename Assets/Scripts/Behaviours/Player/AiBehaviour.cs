using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace TowerFight
{
    [CreateAssetMenu(fileName = "AlBehaviour", menuName = "Behaviour/AlBehaviour")]

    public class AiBehaviour : BehaviourBase, IAwake, ITick
    {
        private DataPlayer dataPlayer;
        [SerializeField]
        private float timeToAction = 5f;
        private float timer;
        private GameManager manager;
        private ManagerUnits managerUnits;
        private Difficult difficult;
        private List<Unit> enemyUnits;

        private int needToSpawn = 0;
        private int wontToSpawn 
        {
            get 
            {
                return Random.Range(0, dataPlayer.squad.Count);
            } 
        }
        private Dictionary<DataUnit, float> coundowns { get; set; } = new Dictionary<DataUnit, float>();

        public void OnAwake()
        {
            dataPlayer = actor.GetData<DataPlayer>();
            foreach (var item in dataPlayer.squad)
            {
                coundowns.Add(item, 0.0f);
            }

            dataPlayer.manaMax = dataPlayer.startMaxMana;
            dataPlayer.mana.Value = 0;

            manager = Toolbox.Get<GameManager>();
            managerUnits = Toolbox.Get<ManagerUnits>();
            if (dataPlayer.side == Side.left)
            {
                enemyUnits = managerUnits.right;
            }
            else
            {
                enemyUnits = managerUnits.left;
            }

            difficult = actor.GetData<DataAi>().difficult;

            if (difficult == Difficult.easy)
            {
                timeToAction = 5f;
            }
            else if (difficult == Difficult.normal)
            {
                timeToAction = 3f;
            }
            else
            {
                timeToAction = 0f;                
            }

        }

        public void UpgradeMana()
        {
            dataPlayer.manaMax += dataPlayer.upgradeMana;
            dataPlayer.mana.Value = 0;
            needToSpawn = wontToSpawn;

        }

        public void SpawnUnit(DataUnit data)
        {
            if (dataPlayer)
            {
                coundowns[data] = data.stats.spawnCountdown;
                dataPlayer.mana.Value -= data.stats.manaCost;
                ManagerUnits.Spawn(data, dataPlayer.side);
                Think();
            }
                
        }

        private void Think()
        {
            needToSpawn = wontToSpawn;
            if (needToSpawn < dataPlayer.squad.Count - 1)
                if (dataPlayer.manaMax < dataPlayer.squad[needToSpawn].stats.manaCost)
                {
                    needToSpawn = dataPlayer.squad.Count + 1;
                }
        }

        public void Tick()
        {
            if (!manager)
                return;

            if (!manager.battle)
                return;

            if (difficult == Difficult.normal)
            {
                dataPlayer.Tower.GetData<DataUnit>().OnSpellCast.OnNext(dataPlayer.Tower);
            }
            else if (enemyUnits.Count > 3 && difficult == Difficult.hard)
            {
                dataPlayer.Tower.GetData<DataUnit>().OnSpellCast.OnNext(dataPlayer.Tower);

            }

            if (dataPlayer.mana.Value < dataPlayer.manaMax & dataPlayer.manaFlows)
            {
                var delta = Time.deltaTime * dataPlayer.manaMax / dataPlayer.upgradeMana * 15f;

                if (dataPlayer.mana.Value + delta > dataPlayer.manaMax)
                    dataPlayer.mana.Value = dataPlayer.manaMax;
                else
                    dataPlayer.mana.Value += delta;

            }

            if (needToSpawn < dataPlayer.squad.Count)
            {
                if (Time.time - (timer + coundowns[dataPlayer.squad[needToSpawn]]) > timeToAction)
                {
                    var unit = dataPlayer.squad[needToSpawn];

                    if (coundowns[unit] == 0f && dataPlayer.mana.Value >= unit.stats.manaCost)
                    {
                        SpawnUnit(unit);
                        timer = Time.time;
                    }
                    else
                    {
                        Think();
                    }
                }
            }
            else if (needToSpawn == dataPlayer.squad.Count + 1 && dataPlayer.mana.Value >= dataPlayer.manaMax)
            {
                UpgradeMana();
            }


            foreach (var item in dataPlayer.squad)
            {
                if (coundowns[item] != 0.0f)
                {
                    coundowns[item] -= Time.deltaTime;
                    if (coundowns[item] < 0.0f)
                        coundowns[item] = 0.0f;
                }
            }

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerFight;
using UniRx;

namespace Homebrew
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
    public class GameManager : ManagerBase, IAwake
    {
                
        [SerializeField]
        private DataPlayer prefabDataPlayer;

        [SerializeField]
        private DataUnits unitsBase;

        [SerializeField]
        private ActorPlayer prefabPlayer;

        [SerializeField]
        private ActorPlayer prefabEnume;
        
        public ActorPlayer Player { get; private set; }
        public ActorPlayer Enemy { get; private set; }

        public DataPlayer dataPlayer { get; private set; }

        public DataUnits UnitBase { get => unitsBase; }

        public bool battle = false;

        public const int SquadMaxSize = 5;

        public void OnAwake()
        {

            if (prefabDataPlayer)
            {
                var data = SaveSystem.Load(UnitBase, prefabDataPlayer);
                if (data)
                {
                    dataPlayer = data;
                }
                else
                {
                    dataPlayer = DataPlayer.Copy(prefabDataPlayer);
                    dataPlayer.playerUnits = new List<DataUnit>();
                    foreach (var item in UnitBase.units)
                    {
                        dataPlayer.playerUnits.Add(DataUnit.Copy(item));
                    }

                    dataPlayer.playerTowers = new List<DataUnit>();
                    foreach (var item in UnitBase.towers)
                    {
                        dataPlayer.playerTowers.Add(DataUnit.Copy(item));
                    }
                    dataPlayer.dataTower = dataPlayer.playerTowers[0];
                    dataPlayer.squad = new List<DataUnit>() { dataPlayer.playerUnits[0] };
                }

              

                dataPlayer.Gold.Subscribe(delegate (int v) { Toolbox.Get<ManagerUI>().gameMenu.Gold = dataPlayer.Gold.ToString();  });
               
            }
            var managerUi = Toolbox.Get<ManagerUI>();
            managerUi.OnDifficultClick.Subscribe(OnDifficultClick).AddTo(Toolbox.Instance);
            managerUi.OnBattleClick.Subscribe(delegate (bool v) { OnBattleClick(managerUi); }).AddTo(Toolbox.Instance);
            managerUi.stand.OnUnitClick.Subscribe(delegate (Dictionary<string, object> dataList) { OnUnitClick(managerUi, dataList); }).AddTo(Toolbox.Instance);
            managerUi.unitMenu.Back.onClick.AddListener(delegate { OnBackUnitClick(managerUi); });
            managerUi.unitMenu.Upgrade.onClick.AddListener(delegate { OnUnitUpgrade(managerUi); }) ;
        }

        public void OnUnitUpgrade(ManagerUI manager)
        {
            var data = manager.unitMenu.dataUnit;
            dataPlayer.Gold.Value -= data.stats.UpgradeCost;
            data.stats.level += 1;
            manager.unitMenu.UpdateValues();
            SaveSystem.Save(dataPlayer);
        }

        private void OnBackUnitClick(ManagerUI manager)
        {         

            manager.OnCapture(StateUi.unit);             
            manager.stand.ActivateAllUnit(true);
       
            Camera.main.transform.position = new Vector3(0f, Camera.main.transform.position.y, Camera.main.transform.position.z);

        }

        private void OnUnitClick(ManagerUI manager, Dictionary<string, object> dataList)
        {
            var dataUnit = dataList[typeof(DataUnit).FullName] as DataUnit;
            var transform = dataList[typeof(Transform).FullName] as Transform;
            manager.SetState(StateUi.unit);
            manager.stand.ActivateUnit(dataUnit);
            manager.unitMenu.SetDataUnit(dataUnit);

            
            var x = manager.unitMenu.XUnitPoint - transform.position.x;
            var endX = Camera.main.transform.position.x - x;
            Camera.main.transform.position = new Vector3(endX, Camera.main.transform.position.y, Camera.main.transform.position.z);

        }

        private void OnBattleClick(ManagerUI manager)
        {
            manager.SetState(StateUi.difficalty);
        }

        private void OnDifficultClick(Difficult difficult)
        {
            Toolbox.Get<ManagerUI>().SetState(StateUi.battle);
            StartBattle(difficult);
        }


        public void StartBattle(Difficult difficult)
        {
       
            if (Player)
                Destroy(Player.gameObject);
            Player = Instantiate(prefabPlayer.gameObject).GetComponent<ActorPlayer>();
            Player.AddTo(dataPlayer);

            if (Enemy)
                Destroy(Enemy.gameObject);

            Enemy = Instantiate(prefabEnume);
            var dataAi = (CreateInstance(typeof(DataAi)) as DataAi);
            dataAi.difficult = difficult;
            Enemy.AddTo(dataAi);          
            Enemy.AddTo(CreateEnumeData(dataPlayer, difficult));
           
            Player.OnAwake();
            Player.StartBattle();
            Enemy.OnAwake();
            Enemy.StartBattle();
          
            battle = true;
        }

        private DataPlayer CreateEnumeData(DataPlayer dataPlayer, Difficult difficult)
        {
            var data = DataPlayer.Copy(dataPlayer);
            data.Player = false;
            List<DataUnit> units = new List<DataUnit>(unitsBase.units);

            float equals = 0f;
            foreach (var item in dataPlayer.squad)
            {
                equals += item.stats.level;
            }
            equals /= dataPlayer.squad.Count;

            int level = 1;
            if (difficult == Difficult.normal)
                level = Mathf.FloorToInt(equals);
            else if (difficult == Difficult.hard)
                level = Mathf.CeilToInt(equals);
                    

            List<DataUnit> squad = new List<DataUnit>();
            int value = units.Count > SquadMaxSize ? SquadMaxSize : units.Count;
            for (int i = 0; i < value; i++)
            {
                int index = Random.Range(0, units.Count);
                squad.Add(DataUnit.Copy(units[index]));
                squad[squad.Count-1].stats.level = level;
                units.RemoveAt(index);
            }
            data.squad = squad;
            data.dataTower = unitsBase.towers[Random.Range(0, unitsBase.towers.Count)];

            data.side = (dataPlayer.side == Side.left) ? Side.right : Side.left;
            return data;
        }


        public void EndGame(Side loser)
        {
            Time.timeScale = 1;
            battle = false;
            var managerUI = Toolbox.Get<ManagerUI>();
            managerUI.SetState(StateUi.battleEnd);
            if (loser == Player.GetData<DataPlayer>().side)
            {
                managerUI.endGameMenu.Message("Defeat");
                managerUI.endGameMenu.Reward = $"+{0}";
            }
            else
            {
                managerUI.endGameMenu.Message("Victory");

                var reward = GetReward(Enemy.GetData<DataAi>().difficult);
                dataPlayer.Gold.Value += reward;
                managerUI.endGameMenu.Reward = $"+{reward}";
                SaveSystem.Save(dataPlayer);

            }
        }

        public int GetReward(Difficult difficult)
        {
         
            int dificultValue = 0;
            switch (difficult)
            {
                case Difficult.easy:
                    dificultValue = 0;
                    break;
                case Difficult.normal:
                    dificultValue = 1;

                    break;
                case Difficult.hard:
                    dificultValue = 2;
                    break;
                default:
                    break;
            }


            return 50 + 100 * dificultValue + Random.Range(0,50);
        }

        public void OnEndContinue()
        {
            Time.timeScale = 1f;
            ManagerUnits.DestroyAllUnits();

            Destroy(Enemy.GetData<DataPlayer>().Tower.gameObject);
            Destroy(Enemy.GetData<DataPlayer>());
            Destroy(Enemy.gameObject);


            Destroy(Player.GetData<DataPlayer>().Tower.gameObject);
            Destroy(Player.gameObject);

            Toolbox.Get<ManagerUI>().SetState(StateUi.menu);
        }

        public void PlayBattle()
        {
            Time.timeScale = 1.0f;
            Toolbox.Get<ManagerUI>().SetState(StateUi.battle);
        }
        public void PauseBattle()
        {

            Time.timeScale = 0.0f;
            Toolbox.Get<ManagerUI>().SetState(StateUi.pause);
        }

        public void ToMenu()
        {
            Toolbox.Get<ManagerUI>().SetState(StateUi.menu);
        }

    }


}
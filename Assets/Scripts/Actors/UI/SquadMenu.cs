using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

namespace TowerFight
{
    public class SquadMenu : ActorBase
    {
        [SerializeField]
        private GameObject squadContaner;

        [SerializeField]
        private GameObject unitsContaner;
        
        [SerializeField]
        private Button back;

        [SerializeField]
        private ButtonMenuSquad buttonMenuSquad;


        [SerializeField]
        private TextMeshProUGUI textMessage;

        public Dictionary<DataUnit, ButtonMenuSquad> units { get; private set; } = new Dictionary<DataUnit, ButtonMenuSquad>();
      
        public List<ButtonMenuSquad> squadButons { get; private set; } = new List<ButtonMenuSquad>();

        public ButtonMenuSquad towerButton { get; private set; }

        public TextMeshProUGUI message { get => textMessage; }

        public Button Back { get => back; }


        private GameManager gameManager;
        private ManagerUI managerUI;
        private DataPlayer dataPlayer;


        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }



        protected override void OnStart()
        {
            gameManager = Toolbox.Get<GameManager>();
            managerUI = Toolbox.Get<ManagerUI>();
            dataPlayer = gameManager.dataPlayer;

            SetSquad(dataPlayer.dataTower, dataPlayer.squad);
            SetAllUnits(dataPlayer.playerTowers, dataPlayer.playerUnits);

            message.gameObject.SetActive(false);
            Back.onClick.AddListener(OnBack);
            managerUI.OnSquadClick.Subscribe(OnSquadClick).AddTo(this);

        }

        private void OnSquadClick(bool v)
        {
            managerUI.SetState(StateUi.squad);
        }

        private void OnBack()
        {
            managerUI.OnCaptionClick.OnNext(StateUi.squad);
        }


        private void SetSquad(DataUnit tower, List<DataUnit> units)
        {

            ButtonListSpawn(new List<DataUnit>() { tower }, units, squadContaner, true);
        }
        private void SetAllUnits(List<DataUnit> towers, List<DataUnit> units)
        {
            this.units = ButtonListSpawn(towers, units, unitsContaner, false);
        }


        private Dictionary<DataUnit, ButtonMenuSquad> ButtonListSpawn(List<DataUnit> towers, List<DataUnit> units, GameObject parent, bool squadContainer)
        {
            int count = 0, maxCount = GameManager.SquadMaxSize;

            Dictionary<DataUnit, ButtonMenuSquad> buttons = new Dictionary<DataUnit, ButtonMenuSquad>();


            List<DataUnit> mix = new List<DataUnit>();


            for (int i = 0; i < units.Count; i++)
            {
               
                if ((i == 0) || ((i) % maxCount == 0))
                {                 
                    if (count < towers.Count)
                    {
                        mix.Add(towers[count]);
                       
                    }
                    count++;
                }
                mix.Add(units[i]);
            }

            if (mix.Count % (maxCount + 1) > 0)
            {
                int length = (maxCount + 1) - (mix.Count % (maxCount + 1));
                for (int i = 0; i < length; i++)
                {
                    mix.Add(null);
                }
            }
            if ( count < towers.Count)//mix.Count <= towers.Count + units.Count || units.Count < towers.Count
            {
               
                for (int i = count; i < towers.Count; i++)
                {
                    mix.Add(towers[i]);
                    for (int k = 0; k < maxCount; k++)
                    {
                        mix.Add(null);
                    }
                }
            }

            foreach (var item in mix)
            {
                ButtonMenuSquad button;

                button = Instantiate(buttonMenuSquad, parent.transform);

                button.transform.SetParent(parent.transform);

                if (item)
                {
                    button.Interacteble = true;
                    button.SetDataUnit(item);
                    
                    if (!squadContainer)
                    {
                        if (dataPlayer.squad.Contains(item))
                            button.Interacteble = false;
                        if (dataPlayer.playerTowers.Contains(item))                        
                            button.Interacteble = false;
                        
                     

                        button.Button.onClick.AddListener(delegate { AddToSquad(button); });
                    }
                    else
                    {
                        if (dataPlayer.dataTower.Equals(item))
                            towerButton = button;
                      

                        button.Button.onClick.AddListener(delegate { RemoveFromSquad(button); });
                    }
                    
                }
                else
                {
                    button.Interacteble = false;
                    button.TextActivate(false);
                }

                if(squadContainer && !dataPlayer.dataTower.Equals(item))
                    squadButons.Add(button);

                if (item)
                    buttons.Add(item, button);
                else
                {
                    if (!squadContainer)
                        buttons.Add(ScriptableObject.CreateInstance(typeof(DataUnit)) as DataUnit, button);
                    else
                    {
                        button.Button.onClick.AddListener(delegate { RemoveFromSquad(button); });
                    }
                }

            }

            return buttons;
        }


        private void AddToSquad(ButtonMenuSquad button)
        {
            button.Interacteble = false;

            if (dataPlayer.playerTowers.Contains(button.dataUnit))
            {
                foreach (var item in dataPlayer.playerTowers)
                {
                    units[item].Interacteble = false;
                }
                dataPlayer.dataTower = button.dataUnit;

                towerButton.TextActivate(true);
                towerButton.Interacteble = true;
                towerButton.SetDataUnit(button.dataUnit);
                back.gameObject.SetActive(true);
                message.gameObject.SetActive(false);

            }
            else
            {
                var b = squadButons.Find(x => x.dataUnit == null);
                if (b)
                {
                    b.Interacteble = true;
                    b.TextActivate(true);
                    b.SetDataUnit(button.dataUnit);
                    dataPlayer.squad.Add(button.dataUnit);
                    if (dataPlayer.squad.Count == GameManager.SquadMaxSize)
                    {
                        foreach (var item in units)
                        {
                            if(item.Value.dataUnit)
                                item.Value.Interacteble = false;
                        }
                    }
                }
            }
            SaveSystem.Save(dataPlayer);
        }



        public void RemoveFromSquad(ButtonMenuSquad button)
        {
            button.Interacteble = false;
            button.TextActivate(false);

            if (dataPlayer.playerTowers.Contains(button.dataUnit))
            {
                foreach (var item in dataPlayer.playerTowers)
                {
                    units[item].Interacteble = true;
                }
                dataPlayer.dataTower = null;

                towerButton.dataUnit = null;
                towerButton.TextActivate(false);
                towerButton.Interacteble = false;
                back.gameObject.SetActive(false);
                message.gameObject.SetActive(true);
            }
            else
            {

                if (dataPlayer.squad.Count == GameManager.SquadMaxSize)
                {
                    foreach (var item in units)
                    {
                        if(item.Value.dataUnit)
                            if(!dataPlayer.squad.Contains(item.Key) && !dataPlayer.playerTowers.Contains(item.Key))

                                item.Value.Interacteble = true;
                    }
                }

                units[button.dataUnit].Interacteble = true;
                dataPlayer.squad.Remove(button.dataUnit);
                                           

                int count = 0;
                foreach (var item in squadButons)
                {
                    if (dataPlayer.squad.Count > count)
                    {

                        item.Interacteble = true;
                        item.TextActivate(true);
                        item.SetDataUnit(dataPlayer.squad[count]);
                        count++;
                    }
                    else
                    {
                        item.dataUnit = null;
                        item.TextActivate(false);
                        item.Interacteble = false;
                    }                    
                }             

            }
            SaveSystem.Save(dataPlayer);


        }


    }
}
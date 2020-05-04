using Homebrew;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFight
{
    public class GameMenu : ActorBase
    {
        [SerializeField]
        private Button battle;

        [SerializeField]
        private Button squad;

        [SerializeField]
        private Button options;

        [SerializeField]
        private TextMeshProUGUI gold;

        public string Gold 
        {
            get => gold?.text;
            set 
            {
                if (gold)
                {
                    gold.text = value;
                }
                else
                {
                    Debug.Log($"Gold text is null. Class {GetType()}. Value = {value}");
                }
            }
        
        }

        public Button Battale { get => battle; }

        public Button Squad { get => squad; }

        public Button Option { get => options; }

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        protected override void OnStart()
        {
            battle.onClick.AddListener(delegate { Toolbox.Get<ManagerUI>().OnBattleClick.OnNext(Activate); });
            squad.onClick.AddListener(delegate { Toolbox.Get<ManagerUI>().OnSquadClick.OnNext(Activate); });
            options.onClick.AddListener(delegate { Toolbox.Get<ManagerUI>().OnOptionsClick.OnNext(Activate); });

        }
    }
}
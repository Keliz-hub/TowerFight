using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFight {
    public class ButtonMenuSquad : ActorBase
    {
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]

        private TextMeshProUGUI _manaCost;

        [SerializeField]
        private TextMeshProUGUI _level;

        [SerializeField]
        private Button button;

        public DataUnit dataUnit { get; set; }

        public Button Button { get => button; }

        public string Name
        {
            get => _name.text;
            set => _name.text = value;
        }


        public string ManaCost
        {
            get => _manaCost.text;
            set => _manaCost.text = value;
        }

        public string Level
        {
            get => _level.text;
            set => _level.text = value;
        }

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public bool Interacteble
        {
            get => button.interactable;
            set => button.interactable = value;
        }

        public void SetDataUnit(DataUnit dataUnit)
        {
            if (dataUnit.stats.manaCost > 0)
                ManaCost = dataUnit.stats.manaCost.ToString();
            else
                ManaCost = "Tower";
            Name = dataUnit.stats.name;
            Level = dataUnit.stats.level.ToString();
            this.dataUnit = dataUnit;
        }



        public void TextActivate(bool v)
        {
            _name.gameObject.SetActive(v);
            _manaCost.gameObject.SetActive(v);
            _level.gameObject.SetActive(v);

        }


    }
}
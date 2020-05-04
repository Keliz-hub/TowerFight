using Homebrew;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFight
{
    public class UnitMenu : ActorBase
    {
        [SerializeField]
        private TextMeshProUGUI health;
        [SerializeField]
        private TextMeshProUGUI attackSpeed;
        [SerializeField]
        private TextMeshProUGUI damage;
        [SerializeField]
        private TextMeshProUGUI moveSpeed;
        [SerializeField]
        private TextMeshProUGUI level;
        [SerializeField]
        private TextMeshProUGUI nameUnit;
        [SerializeField]
        private TextMeshProUGUI upgradeCount;
        [SerializeField]
        private Transform unitPoint;
        [SerializeField]
        private TextMeshProUGUI gold;
        [SerializeField]
        private TextMeshProUGUI countdown;
        [SerializeField]
        private TextMeshProUGUI manaCost;


        [Space(10)]
        [SerializeField]
        private Button back;
        [SerializeField]
        private Button upgrade;

        public DataUnit dataUnit { get; private set; }

        public float XUnitPoint => unitPoint.position.x;

        public Button Back => back;
        public Button Upgrade => upgrade;

        #region Filde
        public string ManaCost
        {
            get => manaCost?.text;
            set
            {
                if (manaCost)
                    manaCost.text = value;
                else
                    Debug.Log($"manaCost text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string Countdown
        {
            get => countdown?.text;
            set
            {
                if (countdown)
                    countdown.text = value;
                else
                    Debug.Log($"countdown text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string Gold
        {
            get => gold?.text;
            set
            {
                if (gold)
                    gold.text = value;
                else
                    Debug.Log($"upgradeCount text is null. Class {GetType()}. Value = {value}");

            }

        }

        public string UpgradeCount
        {
            get => upgradeCount?.text;
            set
            {
                if (upgradeCount)
                    upgradeCount.text = value;
                else
                    Debug.Log($"upgradeCount text is null. Class {GetType()}. Value = {value}");
            }

        }

        public string Health
        {
            get => health?.text;
            set
            {
                if (health)
                    health.text = value;
                else
                    Debug.Log($"health text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string AttackSpeed
        {
            get => attackSpeed?.text;
            set
            {
                if (attackSpeed)
                    attackSpeed.text = value;
                else
                    Debug.Log($"attackSpeed text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string Damage
        {
            get => damage?.text;
            set
            {
                if (damage)
                    damage.text = value;
                else
                    Debug.Log($"damage text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string MoveSpeed
        {
            get => moveSpeed?.text;
            set
            {
                if (moveSpeed)
                    moveSpeed.text = value;
                else
                    Debug.Log($"moveSpeed text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string Level
        {
            get => level?.text;
            set
            {
                if (level)
                    level.text = value;
                else
                    Debug.Log($"level text is null. Class {GetType()}. Value = {value}");
            }

        }
        public string NameUnit
        {
            get => nameUnit?.text;
            set
            {
                if (nameUnit)
                    nameUnit.text = value;
                else
                    Debug.Log($"nameUnit text is null. Class {GetType()}. Value = {value}");
            }

        }
        #endregion



        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }


        public void SetDataUnit(DataUnit dataUnit)
        {

            this.dataUnit = dataUnit;
            UpdateValues();

        }

        public void UpdateValues()
        {
            Health = $"{dataUnit.stats.MaxHitPoint}";
            Damage = $"{dataUnit.stats.Damage}";
            AttackSpeed = $"{dataUnit.stats.attackSpeed} hits/cek";
            string moveSpeed = (dataUnit.stats.speed > 0f) ? dataUnit.stats.speed.ToString() : "Tower";
            MoveSpeed = $"{moveSpeed}";
            Level = $"{dataUnit.stats.level}";
            NameUnit = $"{dataUnit.stats.name}";
            UpgradeCount = $"{dataUnit.stats.UpgradeCost}";
            Countdown = $"{((dataUnit.stats.spawnCountdown > 0f) ? dataUnit.stats.spawnCountdown.ToString() : "Tower")}";
            ManaCost = $"{((dataUnit.stats.manaCost > 0f) ? dataUnit.stats.manaCost.ToString() : "Tower")}";
            UpgradeCostUpdate(Toolbox.Get<GameManager>().dataPlayer);
        }

        private void UpgradeCostUpdate(DataPlayer data)
        {
            upgrade.interactable = Toolbox.Get<GameManager>().dataPlayer.Gold.Value >= dataUnit.stats.UpgradeCost;
            Gold = data.Gold.Value.ToString();
        }


        protected override void OnStart()
        {
            var dataPlayer = Toolbox.Get<GameManager>().dataPlayer;
            dataPlayer.Gold.Subscribe(delegate { OnGoldChange(dataPlayer); }).AddTo(this);


        }

        private void OnGoldChange(DataPlayer data)
        {
            if (dataUnit)
                UpgradeCostUpdate(data);

        }
    }
}
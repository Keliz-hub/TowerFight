using Homebrew;
using System;
using UniRx;
using UnityEngine;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "PlayerBehaviour", menuName = "Behaviour/PlayerBehaviour")]
    public class PlayerBehaviour : BehaviourBase, IAwake, ITick
    {
        private DataPlayer dataPlayer;
        private ManagerUI managerUI;
        private GameManager gameManager;
        public void OnAwake()
        {
            dataPlayer = actor.GetData<DataPlayer>();
            if (dataPlayer.Player)
            {
                gameManager = Toolbox.Get<GameManager>();
                managerUI = Toolbox.Get<ManagerUI>();
                managerUI.pause.onClick.AddListener(OnPause);
                managerUI.OnButtonClick.Where(x => x.buttonPressed == ButtonPressed.unit).Subscribe(OnUnitButton).AddTo(actor);
                managerUI.OnButtonClick.Where(x => x.buttonPressed == ButtonPressed.mana).Subscribe(OnManaButton).AddTo(actor);

                managerUI.OnPauseClick.Subscribe(OnPauseOff).AddTo(actor);
                managerUI.endGameMenu.button.onClick.AddListener(OnEndGameContinue);

                managerUI.pauseMenu.Back.onClick.AddListener(OnPauseBack);

                managerUI.mana.maxCount = dataPlayer.manaMax = dataPlayer.startMaxMana;
                managerUI.mana.upCount = dataPlayer.manaMax + dataPlayer.upgradeMana;
                dataPlayer.mana.Value = 0;
            }

        }

 
        public void OnPauseBack()
        {          
            gameManager.EndGame(dataPlayer.side);
        }
        public void OnEndGameContinue()
        {
            gameManager.OnEndContinue();
        }


        private void OnPause()
        {
            gameManager.PauseBattle();
        }

        private void OnPauseOff(StateUi obj)
        {
            gameManager.PlayBattle();
        }


        private void OnManaButton(UICallBack callBack)
        {
            dataPlayer.manaMax += dataPlayer.upgradeMana;
            managerUI.mana.maxCount = dataPlayer.manaMax;
            managerUI.mana.upCount = dataPlayer.manaMax + dataPlayer.upgradeMana;
            dataPlayer.mana.Value = 0;
        }

        private void OnUnitButton(UICallBack callBack)
        {
            var dataUnit = callBack.data as DataUnit;
            if (dataUnit) 
            {
                dataPlayer.mana.Value -= dataUnit.stats.manaCost;
                ManagerUnits.Spawn(dataUnit, dataPlayer.side);
            }
        }

        public void Tick()
        {
            if (!gameManager.battle)
                return;

            if (dataPlayer.mana.Value < dataPlayer.manaMax & dataPlayer.manaFlows)
            {
                var delta = Time.deltaTime * dataPlayer.manaMax / dataPlayer.upgradeMana * 15f;
                if (dataPlayer.mana.Value + delta > dataPlayer.manaMax)
                    dataPlayer.mana.Value = dataPlayer.manaMax;
                else
                    dataPlayer.mana.Value += delta;
                if (managerUI)
                {
                    managerUI.mana.count = dataPlayer.mana.Value;
                    managerUI.mana.interactable = false;
                }



            }
            else if (dataPlayer.mana.Value == dataPlayer.manaMax)
            {
                if (managerUI)
                    if (!managerUI.mana.interactable)
                         managerUI.mana.interactable = true;
            }

        }

    }
}
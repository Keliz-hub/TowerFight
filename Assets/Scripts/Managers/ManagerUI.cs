using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TowerFight;
using System;

namespace Homebrew
{
    [CreateAssetMenu(fileName = "ManagerUI", menuName = "Managers/ManagerUI")]
    public class ManagerUI : ManagerBase, IAwake
    {

        #region Subject

        public Subject<UICallBack> OnButtonClick { get; private set; } = new Subject<UICallBack>();

        public Subject<bool> OnBattleClick { get; private set; } = new Subject<bool>();
        public Subject<bool> OnSquadClick { get; private set; } = new Subject<bool>();
        public Subject<bool> OnOptionsClick { get; private set; } = new Subject<bool>();
        public Subject<Difficult> OnDifficultClick { get; private set; } = new Subject<Difficult>();
        public Subject<StateUi> OnCaptionClick { get; private set; } = new Subject<StateUi>();

        public Subject<StateUi> OnPauseClick { get; private set; } = new Subject<StateUi>();

        public Subject<StateUi> OnMenuClick { get; private set; } = new Subject<StateUi>();

        #endregion

        #region Public
        public List<ButtonCountdown> unitButtons { get; private set; } = new List<ButtonCountdown>();
        public HealthBar heathLeftTowerBar { get; private set; }
        public HealthBar heathRightTowerBar { get; private set; }
        public ManaUI mana { get; private set; }
        public ButtonCountdown spell { get; private set; }
        public Button pause { get; private set; }
        public GameMenu gameMenu { get; private set; }
        public DifficultMenu difficultMenu { get; private set; }
        public EndGameMenu endGameMenu { get; private set; }
        public Stand stand { get; private set; }
        public PauseMenu pauseMenu { get; private set; }

        public SquadMenu squadMenu { get; private set; }

        public UnitMenu unitMenu { get; private set; }

        #endregion

        #region Private

        [SerializeField]
        private DataUI dataUI = null;

        private GameObject ui;
        private GameObject canvas;

        private GameObject battleMenuConteiner;

        private GameObject battleLayout;

        private string uiName = "[UI]";

        private StateUi state { get; set; } = StateUi.none;


        #endregion

        public void OnAwake()
        {
            if (debug)
                Debug.Log($"{GetType()}");

            if (!dataUI)
            {
                if (debug)
                    Debug.LogError($"{GetType()} do not have {typeof(DataUI)}");
                return;
            }
            ui = GameObject.Find(uiName);
            if (!ui)
                ui = new GameObject(uiName);
            if (dataUI.canvas)
            {
                canvas = Instantiate(dataUI.canvas, ui.transform);
                canvas.transform.SetParent(ui.transform, false);
                canvas.GetComponent<Canvas>().worldCamera = Camera.main;
            }

            SpawnBattleUi();
            SpawnMenuUi();

        }

        public void OnCapture(StateUi state)
        {
            this.state = state;
            if (state == StateUi.difficalty || state == StateUi.squad || state == StateUi.unit)
                SetState(StateUi.menu);
        }

        private void SpawnMenuUi()
        {
            gameMenu = SpawnUIElement(dataUI.gameMenu.gameObject, canvas, false)?.GetComponent<GameMenu>();

            squadMenu = SpawnUIElement(dataUI.squadMenu.gameObject, canvas, false)?.GetComponent<SquadMenu>();
            squadMenu.Back.onClick.AddListener(delegate { stand.UpdateStand(); OnCapture(StateUi.squad); });
         
            OnBattleClick.Subscribe(delegate (bool v) { gameMenu.Activate = false; });

            difficultMenu = SpawnUIElement(dataUI.difficultMenu.gameObject, canvas, false)?.GetComponent<DifficultMenu>();

            OnDifficultClick.Subscribe(delegate (Difficult v) { difficultMenu.Activate = false; }).AddTo(Toolbox.Instance);
            OnCaptionClick.Subscribe(OnCapture).AddTo(Toolbox.Instance);

            unitMenu = SpawnUIElement(dataUI.unitMenu.gameObject, canvas, false).GetComponent<UnitMenu>();
            stand = SpawnUIElement(dataUI.stand.gameObject, ui, false).GetComponent<Stand>();
        }

        private void SpawnBattleUi()
        {

            battleMenuConteiner = Instantiate(dataUI.battleMenuContainer, canvas.transform);
            battleMenuConteiner.transform.SetParent(canvas.transform);

            battleLayout = SpawnUIElement(dataUI.container, battleMenuConteiner, false);

            heathLeftTowerBar = SpawnUIElement(dataUI.prefabLeftBar.gameObject, battleMenuConteiner, false).GetComponent<HealthBar>();
            heathRightTowerBar = SpawnUIElement(dataUI.prefabRightBar.gameObject, battleMenuConteiner, false).GetComponent<HealthBar>();

            pause = SpawnUIElement(dataUI.prefabPause.gameObject, battleMenuConteiner, false).GetComponent<Button>();

            pauseMenu = SpawnUIElement(dataUI.pauseMenu.gameObject, canvas, false).GetComponent<PauseMenu>();

            endGameMenu = SpawnUIElement(dataUI.endGame.gameObject, canvas, false).GetComponent<EndGameMenu>();

            if (dataUI.spell)
            {
                spell = Instantiate(dataUI.spell);
                spell.transform.SetParent(battleLayout.transform, false);                
            }

            if (dataUI.mana)
            {
                mana = Instantiate(dataUI.mana);
                mana.transform.SetParent(battleLayout.transform, false);
            }
        }

        private GameObject SpawnUIElement(GameObject what, GameObject parent, bool v)
        {
            GameObject inst = null;
            if (what)
            {
                inst = Instantiate(what, canvas.transform);
                inst.transform.SetParent(parent.transform, v);

            }
            return inst;
        }

        public void SetState(StateUi newState)
        {
            switch (state)
            {
                case StateUi.none:
                    gameMenu.Activate = false;
                    difficultMenu.Activate = false;
                    BattleMenuActivate(false);
                    stand.Activate = false;
                    endGameMenu.Activate = false;
                    pauseMenu.Activate = false;
                    squadMenu.Activate = false;
                    unitMenu.Activate = false;
                    break;
                case StateUi.menu:                   
                    gameMenu.Activate = false;
                    stand.Activate = false;
                    break;
                case StateUi.difficalty:
                    difficultMenu.Activate = false;
                    break;
                case StateUi.squad:
                    squadMenu.Activate = false;
                    break;
                case StateUi.battle:
                    break;
                case StateUi.battleEnd:
                    BattleMenuActivate(false);
                    endGameMenu.Activate = false;

                    break;
                case StateUi.pause:
                    pauseMenu.Activate = false;
                    break;
                case StateUi.option:
                    break;
                case StateUi.unit:
                    unitMenu.Activate = false;
                    break;
                default:
                    break;
            }

       

            switch (newState)
            {
                case StateUi.menu:
                    gameMenu.Activate = true;
                    stand.Activate = true;
                    break;
                case StateUi.squad:
                    squadMenu.Activate = true;
                    break;
                case StateUi.difficalty:
                    difficultMenu.Activate = true;
                    break;
                case StateUi.battle:
                    BattleMenuActivate(true);
                    break;
                case StateUi.pause:
                    pauseMenu.Activate = true;
                    break;
                case StateUi.battleEnd:
                    endGameMenu.Activate = true;
                    BattleMenuActivate(false);
                    break;
                case StateUi.unit:
                    unitMenu.Activate = true;
                    stand.Activate = true;
                    break;
                default:
                    break;
            }

            state = newState;
        }



        public void BattleButtonSpawn(DataPlayer dataPlayer)
        {
            foreach (var item in unitButtons)
            {
                Destroy(item.gameObject);
            }
            unitButtons = new List<ButtonCountdown>();
            foreach (var unit in dataPlayer.squad)
            {
                var button = Instantiate(dataUI.unit, battleLayout.transform);
                button.transform.SetParent(battleLayout.transform, false);
                button.manaCost = unit.stats.manaCost;

                button.Button.onClick.AddListener(
                    delegate
                    {
                        ButtonUnitDown(new UICallBack(ButtonPressed.unit, unit), button, dataPlayer);
                    });
                button.interactable = false;
                button.SetName(unit.stats.name);
                dataPlayer.mana.Subscribe(
                    delegate
                    {
                        button.interactable = (dataPlayer.mana.Value >= unit.stats.manaCost && button.countdown == 0.0f) ? true : false;
                    }).AddTo(button.gameObject);

                unitButtons.Add(button);
            }

            if (spell)
            {
                var dataSpell = dataPlayer.Tower.GetData<DataSpell>();
                if (dataSpell)
                    if (dataSpell.icon)
                    {
                        spell.SetSprite(dataSpell.icon);
                        spell.Button.onClick.RemoveAllListeners();
                        spell.Button.onClick.AddListener(
                        delegate
                        {
                            ButtonSpellDown(new UICallBack(ButtonPressed.spell, dataSpell), spell, dataPlayer);
                        });

                    }
            }


            if (heathLeftTowerBar && heathRightTowerBar)
            {
                heathLeftTowerBar.health = heathRightTowerBar.health = 1;
            }
       
        }

        private void ButtonSpellDown(UICallBack callBack, ButtonCountdown button, DataPlayer dataPlayer)
        {
            button.StartCountdown((callBack.data as DataSpell).countdown);
            Toolbox.Get<ManagerUI>().OnButtonClick.OnNext(callBack);
        }

        private void ButtonUnitDown(UICallBack callBack, ButtonCountdown button, DataPlayer dataPlayer)
        {
            button.StartCountdown(callBack.data as DataUnit, dataPlayer);
            Toolbox.Get<ManagerUI>().OnButtonClick.OnNext(callBack);
        }

        public void BattleMenuActivate(bool value)
        {
            battleMenuConteiner.SetActive(value);
            pause.gameObject.SetActive(value);
        }     
    }
    public enum StateUi
    {
        none,
        menu,
        difficalty,
        squad,
        battle,
        battleEnd,
        pause,
        option,
        unit
    }
}

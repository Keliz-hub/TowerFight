using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFight
{
    [CreateAssetMenu(fileName = "DataUI", menuName = "Data/DataUI")]
    public class DataUI : DataBase
    {
        [Space(10)]
        public GameObject canvas;

        [Header("GameMenu")]
        [Space(10)]
        public GameMenu gameMenu;
        public DifficultMenu difficultMenu;
        public SquadMenu squadMenu;
        public UnitMenu unitMenu;
        public Stand stand;

        [Header("BattleMenu")]
        [Space(10)]
        public GameObject battleMenuContainer;
        public GameObject container;

        [Space(10)]
        public PauseMenu pauseMenu;
        public EndGameMenu endGame;

        [Space(10)]
        public ButtonCountdown spell;
        public ButtonCountdown unit;

        [Space(10)]
        public ManaUI mana;

        [Space(10)]
        public HealthBar prefabLeftBar;
        public HealthBar prefabRightBar;

        [Space(10)]
        public Button prefabPause;



    }

    public class UICallBack 
    {
        public ButtonPressed buttonPressed;
        public DataBase data;

        public UICallBack(ButtonPressed buttonPressed, DataBase data)
        {
            this.buttonPressed = buttonPressed;
            this.data = data;
        }
    }


    public enum ButtonPressed
    {
        spell,
        unit,
        mana
    }

}
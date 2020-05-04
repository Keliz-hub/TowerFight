using Homebrew;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerFight
{
    public class PauseMenu : ActorBase
    {
        [SerializeField]
        private TextMeshProUGUI text; 

        [SerializeField]
        private Button _continueButton;

        [SerializeField]
        private Button capture;

        [SerializeField]
        private Button back;

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }


        public Button Continue { get => _continueButton; }
        public Button Back { get => back; }
        public string Text 
        {
            get => text.text;
            set => text.text = value;
        }

        protected override void OnStart()
        {
            capture.onClick.AddListener(OnPauseClick);
           // back.onClick.AddListener(OnPauseClick);
            _continueButton.onClick.AddListener(OnPauseClick);
        }

        private void OnPauseClick()
        {
            Toolbox.Get<ManagerUI>().OnPauseClick.OnNext(StateUi.pause);
        }
    }
}
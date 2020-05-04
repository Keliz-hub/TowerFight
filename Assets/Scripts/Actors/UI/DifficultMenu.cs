using Homebrew;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace TowerFight
{
    public class DifficultMenu : ActorBase
    {
        [SerializeField]
        private Button cupture;

        [SerializeField]
        private Button back;

        [SerializeField]
        private Button easy;

        [SerializeField]
        private Button normal;

        [SerializeField]
        private Button hard;

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        protected override void OnStart()
        {
            if(back)
                back.onClick.AddListener(OnBack);
            if (cupture)
                cupture.onClick.AddListener(OnBack);
            if (easy)
                easy.onClick.AddListener(delegate { StartBattle(Difficult.easy); });
            if (normal)
                normal.onClick.AddListener(delegate { StartBattle(Difficult.normal); });
            if (hard)
                hard.onClick.AddListener(delegate { StartBattle(Difficult.hard); });


        }


        private void OnBack()
        {
            Toolbox.Get<ManagerUI>().OnCaptionClick.OnNext(StateUi.difficalty);
        }

        private void StartBattle(Difficult difficult)
        {
            Toolbox.Get<ManagerUI>().OnDifficultClick.OnNext(difficult);
            Activate = false;
        }

    }
}
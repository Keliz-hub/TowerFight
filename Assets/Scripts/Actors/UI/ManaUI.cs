using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Homebrew;
using TMPro;
using UnityEngine.UI;

namespace TowerFight
{
    public class ManaUI : ActorBase
    {

        #region Private

        [SerializeField]
        private TextMeshProUGUI _countMesh;

        [SerializeField]
        private TextMeshProUGUI _maxCountMesh;

        [SerializeField]
        private TextMeshProUGUI _upCountMesh;

        [SerializeField]
        private TextMeshProUGUI _maxTextMesh;
        
        [Space(20)]
        [SerializeField]
        private Button button;

        [Space(20)]
        [SerializeField]
        private string arrow = "↑";

        [Space(20)]
        [SerializeField]
        private int _count;

        [SerializeField]
        private int _maxCount;

        [SerializeField]
        private int _upCount;
        #endregion

        #region Public
        public float count
        {
            get
            {

                return _count;
            }
            set
            {
                _count = (int)value;
                if (_countMesh)
                    _countMesh.text = _count.ToString();
            }
        }

        public float upCount
        {
            get
            {
                return _upCount;
            }
            set
            {
                _upCount = (int)value;
                if (_countMesh)

                    _upCountMesh.text = $"{arrow}{ _upCount}";
            }
        }

        public float maxCount
        {
            get
            {
                return _maxCount;
            }

            set
            {
                _maxCount = (int)value;
                if (_countMesh)

                    _maxCountMesh.text = _maxCount.ToString();
            }
        }

        public bool interactable
        {
            get => button ? button.interactable : false;
            set
            {
                if (button)
                    button.interactable = value;
            }

        }

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        public Button Button
        {
            get
            {
                return button;
            }
        }

        #endregion

        protected override void OnStart()
        {
            count = _count;
            upCount = _upCount;
            maxCount = _maxCount;
            Button.onClick.AddListener(delegate { Toolbox.Get<ManagerUI>().OnButtonClick.OnNext(new UICallBack(ButtonPressed.mana, null)); });           
        }
    }
}
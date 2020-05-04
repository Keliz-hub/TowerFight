using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Homebrew;
namespace TowerFight
{
    public class ButtonCountdown : ActorBase
    {
        #region private

        [SerializeField]
        private TextMeshProUGUI countdownText;

        [SerializeField]
        private TextMeshProUGUI manaCostText;

        [SerializeField]
        private TextMeshProUGUI name;

        [SerializeField]
        private Button button;

        private float countdownValue = 0;

        private int manaCostValue = 0;


        #endregion


        #region public
        public Button Button
        {
            get
            {
                return button;
            }
        }
        public float countdown
        {
            get
            {
                return countdownValue;
            }
            set
            {
                if (countdownText)
                {
                    if (value <= 0f)
                    {
                        countdownText.gameObject.SetActive(false);
                        countdownText.text = $"{0}";
                        countdownValue = 0;
                    }
                    else
                    {
                        if (!countdownText.gameObject.activeSelf)
                            countdownText.gameObject.SetActive(true);

                        countdownText.text = value.ToString("0.0");
                        countdownValue = value;
                    }


                }
            }
        }
        public int manaCost
        {
            get
            {
                return manaCostValue;
            }

            set
            {
                if (manaCostText)
                {
                    manaCostText.text = $"{value}";
                    manaCostValue = value;
                }
            }
        }
        public bool interactable
        {
            get => button ? button.interactable : false;
            set
            {
                if(button)
                    button.interactable = value;
            }

        }
        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
        #endregion



        public void SetName(string name)
        {
            if(this.name)
                this.name.text = name;
        }

        public void SetSprite(Sprite sprite)
        {
            var renderer =  GetComponent<Button>();
            if (renderer)
            {
                renderer.image.sprite = sprite;
            }
        }

    
        public void StartCountdown(float countdown)
        {
            if (button && countdownText)
                StartCoroutine(StartCountdown(countdown, 0.1f));
        }

        public void StartCountdown(DataUnit dataUnit, DataPlayer dataPlayer)
        {
            if (button && countdownText)
                StartCoroutine(StartCountdown(dataUnit, dataPlayer, 0.1f));
        }

        private IEnumerator StartCountdown(DataUnit dataUnit, DataPlayer dataPlayer, float deltaTime)
        {
            interactable = false;
            countdownText.gameObject.SetActive(true);
            for (float count = dataUnit.stats.spawnCountdown; count > 0; count -= deltaTime)
            {
                countdown = count;
                yield return new WaitForSeconds(deltaTime);
            }
            countdown = 0;
            countdownText.gameObject.SetActive(false);
            if (dataPlayer.mana.Value >= dataUnit.stats.manaCost)
                interactable = true;
        }

        private IEnumerator StartCountdown(float spawnCountdown, float deltaTime)
        {
            interactable = false;
            countdownText.gameObject.SetActive(true);
            for (float count = spawnCountdown; count > 0; count -= deltaTime)
            {
                countdown = count;
                yield return new WaitForSeconds(deltaTime);
            }
            countdown = 0;
            countdownText.gameObject.SetActive(false);
            interactable = true;
        }
    }
}
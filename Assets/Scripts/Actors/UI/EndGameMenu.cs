using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using TMPro;

namespace TowerFight
{
    public class EndGameMenu : ActorBase
    {
        [SerializeField]
        private Button continueButton;
        [SerializeField]
        private TextMeshProUGUI textMesh;

        [SerializeField]
        private TextMeshProUGUI reward;


        public string Reward
        {
            get => reward?.text;
            set
            {
                if (reward)
                {
                    reward.text = value;
                }
                else
                {
                    Debug.Log($"Gold text is null. Class {GetType()}. Value = {value}");
                }
            }

        }

        public Button button { get => continueButton; }

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public void Message(string message)
        {
            textMesh.text = message;
        }

    }
}
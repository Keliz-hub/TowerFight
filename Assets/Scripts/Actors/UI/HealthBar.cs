using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Homebrew;
using UnityEngine.UI;

namespace TowerFight
{
    public class HealthBar : ActorBase, ITick
    {
        [SerializeField]
        private Image bar;

        [SerializeField]
        private Image backBar;

        private float timeChangeBackBar = 1.5f;
        
        public float health
        {
            get 
            {
                if (bar)
                    return bar.fillAmount;
                else
                    return 0f;
            }

            set
            {
                if(bar)
                    if (value > 1f)
                        bar.fillAmount = 1.0f;
                    else if (value < 0)
                        bar.fillAmount = 0.0f;
                    else
                        bar.fillAmount = value;
            }
        }

        private float healthBackBar
        {
            get
            {
                if (bar && backBar)
                    return backBar.fillAmount;
                else
                    return 0f;
            }

            set
            {
                if (bar && backBar)
                    if (value > 1f)
                        backBar.fillAmount = 1.0f;
                    else if (value < 0)
                        backBar.fillAmount = 0.0f;
                    else
                        backBar.fillAmount = value;

            }
        }

        public bool Activate
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public static float OnHitChange(float MaxValue, float Value)
        {
            return Value/MaxValue;
        }

        protected override void OnStart()
        {
            healthBackBar = 1f;
        }
        private float delta = 0f;

        public void Tick()
        {
            if (health != healthBackBar)
            {
                if (health > healthBackBar)
                {
                    healthBackBar = health;
                    delta = 0f;
                }
                else if (health < healthBackBar)
                {
                    if (delta == 0f)
                        delta = ((healthBackBar - health) / (timeChangeBackBar * (healthBackBar - health))) * Time.deltaTime;
                    healthBackBar -= delta;
                }
            }
        }
    }
}
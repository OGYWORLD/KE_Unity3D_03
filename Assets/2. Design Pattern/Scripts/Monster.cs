using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject{

    public class Monster : MonoBehaviour
    {
        public Renderer[] bodyRenderers;
        public Renderer[] eyeRenderers;

        public Color bodyDayColor;
        public Color eyeDayColor;

        public Color bodyNightColor;
        public Color eyeNightColor;

        private void Start()
        {
            //GameManager.Instance.OnMonsterSpawn(this);

            GameManager.Instance.onDayNightChange += OnDayNightChange;
            OnDayNightChange(GameManager.Instance.isDay);
        }

        private void OnDestroy()
        {
            //GameManager.Instance.OnMonsterDespaen(this);
            GameManager.Instance.onDayNightChange -= OnDayNightChange;
        }

        public void OnDayNightChange(bool isDay)
        {
            if(isDay)
            {
                DayColor();
            }
            else
            {
                NightColor();
            }
        }

        // Facade Pattern
        public void DayColor()
        {
            foreach(Renderer r in bodyRenderers)
            {
                r.material.color = bodyDayColor;
            }

            foreach(Renderer r in eyeRenderers)
            {
                r.material.color = eyeDayColor;
            }
        }

        public void NightColor()
        {
            foreach (Renderer r in bodyRenderers)
            {
                r.material.color = bodyNightColor;
            }

            foreach (Renderer r in eyeRenderers)
            {
                r.material.color = eyeNightColor;
            }
        }
    }
}

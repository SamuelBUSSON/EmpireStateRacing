using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Twitch
{
    public class CommandLog : MonoBehaviour
    {
        private TextMeshProUGUI log;
        private float timer = 10;

        public void Init(string text)
        {
            log = GetComponent<TextMeshProUGUI>();
            log.text = text;
        }

        public void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 1)
            {
                Color c = log.color;
                c.a = timer;
                log.color = c;
            }

            if (timer <= 0) Destroy(gameObject);
        }
    }
}
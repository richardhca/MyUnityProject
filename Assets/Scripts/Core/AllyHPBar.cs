using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Player.Config;

namespace GameCore.Widget
{
    public class AllyHPBar : MonoBehaviour
    {
        [SerializeField] private GameObject HPBar;
        [SerializeField] private GameObject player;
        private float maxHP;
        private int previousHP;
        private float HPChangeTime;

        void Start()
        {
            maxHP = player.GetComponent<PlayerStats>().MaxHealth();
            GetComponent<Image>().fillAmount = 1.0f;
            HPBar.GetComponent<Canvas>().enabled = false;
            previousHP = player.GetComponent<PlayerStats>().MaxHealth();
            HPChangeTime = 10.0f; // Any number greater than 5.0 (seconds)
        }
        
        void Update()
        {
            GetComponent<Image>().fillAmount = player.GetComponent<PlayerStats>().Health / maxHP;
            if (player.GetComponent<PlayerStats>().Health != previousHP)
                HPChangeTime = 0.0f;
            HPBar.GetComponent<Canvas>().enabled = (player.GetComponent<PlayerStats>().IsDead()) ? (HPChangeTime < 1.0f) : (HPChangeTime < 3.0f);
            previousHP = player.GetComponent<PlayerStats>().Health;
            HPChangeTime += Time.deltaTime;
        }
    }
}

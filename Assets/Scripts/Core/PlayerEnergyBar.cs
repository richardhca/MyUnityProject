using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Player.Config;

namespace GameCore.Widget
{
    public class PlayerEnergyBar : MonoBehaviour
    {
        [SerializeField] private GameObject EnergyBar;
        [SerializeField] private GameObject player;

        void Start()
        {
            GetComponent<Image>().fillAmount = 1.0f;
            EnergyBar.GetComponent<Canvas>().enabled = true;
        }

        void Update()
        {
            GetComponent<Image>().fillAmount = player.GetComponent<PlayerStats>().Energy / 100.0f;
        }
    }
}

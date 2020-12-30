using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Player.Config;

namespace GameCore.Widget
{
    public class AllyHPBar : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private float maxHP;
        
        void Start()
        {
            maxHP = player.GetComponent<PlayerStats>().MaxHealth();
            GetComponent<Image>().fillAmount = 1.0f;
        }
        
        void Update()
        {
            GetComponent<Image>().fillAmount = player.GetComponent<PlayerStats>().Health / maxHP;
        }
    }
}

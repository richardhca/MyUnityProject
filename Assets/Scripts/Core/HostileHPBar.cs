using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Monster.Config;

namespace GameCore.Widget
{
    public class HostileHPBar : MonoBehaviour
    {
        [SerializeField] private GameObject enemy;
        private float maxHP;

        void Start()
        {
            maxHP = enemy.GetComponent<MonsterStats>().MaxHealth();
            GetComponent<Image>().fillAmount = 1.0f;
        }

        void Update()
        {
            GetComponent<Image>().fillAmount = enemy.GetComponent<MonsterStats>().Health / maxHP;
        }
    }
}

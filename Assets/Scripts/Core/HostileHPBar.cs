using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Monster.Config;

namespace GameCore.Widget
{
    public class HostileHPBar : MonoBehaviour
    {
        [SerializeField] private GameObject HPBar;
        [SerializeField] private GameObject enemy;
        private float maxHP;
        private int previousHP;
        private float HPChangeTime;

        void Start()
        {
            maxHP = enemy.GetComponent<MonsterStats>().MaxHealth();
            GetComponent<Image>().fillAmount = 1.0f;
            HPBar.GetComponent<Canvas>().enabled = false;
            previousHP = enemy.GetComponent<MonsterStats>().MaxHealth();
            HPChangeTime = 10.0f; // Any number greater than 5.0 (seconds)
        }

        void Update()
        {
            GetComponent<Image>().fillAmount = enemy.GetComponent<MonsterStats>().Health / maxHP;
            if (enemy.GetComponent<MonsterStats>().Health != previousHP)
                HPChangeTime = 0.0f;
            HPBar.GetComponent<Canvas>().enabled = (enemy.GetComponent<MonsterStats>().IsDead()) ? (HPChangeTime < 1.0f) : (HPChangeTime < 3.0f);
            previousHP = enemy.GetComponent<MonsterStats>().Health;
            HPChangeTime += Time.deltaTime;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Config
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] PlayerData playerInfo;
        [SerializeField] GameObject weapon;
        [SerializeField] GameObject arrow;
        [SerializeField] Transform arrowSpawn;
        
        private GameObject player;
        private CombatType attackType;
        private int HitPoint;
        private int Strength;
        private float Agility;
        private float Stamina;

        public GameObject Player => player;
        public GameObject Weapon => weapon;
        public GameObject Arrow => arrow;
        public Transform ArrowSpawn => arrowSpawn;

        public CombatType AttackType => attackType;
        public int Health => HitPoint;
        public int Attack => Strength;
        public float Speed => Agility;
        public float Energy => Stamina;

        void Start()
        {
            player = GameObject.FindWithTag("Player");
            attackType = playerInfo.ATKType;
            HitPoint = playerInfo.HP;
            Strength = playerInfo.STR;
            Agility = playerInfo.AGI / 20.0f;
            Stamina = 100.0f;
        }

        public int MaxHealth()
        {
            return playerInfo.HP;
        }

        public bool ConsumeStamina()
        {
            if (Stamina <= 0)
                return false;

            Stamina -= 0.5f;
            return true;
        }

        public void RestoreStamina()
        {
            Stamina = Mathf.Min(Stamina+0.75f, 100.0f);
        }

        public void TakeDamage(int damage)
        {
            HitPoint = Mathf.Max(HitPoint - damage, 0);
        }

        public bool IsDead()
        {
            return HitPoint == 0;
        }
    }  
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Config
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] GameObject player;
        [SerializeField] GameObject weapon;
        [SerializeField] GameObject arrow;
        [SerializeField] Transform arrowSpawn;

        private static readonly string[] Name = {"Kikyou"};
        private static readonly CombatType[] ATKType = { CombatType.Range };
        private static readonly int[] HP = { 100 };
        private static readonly int[] STR = { 8 };
        private static readonly float[] AGI = { 5.0f };

        private int i;
        private CombatType attackType;
        private int HitPoint;
        private int Strength;
        private float Agility;

        public GameObject Player => player;
        public GameObject Weapon => weapon;
        public GameObject Arrow => arrow;
        public Transform ArrowSpawn => arrowSpawn;

        public CombatType AttackType => attackType;
        public int Health => HitPoint;
        public int Attack => Strength;
        public float Speed => Agility;

        void Start()
        {
            i = Array.IndexOf(Name, transform.name);
            attackType = ATKType[i];
            HitPoint = HP[i];
            Strength = STR[i];
            Agility = AGI[i] / 20.0f;
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

public enum CombatType {Melee, Range};
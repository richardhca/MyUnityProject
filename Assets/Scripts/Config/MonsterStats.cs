using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Monster.Config
{
    public class MonsterStats : MonoBehaviour
    {
        private static readonly string[] Name = { "Skeleton_Samurai" };
        private static readonly CombatType[] ATKType = { CombatType.Melee };
        private static readonly int[] HP = { 100 };
        private static readonly int[] STR = { 5 };
        private static readonly float[] AGI = { 5.0f };
        private static readonly float[] ATKRANGE = { 4.5f };

        public static readonly float actionInterval = 2.0f;

        private int i;
        private CombatType attackType;
        private int HitPoint;
        private int Strength;
        private float Agility;
        private float AtkRange;

        public CombatType AttackType => attackType;
        public int Health => HitPoint;
        public int Attack => Strength;
        public float Speed => Agility;
        public float AttackRange => AtkRange;
        public float ActionInterval => actionInterval;
        
        void Start()
        {
            i = 0; // Array.IndexOf(Name, transform.name);
            attackType = ATKType[i];
            HitPoint = HP[i];
            Strength = STR[i];
            Agility = AGI[i];
            AtkRange = ATKRANGE[i];

            GetComponent<NavMeshAgent>().speed = Agility;
        }

        public int MaxHealth()
        {
            return HP[i];
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

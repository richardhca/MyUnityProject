using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Monster.Config
{
    public class MonsterStats : MonoBehaviour
    {
        [SerializeField] MonsterData monsterInfo;

        public static readonly float actionInterval = 2.0f;
        
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
            attackType = monsterInfo.ATKType;
            HitPoint = monsterInfo.HP;
            Strength = monsterInfo.STR;
            Agility = monsterInfo.AGI;
            AtkRange = monsterInfo.ATKRANGE;

            GetComponent<NavMeshAgent>().speed = Agility;
        }

        public int MaxHealth()
        {
            return monsterInfo.HP;
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

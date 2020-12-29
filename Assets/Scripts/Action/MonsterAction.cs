using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Monster.Config;
using Player.Config;
using Player.Action;

namespace Monster.Action
{
    public class MonsterAction : MonoBehaviour
    {
        private Animator monsterAnime;
        private bool freezeMove;
        private float actionTime;
        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            monsterAnime = GetComponent<Animator>();
            freezeMove = false;
            actionTime = Mathf.Infinity;
            player = GameObject.FindWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<MonsterStats>().IsDead()) return;

            if (!player.transform.GetChild(0).GetComponent<PlayerStats>().IsDead() && actionTime > GetComponent<MonsterStats>().ActionInterval)
            {
                Vector3 playerPosition = player.transform.position;
                if (Vector3.Distance(transform.position, playerPosition) > GetComponent<MonsterStats>().AttackRange)
                {
                    if (monsterAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack") || isUnderAttack())
                    {
                        stopMove();
                        actionTime = 0;
                        return;
                    }
                    GetComponent<NavMeshAgent>().destination = playerPosition;
                    performAction("Move");
                }
                else
                {
                    stopMove();
                    transform.LookAt(playerPosition);
                    performAction("Attack");
                }
            }
            else
            {
                performAction("Idle");
            }

            actionTime += Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("Player"))
            {

            }
        }

        private void performAction(string animeName)
        {
            switch (animeName)
            {
                case "Idle":
                    monsterAnime.SetTrigger("Idle");
                    monsterAnime.ResetTrigger("Attack");
                    monsterAnime.ResetTrigger("Move");  
                    break;
                case "Move":
                    monsterAnime.SetTrigger("Move");
                    monsterAnime.ResetTrigger("Idle");
                    monsterAnime.ResetTrigger("Attack");
                    break;
                case "Attack":
                    monsterAnime.SetTrigger("Attack");
                    monsterAnime.ResetTrigger("Move");
                    monsterAnime.ResetTrigger("Idle");
                    break;
            }
        }

        private void stopMove()
        {
            GetComponent<NavMeshAgent>().destination = transform.position;
            GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            performAction("Idle");
        }

        public void GetHit(int damage)
        {
            if (GetComponent<MonsterStats>().IsDead()) return;

            stopMove();
            GetComponent<MonsterStats>().TakeDamage(damage);
            if (GetComponent<MonsterStats>().IsDead())
            {
                monsterAnime.SetTrigger("Die");
                transform.GetComponent<Collider>().enabled = false;
            }
            else
            {
                monsterAnime.SetTrigger("Hit");
            }
        }

        private bool isUnderAttack()
        {
            return monsterAnime.GetCurrentAnimatorStateInfo(0).IsName("Hit") || monsterAnime.GetCurrentAnimatorStateInfo(0).IsName("HitReturn");
        }

        void HitCheck() // This function is used by animation event
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= GetComponent<MonsterStats>().AttackRange)
            {
                player.transform.GetChild(0).GetComponent<PlayerAction>().GetHit(GetComponent<MonsterStats>().Attack);
            }
        }
    }
}

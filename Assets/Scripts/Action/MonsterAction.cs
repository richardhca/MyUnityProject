using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Monster.Config;
using Player.Config;

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
            if (actionTime > GetComponent<MonsterStats>().ActionInterval)
            {
                Vector3 playerPosition = player.transform.position;
                if (Vector3.Distance(transform.position, playerPosition) > GetComponent<MonsterStats>().AttackRange)
                {
                    if (monsterAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        actionTime = 0;
                        return;
                    }
                    GetComponent<NavMeshAgent>().destination = playerPosition;
                    performAction("Move");
                }
                else
                {
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
                    monsterAnime.SetTrigger("StopAction");
                    monsterAnime.ResetTrigger("Attack");
                    monsterAnime.ResetTrigger("Move");
                    break;
                case "Move":
                    monsterAnime.SetTrigger("Move");
                    break;
                case "Attack":
                    monsterAnime.SetTrigger("Attack");
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore.View;
using Player.Config;

namespace Player.Action
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerAction : MonoBehaviour
    {
        private Animator playerAnime;
        private Animator weaponAnime;
        private List<string> ActionQueue;
        //private RigidbodyConstraints originalConstraints;
        private bool freezeMove;
        private bool secondJump;
        private bool landed;
        private bool arrowSpawned;
        private GameObject spawnArrow;

        void Start()
        {
            playerAnime = GetComponent<Animator>();
            weaponAnime = GetComponent<PlayerStats>().Weapon.GetComponent<Animator>();
            ActionQueue = new List<string>();
            //originalConstraints = player.GetComponent<Rigidbody>().constraints;
            freezeMove = false;
            secondJump = false;
            landed = true;
            arrowSpawned = false;
            spawnArrow = null;
            playerAnime.PlayInFixedTime("Idle");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("Terrain"))
                landed = true;
        }

        public void QueueAttack1Action()
        {
            AnimatorStateInfo currentAnime = playerAnime.GetCurrentAnimatorStateInfo(0);
            if (ActionQueue.Count == 0 || currentAnime.IsName("Attack_1") || currentAnime.IsName("Attack_2"))
            {
                if (ActionQueue.Count == 0)
                    ActionQueue.Add("Attack_1");
                else if (currentAnime.IsName("Attack_1") && ActionQueue.Count == 1)
                    ActionQueue.Add("Attack_2");
                else if (currentAnime.IsName("Attack_2") && ActionQueue.Count == 1)
                    ActionQueue.Add("Attack_3");
            }
        }

        public void QueueAttack2Action()
        {
            if (ActionQueue.Count == 0)
                ActionQueue.Add("LightAtk");
        }

        public void QueueJumpAction()
        {
            if (ActionQueue.Count == 0)
            {
                ActionQueue.Add("Jump");
                ActionQueue.Add("Glide");
                ActionQueue.Add("Land");
            }
            else
            {
                if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Jump") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
                    PerformSecondJump();
            }
        }

        public void PerformAction(string animeName)
        {
            if (ActionQueue.Count != 0) // Jump, Attack
            {
                if (!playerAnime.GetCurrentAnimatorStateInfo(0).IsName(ActionQueue[0]))
                {
                    playerAnime.PlayInFixedTime(ActionQueue[0]);
                    switch (ActionQueue[0].ToString())
                    {
                        case "Jump": // Jump
                            GetComponent<PlayerStats>().Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
                            landed = false;
                            secondJump = true;
                            break;
                        case "Land": // Land
                            freezeMove = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            break;
                        case "Attack_1":
                            weaponAnime.PlayInFixedTime("Attack_1");
                            freezeMove = true;
                            break;
                        case "Attack_2":
                            weaponAnime.PlayInFixedTime("Attack_2");
                            freezeMove = true;
                            break;
                        case "Attack_3":
                            weaponAnime.PlayInFixedTime("Attack_3");
                            freezeMove = true;
                            break;
                        case "LightAtk":
                            freezeMove = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            break;
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Jump") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
                {
                    if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Jump") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        ActionQueue.RemoveAt(0);

                    if (landed)
                    {
                        while (!ActionQueue[0].ToString().Equals("Land"))
                            ActionQueue.RemoveAt(0);
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_3"))
                {
                    //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f ||
                        playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f ||
                        playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_3") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
                    {
                        if (!arrowSpawned)
                        {
                            arrowSpawned = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            Quaternion angle = Quaternion.Euler(0, GetComponent<PlayerStats>().Player.transform.eulerAngles.y + transform.localEulerAngles.y, 0);
                            spawnArrow = Instantiate(GetComponent<PlayerStats>().Arrow, GetComponent<PlayerStats>().ArrowSpawn.position, angle);
                            spawnArrow.transform.localPosition += spawnArrow.transform.forward * 1.2f;
                            if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_1"))
                                spawnArrow.transform.localPosition += spawnArrow.transform.up * 0.1f;
                            else
                                spawnArrow.transform.localPosition += spawnArrow.transform.up * 0.15f;
                        }
                        else
                        {
                            if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.50f ||
                                playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.60f ||
                                playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_3") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f)
                            {
                                if (spawnArrow != null)
                                    spawnArrow.GetComponent<Rigidbody>().velocity = spawnArrow.transform.forward * 50.0f;
                            }
                        }
                    }
                    if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        while (ActionQueue.Count > 0 && playerAnime.GetCurrentAnimatorStateInfo(0).IsName(ActionQueue[0]))
                            ActionQueue.RemoveAt(0);
                        freezeMove = false;
                        GetComponent<CameraAdjust>().freezeRotate = false;
                        arrowSpawned = false;
                        spawnArrow = null;
                        //player.GetComponent<Rigidbody>().constraints = originalConstraints;
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    ActionQueue.RemoveAt(0);
                    freezeMove = false;
                    GetComponent<CameraAdjust>().freezeRotate = false;
                }
            }
            else
            {
                playerAnime.PlayInFixedTime(animeName);
                weaponAnime.PlayInFixedTime("Idle");
            }

            if (!freezeMove)
            {
                if (!animeName.Equals("Idle"))
                {
                    float moveSpeed = (Input.GetKey(GetComponent<PlayerControl>().Run)) ? GetComponent<PlayerStats>().Speed * 1.5f : GetComponent<PlayerStats>().Speed;
                    GetComponent<PlayerMovement>().RotateCharacter();
                    GetComponent<PlayerMovement>().MoveCharacter(moveSpeed);
                }
            }
        }

        private void PerformSecondJump()
        {
            if (secondJump)
            {
                secondJump = false;
                GetComponent<PlayerStats>().Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore.View;
using Player.Config;

namespace Player.Action
{
    [RequireComponent(typeof(PlayerControl))]
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerAction : MonoBehaviour
    {
        [SerializeField] private AudioClip moveSE;
        [SerializeField] private AudioClip jumpSE;
        [SerializeField] private AudioClip landSE;
        [SerializeField] private AudioClip attackSE;

        private Animator playerAnime;
        private Animator weaponAnime;
        private List<string> ActionQueue;
        //private RigidbodyConstraints originalConstraints;
        private bool freezeMove;
        private bool secondJump;
        private bool landed;
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
            spawnArrow = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("Terrain"))
                landed = true;
        }

        public void QueueAttack1Action()
        {
            AnimatorStateInfo currentAnime = playerAnime.GetCurrentAnimatorStateInfo(0);
            if (ActionQueue.Count == 0)
                ActionQueue.Add("Attack_1");
            else if (currentAnime.IsName("Attack_1") && ActionQueue.Count == 1)
                ActionQueue.Add("Attack_2");
            else if (currentAnime.IsName("Attack_2") && ActionQueue.Count == 1)
                ActionQueue.Add("Attack_3");
        }

        public void QueueAttack2Action()
        {
            if (ActionQueue.Count == 0)
                ActionQueue.Add("Attack_High");
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
            if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                ActionQueue.Clear();
                return;
            }

            if (ActionQueue.Count != 0) // Jump, Attack
            {
                if (!playerAnime.GetCurrentAnimatorStateInfo(0).IsName(ActionQueue[0]))
                {
                    ResetActionTriggers();
                    playerAnime.SetTrigger(ActionQueue[0]);
                    switch (ActionQueue[0].ToString())
                    {
                        case "Jump": // Jump
                            GetComponent<PlayerStats>().Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
                            AudioSource.PlayClipAtPoint(jumpSE, transform.position);
                            landed = false;
                            secondJump = true;
                            break;
                        case "Land": // Land
                            AudioSource.PlayClipAtPoint(landSE, transform.position);
                            freezeMove = true;
                            break;
                        case "Attack_1":
                            if (!Input.GetKey(GetComponent<PlayerControl>().UnlockAttackDirection))
                            {
                                GetComponent<CameraAdjust>().AlignWithCamera();
                                GetComponent<PlayerMovement>().SetPlayerLocalAngle(0.0f);
                            }
                            weaponAnime.PlayInFixedTime("Attack_1");
                            freezeMove = true;
                            break;
                        case "Attack_2":
                            if (!Input.GetKey(GetComponent<PlayerControl>().UnlockAttackDirection))
                            {
                                GetComponent<CameraAdjust>().AlignWithCamera();
                                GetComponent<PlayerMovement>().SetPlayerLocalAngle(0.0f);
                            }
                            weaponAnime.PlayInFixedTime("Attack_2");
                            freezeMove = true;
                            break;
                        case "Attack_3":
                            if (!Input.GetKey(GetComponent<PlayerControl>().UnlockAttackDirection))
                            {
                                GetComponent<CameraAdjust>().AlignWithCamera();
                                GetComponent<PlayerMovement>().SetPlayerLocalAngle(0.0f);
                            }
                            weaponAnime.PlayInFixedTime("Attack_3");
                            freezeMove = true;
                            break;
                        case "Attack_High":
                            if (!Input.GetKey(GetComponent<PlayerControl>().UnlockAttackDirection))
                            {
                                GetComponent<CameraAdjust>().AlignWithCamera();
                                GetComponent<PlayerMovement>().SetPlayerLocalAngle(0.0f);
                            }
                            weaponAnime.PlayInFixedTime("Attack_1");
                            freezeMove = true;
                            break;
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Jump") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Glide"))
                {
                    if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Jump") && playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                        ActionQueue.RemoveAt(0);

                    if (landed)
                    {
                        while (!ActionQueue[0].ToString().Equals("Land"))
                            ActionQueue.RemoveAt(0);
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_1") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_2") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_3") || playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Attack_High"))
                {
                    //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)
                    {
                        playerAnime.ResetTrigger(ActionQueue[0]);
                        while (ActionQueue.Count > 0 && playerAnime.GetCurrentAnimatorStateInfo(0).IsName(ActionQueue[0]))
                            ActionQueue.RemoveAt(0);
                        //player.GetComponent<Rigidbody>().constraints = originalConstraints;
                    }
                }
                else if (playerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
                {
                    playerAnime.ResetTrigger(ActionQueue[0]);
                    ActionQueue.RemoveAt(0);
                    if (ActionQueue.Count == 0)
                        playerAnime.PlayInFixedTime(animeName);
                }
            }
            else // Perform Player Movement anime
            {
                if (ActionQueue.Count == 0 && playerAnime.GetCurrentAnimatorStateInfo(0).IsName(animeName))
                    freezeMove = false;

                if (animeName.Equals("Idle"))
                {
                    playerAnime.SetTrigger("Idle");
                    playerAnime.ResetTrigger("Walk");
                    playerAnime.ResetTrigger("Run");
                    GetComponent<PlayerStats>().RestoreStamina();
                }
                else if (animeName.Equals("Walk"))
                {
                    playerAnime.SetTrigger("Walk");
                    playerAnime.ResetTrigger("Idle");
                    playerAnime.ResetTrigger("Run");
                    GetComponent<PlayerStats>().RestoreStamina();
                }
                else if (animeName.Equals("Run"))
                {
                    if (GetComponent<PlayerStats>().ConsumeStamina())
                    {
                        playerAnime.SetTrigger("Run");
                        playerAnime.ResetTrigger("Walk");
                    }
                    else
                    {
                        playerAnime.SetTrigger("Walk");
                        playerAnime.ResetTrigger("Run");
                    }
                    playerAnime.ResetTrigger("Idle");
                }
                weaponAnime.PlayInFixedTime("Idle");
            }

            if (!freezeMove) // Move Player
            {
                if (!animeName.Equals("Idle"))
                {
                    float moveSpeed = (playerAnime.GetCurrentAnimatorStateInfo(0).IsName("Run")) ? GetComponent<PlayerStats>().Speed * 1.5f : GetComponent<PlayerStats>().Speed;
                    GetComponent<CameraAdjust>().AlignWithCamera();
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
                AudioSource.PlayClipAtPoint(jumpSE, transform.position);
            }
        }

        private void ResetActionTriggers()
        {
            playerAnime.ResetTrigger("Idle");
            playerAnime.ResetTrigger("Walk");
            playerAnime.ResetTrigger("Run");
            playerAnime.ResetTrigger("Jump");
            playerAnime.ResetTrigger("Glide");
            playerAnime.ResetTrigger("Land");
            playerAnime.ResetTrigger("Attack_1");
            playerAnime.ResetTrigger("Attack_2");
            playerAnime.ResetTrigger("Attack_3");
            playerAnime.ResetTrigger("Attack_High");
            playerAnime.ResetTrigger("Hit");
        }

        public void GetHit(int damage)
        {
            if (GetComponent<PlayerStats>().IsDead()) return;

            ResetActionTriggers();
            GetComponent<PlayerStats>().TakeDamage(damage);
            if (GetComponent<PlayerStats>().IsDead())
                playerAnime.SetTrigger("Die");
            else
                playerAnime.SetTrigger("Hit");
        }

        void SpawnArrow(string attackType) // Use in anime event
        {
            float aimAngle = (attackType == "AttackHigh") ? -30.0f : -2.5f;
            Quaternion angle = Quaternion.Euler(aimAngle, GetComponent<PlayerStats>().Player.transform.eulerAngles.y + transform.localEulerAngles.y, 0);
            spawnArrow = Instantiate(GetComponent<PlayerStats>().Arrow, GetComponent<PlayerStats>().ArrowSpawn.position, angle);
            spawnArrow.transform.localPosition -= spawnArrow.transform.forward * 0.5f;
            spawnArrow.GetComponent<Arrow>().SetArrowInfo(transform, 50.0f);
            spawnArrow.GetComponent<Arrow>().toggleArrowEffect(false);
        }

        void ProjectileArrow() // Use in animate event
        {
            if (spawnArrow != null)
            {
                spawnArrow.GetComponent<Rigidbody>().velocity = spawnArrow.transform.forward * 50.0f;
                AudioSource.PlayClipAtPoint(attackSE, GetComponent<PlayerStats>().ArrowSpawn.position);
                spawnArrow.GetComponent<Arrow>().toggleArrowEffect(true);
                spawnArrow = null;
            }
        }

        void MoveSound() // Use in animate event
        {
            AudioSource.PlayClipAtPoint(moveSE, transform.position);
        }
    }
}

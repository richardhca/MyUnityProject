using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameCore.View;
using Player.Config;

namespace Player.Action
{
    public class PlayerAction : MonoBehaviour
    {
        public Animator PlayerAnime;
        public Animator WeaponAnime;
        public ArrayList ActionQueue;
        //private RigidbodyConstraints originalConstraints;
        private bool freezeMove;
        private bool secondJump;
        private bool landed;
        private bool arrowSpawned;
        private GameObject spawnArrow;

        void Start()
        {
            PlayerAnime = GetComponent<Animator>();
            WeaponAnime = GetComponent<PlayerControl>().Weapon.GetComponent<Animator>();
            ActionQueue = new ArrayList();
            //originalConstraints = player.GetComponent<Rigidbody>().constraints;
            freezeMove = false;
            secondJump = false;
            landed = true;
            arrowSpawned = false;
            spawnArrow = null;
            PlayerAnime.PlayInFixedTime("103_idle");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name.Equals("Terrain"))
                landed = true;
        }

        public void PerformAction(string animeName)
        {
            if (ActionQueue.Count != 0) // Jump, Attack
            {
                string currentAnime = PlayerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                if (!currentAnime.Equals(ActionQueue[0]))
                {
                    PlayerAnime.PlayInFixedTime(ActionQueue[0].ToString());
                    switch (ActionQueue[0].ToString())
                    {
                        case "103_jump3": // Jump
                            GetComponent<PlayerControl>().Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
                            landed = false;
                            secondJump = true;
                            break;
                        case "103_landing2": // Land
                            freezeMove = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            break;
                        case "103_normalatk4":
                            WeaponAnime.PlayInFixedTime("10003_attack_2b");
                            freezeMove = true;
                            break;
                        case "10003_attack":
                            WeaponAnime.PlayInFixedTime("10003_attack_b");
                            freezeMove = true;
                            break;
                        case "103_fastshot":
                            WeaponAnime.PlayInFixedTime("10003_fastshot_b");
                            freezeMove = true;
                            break;
                        case "103_common":
                            freezeMove = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            break;
                    }
                }
                else if (currentAnime.Equals("103_jump3") || currentAnime.Equals("103_glide3"))
                {
                    if (currentAnime.Equals("103_jump3") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        ActionQueue.RemoveAt(0);

                    if (landed)
                    {
                        while (!ActionQueue[0].ToString().Equals("103_landing2"))
                            ActionQueue.RemoveAt(0);
                    }
                }
                else if (currentAnime.Equals("103_normalatk4") || currentAnime.Equals("10003_attack") || currentAnime.Equals("103_fastshot"))
                {
                    //player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    if (currentAnime.Equals("103_normalatk4") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f ||
                        currentAnime.Equals("10003_attack") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f ||
                        currentAnime.Equals("103_fastshot") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
                    {
                        if (!arrowSpawned)
                        {
                            arrowSpawned = true;
                            GetComponent<CameraAdjust>().freezeRotate = true;
                            Quaternion angle = Quaternion.Euler(0, GetComponent<PlayerControl>().Player.transform.eulerAngles.y + transform.localEulerAngles.y, 0);
                            spawnArrow = Instantiate(GetComponent<PlayerControl>().Arrow, GetComponent<PlayerControl>().ArrowSpawn.position, angle);
                            spawnArrow.transform.localPosition += spawnArrow.transform.forward * 1.2f;
                            if (currentAnime.Equals("103_normalatk4"))
                                spawnArrow.transform.localPosition += spawnArrow.transform.up * 0.1f;
                            else
                                spawnArrow.transform.localPosition += spawnArrow.transform.up * 0.15f;
                        }
                        else
                        {
                            if (currentAnime.Equals("103_normalatk4") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.50f ||
                                currentAnime.Equals("10003_attack") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.60f ||
                                currentAnime.Equals("103_fastshot") && PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f)
                                if (spawnArrow != null)
                                    spawnArrow.GetComponent<Rigidbody>().velocity = spawnArrow.transform.forward * 50.0f;
                        }
                    }
                    if (PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        while (ActionQueue.Count > 0 && currentAnime.Equals(ActionQueue[0]))
                            ActionQueue.RemoveAt(0);
                        freezeMove = false;
                        GetComponent<CameraAdjust>().freezeRotate = false;
                        arrowSpawned = false;
                        spawnArrow = null;
                        //player.GetComponent<Rigidbody>().constraints = originalConstraints;
                    }
                }
                else if (PlayerAnime.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    ActionQueue.RemoveAt(0);
                    freezeMove = false;
                    GetComponent<CameraAdjust>().freezeRotate = false;
                }
            }
            else
            {
                PlayerAnime.PlayInFixedTime(animeName);
                WeaponAnime.PlayInFixedTime("10003_idle");
            }

            if (!freezeMove)
            {
                if (!animeName.Equals("103_idle"))
                {
                    float moveSpeed = (Input.GetKey(GetComponent<PlayerControl>().Run)) ? GetComponent<PlayerStats>().AGI * 1.5f / 20.0f : GetComponent<PlayerStats>().AGI / 20.0f;
                    GetComponent<PlayerMovement>().RotateCharacter();
                    GetComponent<PlayerMovement>().MoveCharacter(moveSpeed);
                }
            }
        }

        public void PerformSecondJump()
        {
            if (secondJump)
            {
                secondJump = false;
                GetComponent<PlayerControl>().Player.GetComponent<Rigidbody>().velocity = new Vector3(0, 7.5f, 0);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

using GameCore.View;
using Player.Action;

namespace Player.Config
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] GameObject player;
        [SerializeField] GameObject weapon;
        [SerializeField] GameObject arrow;
        [SerializeField] Transform arrowSpawn;
        public KeyCode UP = KeyCode.Keypad5;
        public KeyCode DOWN = KeyCode.Keypad2;
        public KeyCode LEFT = KeyCode.Keypad1;
        public KeyCode RIGHT = KeyCode.Keypad3;
        public KeyCode CameraUP = KeyCode.Keypad7;
        public KeyCode CameraDOWN = KeyCode.Keypad9;
        public KeyCode CameraLEFT = KeyCode.Keypad4;
        public KeyCode CameraRIGHT = KeyCode.Keypad6;
        public KeyCode CameraZoomIn = KeyCode.Keypad8;
        public KeyCode CameraZoomOut = KeyCode.Keypad0;
        public KeyCode CameraReset = KeyCode.Z;
        public KeyCode Attack1 = KeyCode.F;
        public KeyCode Attack2 = KeyCode.G;
        public KeyCode Jump = KeyCode.D;
        public KeyCode Run = KeyCode.S;
        public KeyCode Restart = KeyCode.Escape;

        public GameObject Player => player;
        public GameObject Weapon => weapon;
        public GameObject Arrow => arrow;
        public Transform ArrowSpawn => arrowSpawn;

        void FixedUpdate()
        {
            if (Input.GetKey(UP) || Input.GetKey(DOWN) || Input.GetKey(LEFT) || Input.GetKey(RIGHT))
            {
                string action = (Input.GetKey(Run)) ? "103_run" : "10003_walk";
                GetComponent<PlayerAction>().PerformAction(action);
            }
            else
            {
                GetComponent<PlayerAction>().PerformAction("103_idle");
            }

            if (Input.GetKeyDown(CameraReset))
            {
                GetComponent<CameraAdjust>().ResetCamera();
            }

            if (Input.GetKey(CameraUP) || Input.GetKey(CameraDOWN) || Input.GetKey(CameraLEFT) || Input.GetKey(CameraRIGHT) || Input.GetKey(CameraZoomIn) || Input.GetKey(CameraZoomOut))
            {
                GetComponent<CameraAdjust>().AdjustCamera();
            }

            if (Input.GetKeyDown(Attack1))
            {
                string currentAnime = GetComponent<PlayerAction>().PlayerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                if (GetComponent<PlayerAction>().ActionQueue.Count == 0 || currentAnime.Equals("103_normalatk4") || currentAnime.Equals("10003_attack"))
                {
                    if (GetComponent<PlayerAction>().ActionQueue.Count == 0)
                        GetComponent<PlayerAction>().ActionQueue.Add("103_normalatk4");
                    else if (currentAnime.Equals("103_normalatk4") && GetComponent<PlayerAction>().ActionQueue.Count == 1)
                        GetComponent<PlayerAction>().ActionQueue.Add("10003_attack");
                    else if (currentAnime.Equals("10003_attack") && GetComponent<PlayerAction>().ActionQueue.Count == 1)
                        GetComponent<PlayerAction>().ActionQueue.Add("103_fastshot");
                }
            }
            else if (Input.GetKeyDown(Attack2))
            {
                if (GetComponent<PlayerAction>().ActionQueue.Count == 0)
                    GetComponent<PlayerAction>().ActionQueue.Add("103_common");
            }
            else if (Input.GetKeyDown(Jump))
            {
                if (GetComponent<PlayerAction>().ActionQueue.Count == 0)
                {
                    GetComponent<PlayerAction>().ActionQueue.Add("103_jump3");
                    GetComponent<PlayerAction>().ActionQueue.Add("103_glide3");
                    GetComponent<PlayerAction>().ActionQueue.Add("103_landing2");
                }
                else
                {
                    string currentAnime = GetComponent<PlayerAction>().PlayerAnime.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                    if (currentAnime.Equals("103_jump3") || currentAnime.Equals("103_glide3"))
                    {
                        GetComponent<PlayerAction>().PerformSecondJump();
                    }
                }
            }

            if (Input.GetKeyDown(Restart))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}

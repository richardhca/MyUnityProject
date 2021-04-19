using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameCore.View;
using Player.Action;

namespace Player.Config
{
    public class PlayerControl : MonoBehaviour
    {
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
        public KeyCode CameraAimRotate = KeyCode.A;
        public KeyCode CameraReset = KeyCode.Z;
        public KeyCode Attack1 = KeyCode.F;
        public KeyCode Attack2 = KeyCode.G;
        public KeyCode UnlockAttackDirection = KeyCode.Space;
        public KeyCode Jump = KeyCode.D;
        public KeyCode Run = KeyCode.S;
        public KeyCode Pause = KeyCode.Escape;

        public bool GamePaused = false;

        void FixedUpdate()
        {
            if (GetComponent<PlayerStats>().IsDead()) return;

            if (GamePaused) return;

            if (Input.GetKey(UP) || Input.GetKey(DOWN) || Input.GetKey(LEFT) || Input.GetKey(RIGHT))
            {
                string action = (Input.GetKey(Run)) ? "Run" : "Walk";
                GetComponent<PlayerAction>().PerformAction(action);
            }
            else
            {
                GetComponent<PlayerAction>().PerformAction("Idle");
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
                GetComponent<PlayerAction>().QueueAttack1Action();
            }
            else if (Input.GetKeyDown(Attack2))
            {
                GetComponent<PlayerAction>().QueueAttack2Action();
            }
            else if (Input.GetKeyDown(Jump))
            {
                GetComponent<PlayerAction>().QueueJumpAction();
            }

            /*if (Input.GetKeyDown(Pause))
            {
                SceneManager.LoadScene("TitleScene");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }*/
        }
    }
}

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
        [SerializeField] KeyConfigData keyConfig;

        private bool attackKeyOnPress = false;

        public bool GamePaused = false;

        void Update()
        {
            if (GetComponent<PlayerStats>().IsDead()) return;

            if (GamePaused) return;

            if (Input.GetKey(keyConfig.UP) || Input.GetKey(keyConfig.DOWN) || Input.GetKey(keyConfig.LEFT) || Input.GetKey(keyConfig.RIGHT))
            {
                string action = (Input.GetKey(keyConfig.Run)) ? "Run" : "Walk";
                GetComponent<PlayerAction>().PerformAction(action);
            }
            else
            {
                GetComponent<PlayerAction>().PerformAction("Idle");
            }

            if (Input.GetKeyDown(keyConfig.CameraReset))
            {
                GetComponent<CameraAdjust>().ResetCamera();
            }

            if (Input.GetKey(keyConfig.CameraUP) || Input.GetKey(keyConfig.CameraDOWN) || Input.GetKey(keyConfig.CameraLEFT) || Input.GetKey(keyConfig.CameraRIGHT) || Input.GetKey(keyConfig.CameraZoomIn) || Input.GetKey(keyConfig.CameraZoomOut))
            {
                GetComponent<CameraAdjust>().AdjustCamera();
            }

            if (Input.GetKeyDown(keyConfig.Attack1))
            {
                if (attackKeyOnPress) return;
                attackKeyOnPress = true;
                GetComponent<PlayerAction>().QueueAttack1Action();
            }
            else if (Input.GetKeyDown(keyConfig.Attack2))
            {
                GetComponent<PlayerAction>().QueueAttack2Action();
            }
            else if (Input.GetKeyDown(keyConfig.Jump))
            {
                GetComponent<PlayerAction>().QueueJumpAction();
            }

            if (Input.GetKeyUp(keyConfig.Attack1))
            {
                attackKeyOnPress = false;
            }
        }
    }
}

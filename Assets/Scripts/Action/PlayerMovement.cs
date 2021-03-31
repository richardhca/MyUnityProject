using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Config;

namespace Player.Action
{
    [RequireComponent(typeof(PlayerControl))]
    public class PlayerMovement : MonoBehaviour
    {
        private float targetAngle;
        private float rotateSpeed;

        void Start()
        {
            targetAngle = 0.0f;
            rotateSpeed = 0.0f;
        }

        private void FixedUpdate()
        {
            if (transform.localEulerAngles.y != targetAngle)
            {
                if (Mathf.Abs(targetAngle - transform.localEulerAngles.y) > rotateSpeed)
                {
                    float playerAngleNorm = (transform.localEulerAngles.y >= 0) ? transform.localEulerAngles.y : transform.localEulerAngles.y + 360.0f;
                    float leftDiff = (playerAngleNorm - targetAngle >= 0) ? playerAngleNorm - targetAngle : playerAngleNorm - targetAngle + 360.0f;
                    float rightDiff = (targetAngle - playerAngleNorm >= 0) ? targetAngle - playerAngleNorm : targetAngle - playerAngleNorm + 360.0f;
                    float increment = (leftDiff < rightDiff) ? -1 * rotateSpeed : rotateSpeed;
                    transform.localEulerAngles += new Vector3(0, increment, 0);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, targetAngle, 0);
                }
            }
        }

        public void MoveCharacter(float moveSpeed)
        {
            float radDeg = (GetComponent<PlayerStats>().Player.transform.eulerAngles.y + targetAngle) * Mathf.Deg2Rad;
            GetComponent<PlayerStats>().Player.transform.position += new Vector3(Mathf.Sin(radDeg) * moveSpeed, 0, Mathf.Cos(radDeg) * moveSpeed);
        }

        public void SetPlayerLocalAngle(float angle)
        {
            if (targetAngle != angle)
            {
                targetAngle = angle;
                rotateSpeed = Mathf.Abs(angle - transform.localEulerAngles.y) / 9.0f;
            }
        }

        public void RotateCharacter()
        {
            if (Input.GetKey(GetComponent<PlayerControl>().UP) && Input.GetKey(GetComponent<PlayerControl>().LEFT)) SetPlayerLocalAngle(315.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().UP) && Input.GetKey(GetComponent<PlayerControl>().RIGHT)) SetPlayerLocalAngle(45.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN) && Input.GetKey(GetComponent<PlayerControl>().LEFT)) SetPlayerLocalAngle(225.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN) && Input.GetKey(GetComponent<PlayerControl>().RIGHT)) SetPlayerLocalAngle(135.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().UP)) SetPlayerLocalAngle(0.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN)) SetPlayerLocalAngle(180.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().LEFT)) SetPlayerLocalAngle(270.0f);
            else if (Input.GetKey(GetComponent<PlayerControl>().RIGHT)) SetPlayerLocalAngle(90.0f);
        }

        public void ResetTargetAngle()
        {
            targetAngle = 0.0f;
        }
    }
}

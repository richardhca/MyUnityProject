using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Config;

namespace Player.Action
{
    public class PlayerMovement : MonoBehaviour
    {
        private float targetAngle;

        void Start()
        {
            targetAngle = 0.0f;
        }

        public void MoveCharacter(float moveSpeed)
        {
            float radDeg = (GetComponent<PlayerControl>().Player.transform.eulerAngles.y + targetAngle) * Mathf.Deg2Rad;
            GetComponent<PlayerControl>().Player.transform.position += new Vector3(Mathf.Sin(radDeg) * moveSpeed, 0, Mathf.Cos(radDeg) * moveSpeed);
        }

        public void RotateCharacter()
        {
            if (Input.GetKey(GetComponent<PlayerControl>().UP) && Input.GetKey(GetComponent<PlayerControl>().LEFT)) targetAngle = 315.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().UP) && Input.GetKey(GetComponent<PlayerControl>().RIGHT)) targetAngle = 45.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN) && Input.GetKey(GetComponent<PlayerControl>().LEFT)) targetAngle = 225.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN) && Input.GetKey(GetComponent<PlayerControl>().RIGHT)) targetAngle = 135.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().UP)) targetAngle = 0.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().DOWN)) targetAngle = 180.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().LEFT)) targetAngle = 270.0f;
            else if (Input.GetKey(GetComponent<PlayerControl>().RIGHT)) targetAngle = 90.0f;

            if (Mathf.Abs(targetAngle - transform.localEulerAngles.y) > 10.0f)
            {
                float playerAngleNorm = (transform.localEulerAngles.y >= 0) ? transform.localEulerAngles.y : transform.localEulerAngles.y + 360.0f;
                float leftDiff = (playerAngleNorm - targetAngle >= 0) ? playerAngleNorm - targetAngle : playerAngleNorm - targetAngle + 360.0f;
                float rightDiff = (targetAngle - playerAngleNorm >= 0) ? targetAngle - playerAngleNorm : targetAngle - playerAngleNorm + 360.0f;
                float increment = (leftDiff < rightDiff) ? -10.0f : 10.0f;
                transform.localEulerAngles += new Vector3(0, increment, 0);
            }
            else
            {
                transform.localEulerAngles = new Vector3(0, targetAngle, 0);
            }
        }
    }
}


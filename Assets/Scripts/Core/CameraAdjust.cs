using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Config;
using Player.Action;

namespace GameCore.View
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerControl))]
    public class CameraAdjust : MonoBehaviour
    {
        [SerializeField] KeyConfigData keyConfig;
        [SerializeField] GameObject cameraHolder;

        private Camera mainCamera;
        public bool freezeRotate;

        void Start()
        {
            mainCamera = Camera.main;
            freezeRotate = false;
        }

        void Update()
        {
            cameraHolder.transform.position = GetComponent<PlayerStats>().Player.transform.position;
        }

        public void AdjustCamera()
        {
            if (Input.GetKey(keyConfig.CameraLEFT) || Input.GetKey(keyConfig.CameraRIGHT))
            {
                if (!freezeRotate)
                {
                    float rotDir = Input.GetKey(keyConfig.CameraLEFT) ? -2.0f : 2.0f;
                    if (Input.GetKey(keyConfig.CameraAimRotate)) rotDir /= 4.0f;
                    cameraHolder.transform.Rotate(0, rotDir, 0);
                }
            }

            if (Input.GetKey(keyConfig.CameraUP) || Input.GetKey(keyConfig.CameraDOWN))
            {
                float cameraPosition = mainCamera.gameObject.transform.localPosition.y;
                if (Input.GetKey(keyConfig.CameraUP) && cameraPosition > 1.0f)
                {
                    mainCamera.gameObject.transform.localPosition -= new Vector3(0, 0.25f, 0);
                    mainCamera.gameObject.transform.localEulerAngles -= new Vector3(3.0f, 0, 0);
                }
                else if (Input.GetKey(keyConfig.CameraDOWN) && cameraPosition < 6.0f)
                {
                    mainCamera.gameObject.transform.localPosition += new Vector3(0, 0.25f, 0);
                    mainCamera.gameObject.transform.localEulerAngles += new Vector3(3.0f, 0, 0);
                }
            }

            if (Input.GetKey(keyConfig.CameraZoomIn) || Input.GetKey(keyConfig.CameraZoomOut))
            {
                float cameraZoom = mainCamera.gameObject.transform.localPosition.z;
                if (Input.GetKey(keyConfig.CameraZoomIn) && cameraZoom < -2.5f)
                    mainCamera.gameObject.transform.localPosition += new Vector3(0, 0, 0.1f);
                else if (Input.GetKey(keyConfig.CameraZoomOut) && cameraZoom > -7.5f)
                    mainCamera.gameObject.transform.localPosition -= new Vector3(0, 0, 0.1f);
            }
        }

        public void AlignWithCamera()
        {
            GetComponent<PlayerStats>().Player.transform.eulerAngles = cameraHolder.transform.eulerAngles;
        }

        public void ResetCamera()
        {
            GetComponent<PlayerStats>().Player.transform.eulerAngles += transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, 0, 0);
            GetComponent<PlayerMovement>().ResetTargetAngle();
            cameraHolder.transform.eulerAngles = GetComponent<PlayerStats>().Player.transform.eulerAngles;
        }
    }
}

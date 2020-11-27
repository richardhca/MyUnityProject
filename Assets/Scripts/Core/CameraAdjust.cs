using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Config;

namespace GameCore.View
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerControl))]
    public class CameraAdjust : MonoBehaviour
    {
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
            if (Input.GetKey(GetComponent<PlayerControl>().CameraLEFT) || Input.GetKey(GetComponent<PlayerControl>().CameraRIGHT))
            {
                if (!freezeRotate)
                {
                    float rotDir = Input.GetKey(GetComponent<PlayerControl>().CameraLEFT) ? -2.5f : 2.5f;
                    cameraHolder.transform.eulerAngles += new Vector3(0, rotDir, 0);
                }
            }

            if (Input.GetKey(GetComponent<PlayerControl>().CameraUP) || Input.GetKey(GetComponent<PlayerControl>().CameraDOWN))
            {
                float cameraPosition = mainCamera.gameObject.transform.localPosition.y;
                if (Input.GetKey(GetComponent<PlayerControl>().CameraUP) && cameraPosition > 1.0f)
                {
                    mainCamera.gameObject.transform.localPosition -= new Vector3(0, 0.25f, 0);
                    mainCamera.gameObject.transform.localEulerAngles -= new Vector3(3.0f, 0, 0);
                }
                else if (Input.GetKey(GetComponent<PlayerControl>().CameraDOWN) && cameraPosition < 6.0f)
                {
                    mainCamera.gameObject.transform.localPosition += new Vector3(0, 0.25f, 0);
                    mainCamera.gameObject.transform.localEulerAngles += new Vector3(3.0f, 0, 0);
                }
            }

            if (Input.GetKey(GetComponent<PlayerControl>().CameraZoomIn) || Input.GetKey(GetComponent<PlayerControl>().CameraZoomOut))
            {
                float cameraZoom = mainCamera.gameObject.transform.localPosition.z;
                if (Input.GetKey(GetComponent<PlayerControl>().CameraZoomIn) && cameraZoom < -2.5f)
                    mainCamera.gameObject.transform.localPosition += new Vector3(0, 0, 0.1f);
                else if (Input.GetKey(GetComponent<PlayerControl>().CameraZoomOut) && cameraZoom > -7.5f)
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
            cameraHolder.transform.eulerAngles = GetComponent<PlayerStats>().Player.transform.eulerAngles;
        }
    }
}

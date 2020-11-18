using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Player.Config;

namespace GameCore.View
{
    public class CameraAdjust : MonoBehaviour
    {
        private Camera mainCamera;
        public bool freezeRotate;

        void Start()
        {
            mainCamera = Camera.main;
            freezeRotate = false;
        }

        public void AdjustCamera()
        {
            if (Input.GetKey(GetComponent<PlayerControl>().CameraLEFT) || Input.GetKey(GetComponent<PlayerControl>().CameraRIGHT))
            {
                if (!freezeRotate)
                {
                    ResetCamera();
                    float rotDir = Input.GetKey(GetComponent<PlayerControl>().CameraLEFT) ? -2.5f : 2.5f;
                    GetComponent<PlayerControl>().Player.transform.eulerAngles += new Vector3(0, rotDir, 0);
                    transform.localEulerAngles = new Vector3(0, 0, 0);
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

        public void ResetCamera()
        {
            GetComponent<PlayerControl>().Player.transform.eulerAngles += transform.localEulerAngles;
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }
}

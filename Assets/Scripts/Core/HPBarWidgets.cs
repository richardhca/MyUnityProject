using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Widget
{
    public class HPBarWidgets : MonoBehaviour
    {
        public float OffsetToCamera = 0.0f;
        private Vector3 localStartPosition;

        void Start()
        {
            localStartPosition = transform.localPosition;
        }

        void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.localPosition = localStartPosition;
            transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
        }
    }
}

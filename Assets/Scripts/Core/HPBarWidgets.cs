﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Widget
{
    public class HPBarWidgets : MonoBehaviour
    {
        public bool BillboardX = true;
        public bool BillboardY = true;
        public bool BillboardZ = true;
        public float OffsetToCamera;
        protected Vector3 localStartPosition;

        void Start()
        {
            localStartPosition = transform.localPosition;
        }

        void Update()
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
            if (!BillboardX || !BillboardY || !BillboardZ)
                transform.rotation = Quaternion.Euler(BillboardX ? transform.rotation.eulerAngles.x : 0f, BillboardY ? transform.rotation.eulerAngles.y : 0f, BillboardZ ? transform.rotation.eulerAngles.z : 0f);
            transform.localPosition = localStartPosition;
            transform.position = transform.position + transform.rotation * Vector3.forward * OffsetToCamera;
        }
    }
}

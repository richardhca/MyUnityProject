using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, initialPosition) > 100.0f)
            GetComponent<Rigidbody>().useGravity = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monster.Action;

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
        if (!other.name.Equals("Terrain"))
        {
            if (other.transform.parent != null && other.transform.parent.name.Equals("Enemies"))
                other.GetComponent<MonsterAction>().GetHit(40); // Currently magic number for test, change value to player's attack later
        }
        Destroy(gameObject);
    }
}

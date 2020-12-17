using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monster.Config;
using Player.Action;

public class MonsterWeapon : MonoBehaviour
{
    /* This file is temporary unused because the collision detect on high velocity issue has not yet been solved */

    [SerializeField] GameObject Owner;

    /*private Vector3 currPos;
    private Vector3 prevPos;

    void Start()
    {
        currPos = transform.position;
    }

    void FixedUpdate()
    {
        currPos = transform.position;
        float length = (currPos - prevPos).magnitude;
        Vector3 direction = transform.forward;
        bool isCollider = Physics.Raycast(currPos, direction, out RaycastHit hitinfo, length);
        Debug.DrawRay(direction, direction*2, Color.yellow);
        if (isCollider)
        {
            Debug.Log("Hit " + hitinfo.transform.name);
        }
    }*/

    /*private void OnTriggerEnter(Collider other)
    {
        if (Owner.GetComponent<MonsterStats>().IsDead()) return;
        
        if (other.transform.parent != null && other.transform.parent.name.Equals("Player"))
        {
            other.gameObject.GetComponent<PlayerAction>().GetHit(Owner.GetComponent<MonsterStats>().Attack);
        }
    }*/
}

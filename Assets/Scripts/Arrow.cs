using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monster.Action;
using Player.Config;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private ParticleSystem projectileEffect;
    [SerializeField] private List<ParticleSystem> particleEffects;

    private Transform owner;
    private Vector3 initialPosition;
    private float dx;
    private bool removingObject;

    void Start()
    {
        initialPosition = transform.position;
        removingObject = false;
    }

    void LateUpdate()
    {
        if (removingObject) return;

        if (!GetComponent<Rigidbody>().useGravity)
        {
            //float angle = (transform.eulerAngles.x > 180.0f) ? transform.eulerAngles.x - 360.0f : transform.eulerAngles.x;
            //if (Mathf.Abs(angle) * Vector3.Distance(transform.position, initialPosition) >= 30.0f)
            if (Vector3.Distance(transform.position, initialPosition) >= 3.0f)
                GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            float radDeg = (GetComponent<Rigidbody>().velocity.y / dx); // dy / dx
            //Debug.Log(-1 * Mathf.Atan(radDeg) / Mathf.Deg2Rad);
            float angleDiff = -1 * Mathf.Atan(radDeg) / Mathf.Deg2Rad - transform.eulerAngles.x;
            transform.Rotate(angleDiff, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject == GameObject.FindWithTag("Player")) return;
            if (other.transform.parent.name.Equals("Enemies"))
            {
                int damage = (owner != null) ? owner.GetComponent<PlayerStats>().Attack : 20;
                other.GetComponent<MonsterAction>().GetHit(damage);
            }
        }
        removingObject = true;
        StartCoroutine(destroyArrow());
    }

    public void SetArrowInfo(Transform character, float velocity)
    {
        owner = character;
        float initialAngle = (transform.eulerAngles.x <= 0) ? -1 * transform.eulerAngles.x : -1 * (transform.eulerAngles.x - 360);
        dx = Mathf.Cos(initialAngle*Mathf.Deg2Rad) * velocity;
    }

    public void toggleArrowEffect(bool toggle)
    {
        if (toggle)
        {
            projectileEffect.Play();
            foreach (ParticleSystem particleEffect in particleEffects)
                particleEffect.Play();
        }
        else
        {
            projectileEffect.Stop();
            foreach (ParticleSystem particleEffect in particleEffects)
                particleEffect.Stop();
        }
    }

    IEnumerator destroyArrow()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        arrowObject.SetActive(false);
        projectileEffect.gameObject.SetActive(false);
        foreach (ParticleSystem particleEffect in particleEffects)
        {
            var emission = particleEffect.emission;
            emission.enabled = false;
        }
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds to let all the particles end their lifespans
        Destroy(gameObject);
    }
}

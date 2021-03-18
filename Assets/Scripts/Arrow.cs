using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Monster.Action;

public class Arrow : MonoBehaviour
{
    [SerializeField] private GameObject arrowObject;
    [SerializeField] private ParticleSystem projectileEffect;
    [SerializeField] private List<ParticleSystem> particleEffects;

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
        if (other.transform.parent != null)
        {
            if (other.transform.parent.gameObject == GameObject.FindWithTag("Player")) return;
            if (other.transform.parent.name.Equals("Enemies"))
                other.GetComponent<MonsterAction>().GetHit(40); // Currently magic number for test, change value to player's attack later
        }
        /*if (!other.name.Equals("Terrain"))
        {
            if (other.transform.parent != null && other.transform.parent.name.Equals("Enemies"))
                other.GetComponent<MonsterAction>().GetHit(40); // Currently magic number for test, change value to player's attack later
        }*/
        StartCoroutine(destroyArrow());
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

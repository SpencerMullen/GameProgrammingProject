using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DarkTonic.MasterAudio;

public class GrenadeBomb : MonoBehaviour
{
    public GameObject explosionEffect;
    public float radius= 5;
    public float force = 700f;
    public float delay = 3f;

    float countdown;
    bool hasExploded = false;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            MasterAudio.PlaySound3DAtVector3("Grenade_explosion", transform.position);
            hasExploded = true;
        }
    }
    void Explode()
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerMask.GetMask("Enemy"));
        foreach(Collider obj in colliders)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
                rb.GetComponent<EnemyHealth>().TakeDamage(100);
            }
        }
        Destroy(explosionEffect, 1f);
        Destroy(gameObject, 1.5f);
    }
}

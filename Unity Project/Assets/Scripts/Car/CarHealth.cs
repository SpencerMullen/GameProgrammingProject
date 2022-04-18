using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarHealth : EnemyHealth
{
    //public GameObject carExplosion;
    //public AudioClip explosionFXS;
    
    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        currentHealth = startingHealth;
        slider.value = CalculateHealth();
    }

    //public void TakeDamage(float damage)
    //{
    //    this.currentHealth -= damage;
    //}
    public override void TakeDamage(float damage)
    {
        // Debug.Log("HEALTH: " + currentHealth + "/" + startingHealth + " DAMAGE: " + damage);
        this.currentHealth -= damage;
        slider.value = CalculateHealth();
        if (currentHealth <= 0)
        {
            alive = false;
            slider.value = 0;
            DestroyEnemy();
        }
    }

    protected override void DestroyEnemy()
    {
        GameObject blood = Instantiate(enemyKilled, transform.position + new Vector3(0f, 3f, 0), transform.rotation);
        ExplosionDamage(transform.position, 6);
        AudioSource.PlayClipAtPoint(enemyFXS, transform.position);
        MoneyManager.Instance.addMoney(0);
        Destroy(gameObject);
    }

    // Initiate enemy destroy sequence

    void ExplosionDamage(Vector3 center, float radius)
    {
        Collider[] hitColliders= Physics.OverlapSphere(center, radius , LayerMask.GetMask("Enemy"));
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enem = GetComponent<EnemyHealth>();
            if (enem !=  null)
            {
                enem.TakeDamage(15);
            }
        }
    }

    float CalculateHealth()
    {
        return currentHealth / startingHealth;
    }
}

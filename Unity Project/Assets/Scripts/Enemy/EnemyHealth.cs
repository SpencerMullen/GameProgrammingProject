using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject enemyKilled;
    public AudioClip enemyFXS;
    public float startingHealth;
    protected float currentHealth;
    public Slider slider;
    public bool alive = true;
    public int value;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        currentHealth = startingHealth;
        slider.value = CalculateHealth();
    }

    // Take damage
    public virtual void TakeDamage(float damage){
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

    // Initiate enemy destroy sequence
    protected virtual void DestroyEnemy() {
        
        GameObject blood = Instantiate(enemyKilled, transform.position + new Vector3(0f, 3f, 0), transform.rotation);
        blood.transform.Rotate(0f, 90f, 0f);
        AudioSource.PlayClipAtPoint(enemyFXS, transform.position);
        MoneyManager.Instance.addMoney(value);
        Destroy(gameObject);
        
    }

    public float getCurrentHealth() {
        return currentHealth;
    }

    float CalculateHealth() {
        return currentHealth / startingHealth;
    }
}

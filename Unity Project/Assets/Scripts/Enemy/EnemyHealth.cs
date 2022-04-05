using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public GameObject enemyKilled;
    public AudioClip enemyFXS;
    public float startingHealth;
    float currentHealth;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        slider.value = CalculateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        if(currentHealth <= 0) {
            DestroyEnemy();
            slider.value = 0;
        }
    }

    // Take damage
    public void TakeDamage(float damage){
        Debug.Log("HEALTH: " + currentHealth + "/" + startingHealth + " DAMAGE: " + damage);
        this.currentHealth -= damage;
    }

    // Initiate enemy destroy sequence
    void DestroyEnemy() {
        AudioSource.PlayClipAtPoint(enemyFXS, transform.position);
        Instantiate(enemyKilled, transform.position + new Vector3(0f, 3f, 0), transform.rotation);
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    float CalculateHealth() {
        return currentHealth / startingHealth;
    }
}

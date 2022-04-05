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
    public GameObject moneyManager;
    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        currentHealth = startingHealth;
        slider.value = CalculateHealth();
        if(moneyManager == null) {
            moneyManager = GameObject.FindGameObjectWithTag("Lab");
        }
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        if(currentHealth <= 0) { 
            DestroyEnemy();
            alive = false;
            slider.value = 0;
        }
    }

    // Take damage
    public void TakeDamage(float damage){
        // Debug.Log("HEALTH: " + currentHealth + "/" + startingHealth + " DAMAGE: " + damage);
        this.currentHealth -= damage;
    }

    // Initiate enemy destroy sequence
    void DestroyEnemy() {
        if(alive == true) {
            GameObject blood = Instantiate(enemyKilled, transform.position + new Vector3(0f, 3f, 0), transform.rotation);
            blood.transform.Rotate(0f, 90f, 0f);
            AudioSource.PlayClipAtPoint(enemyFXS, transform.position);
            moneyManager.GetComponent<MoneyManager>().addMoney(10);
            Destroy(gameObject, 1f);
        }
    }

    float CalculateHealth() {
        return currentHealth / startingHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LabHealth : MonoBehaviour
{
    public float startingHealth = 1000f;
    float currentHealth;
    public Slider slider;
    private GameObject losetxt;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = CalculateHealth();
        currentHealth = startingHealth;
        losetxt = GameObject.FindGameObjectWithTag("LoseTxt");
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
        if(currentHealth <= 0) {
            LoseGame();
            slider.value = 0;
        }
    }

    // Take damage
    public void TakeDamage(float damage){
        // Debug.Log("HEALTH: " + currentHealth + "/" + startingHealth + " DAMAGE: " + damage);
        this.currentHealth -= damage;
    }

    void LoseGame() {
        losetxt.SetActive(true);
        Invoke("RestartScene", 3f);
    }

    void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    float CalculateHealth() {
        return currentHealth / startingHealth;
    }
}

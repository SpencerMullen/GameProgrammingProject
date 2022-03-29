using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    int countdown = 120;
    public Text timerUI;
    GameObject[] enemies;
    public AudioClip win;
    public AudioClip lose;
    // Start is called before the first frame update
    void Start()
    {
        countDownTimer();
        
        
    }

    // Update is called once per frame
    void countDownTimer() {  
    if (countdown > 0 ) {  
      TimeSpan spanTime = TimeSpan.FromSeconds(countdown);  
      timerUI.text = "Timer : " + spanTime.Minutes + " : " + spanTime.Seconds;  
      countdown--;  
      Invoke("countDownTimer", 1.0f);  
    } 
    else {  
      timerUI.text = "GameOver!";
      AudioSource.PlayClipAtPoint(lose, Camera.main.transform.position);
      Invoke("loadGame", 2.0f);  
    }  
    }  
    void Update() {
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      if(enemies.Length == 0){
        timerUI.text = "You Win!";
        AudioSource.PlayClipAtPoint(win, Camera.main.transform.position);
        Invoke("loadGame", 1.0f);  
    }
  }
    void loadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }  
}


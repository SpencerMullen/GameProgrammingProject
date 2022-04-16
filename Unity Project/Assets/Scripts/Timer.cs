using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    int enemyLeft;
    public TextMeshProUGUI enemyText;
    GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame  
    void Update() {
      enemies = GameObject.FindGameObjectsWithTag("Enemy");
      enemyLeft = enemies.Length;
      enemyText.text = "enemies left: "+ enemyLeft;
  }
     
}


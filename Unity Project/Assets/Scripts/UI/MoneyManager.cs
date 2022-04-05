using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public float startingMoney = 0f;
    float currentMoney;
    public TextMeshProUGUI moneyText;
    public GameObject turret1;

    // Start is called before the first frame update
    void Start()
    {
        currentMoney = startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Radiation Collected: " + currentMoney;

        if(Input.GetKeyDown("t")) {
            createTurret(GameObject.FindGameObjectWithTag("Player").transform);
        }
    }

    public void addMoney(float value) {
        this.currentMoney += value;
    }

    public void createTurret(Transform location) {
        if(currentMoney >= 100) {
            currentMoney -= 100;
            Instantiate(turret1, location.position, Quaternion.identity);
        }
    }
}

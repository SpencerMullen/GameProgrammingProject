using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public float startingMoney = 0f;
    float currentMoney;
    public TextMeshProUGUI moneyText;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        currentMoney = startingMoney;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "Radiation Collected: " + currentMoney;
    }

    public void addMoney(float value) {
        this.currentMoney += value;
    }

    // return true if have enough money
    public bool UseMoney(float value)
    {
        Debug.Log($"{currentMoney} - {value}");
        if (currentMoney - value >= 0)
        {
            currentMoney -= value;
            return true;
        }
        return currentMoney - value >= 0;
    }
    //public void createTurret(Transform location) {
    //    if(currentMoney >= 100) {
    //        currentMoney -= 100;
    //        Instantiate(turret1, location.position, Quaternion.identity);
    //    }
    //}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveText : MonoBehaviour
{
    public GameObject spawnManager;
    public TextMeshProUGUI waveText;

    // Start is called before the first frame update
    void Start()
    {
        if(spawnManager == null) {
            spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = "Wave: " + spawnManager.GetComponent<SpawnManager>().getWave();
    }
}

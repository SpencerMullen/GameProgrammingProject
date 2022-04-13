using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSky : MonoBehaviour
{
    public static ControlSky Instance;

    public Material dayMat;
    public Material nightMat;
    public GameObject dayLight;
    public GameObject nightLight;

    public Color dayFog;
    public Color nightFog;

    public bool isNight;

    private void Start()
    {
        Instance = this;
        if (isNight)
            Night();
        else 
            Day();
    }
    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 0.5f);
    }

    public void Day()
    {
        isNight = false;
        RenderSettings.skybox = dayMat;
        dayLight.SetActive(true);
        RenderSettings.fogColor = dayFog;
        nightLight.SetActive(false);
    }

    public void Night()
    {
        isNight = true;
        RenderSettings.skybox = nightMat;
        dayLight.SetActive(false);
        RenderSettings.fogColor = nightFog;
        nightLight.SetActive(true);
    }

    private void OnGUI()
    {
        //if (GUI.Button(new Rect(5,5,80,20), "Day")) {
        //    RenderSettings.skybox = dayMat;
        //    dayLight.SetActive(true);
        //    RenderSettings.fogColor = dayFog;
        //    nightLight.SetActive(false);
        //}

        //if (GUI.Button(new Rect(5, 35, 80, 20), "Night"))
        //{
        //    RenderSettings.skybox = nightMat;
        //    dayLight.SetActive(false);
        //    RenderSettings.fogColor = nightFog;
        //    nightLight.SetActive(true);
        //}
    }
}

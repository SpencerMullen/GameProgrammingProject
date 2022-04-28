using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeText = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text sensitivityText = null;
    [SerializeField] private Slider sensSlider = null;
    private float origSense = 100f;

    [Header("Levels To Load")]
    public string startGame;

    public void StartGame()
    {
        SceneManager.LoadScene(startGame);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeText.text = volume.ToString("0.0");
    }

    public void SetSens(float sense)
    {
        origSense = sense;
        sensitivityText.text = sense.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("mastervolume", AudioListener.volume);
    }

    public void SenseApply()
    {
        PlayerPrefs.SetFloat("masterSens", origSense);
        MouseLook.mouseSensitivity = origSense;
    }
}

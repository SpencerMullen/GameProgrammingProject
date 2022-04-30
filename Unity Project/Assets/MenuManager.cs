using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DarkTonic.MasterAudio;


public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeText = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Sensitivity Setting")]
    [SerializeField] private TMP_Text sensitivityText = null;
    [SerializeField] private Slider sensSlider = null;
    private float origSense = 100f;

    public void ShowMenu()
    {
        settingsMenu.SetActive(true);
    }

    public void HideMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        ShootProjectile.canShoot = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void SetVolume(float volume)
    {
        //AudioListener.volume = volume;
        PersistentAudioSettings.MixerVolume = volume / 100;
        volumeText.text = volume.ToString("0.0");
    }

    public void SetSens(float sense)
    {
        origSense = sense;
        sensitivityText.text = sense.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("mastervolume", PersistentAudioSettings.MixerVolume.Value);
    }

    public void SenseApply()
    {
        PlayerPrefs.SetFloat("masterSens", origSense);
        MouseLook.mouseSensitivity = origSense;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsMenu.activeInHierarchy)
            {
                settingsMenu.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;
                ShootProjectile.canShoot = true;
            } else
            {
                settingsMenu.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
                ShootProjectile.canShoot = false;
            }
        }
    }
}

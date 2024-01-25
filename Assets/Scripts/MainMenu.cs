using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource mainMenuAudio;

    // Start is called before the first frame update
    void Start()
    {
        if (Utils.MasterVolumeSet)
        {
            mainMenuAudio.volume = Utils.MasterVolume;
            volumeSlider.value = Utils.MasterVolume;
        }
        else
        {
            Utils.MasterVolume = .5f;
            Utils.MasterVolumeSet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }

    public void OnVolumeChange()
    {
        Utils.MasterVolume = volumeSlider.value;
        mainMenuAudio.volume = Utils.MasterVolume;
    }
}

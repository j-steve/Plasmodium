using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioSource mainMenuAudio;

    [SerializeField] Image howToPlayImage;
    [SerializeField] GameObject btnPrevious;
    [SerializeField] GameObject btnNext;
    [SerializeField] List<Sprite> screenShots;
    [SerializeField] TextMeshProUGUI txtDescription;
    [SerializeField] List<string> descriptions;
 
    int currentScreenshot;

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

        if(screenShots.Count > 0)
        {
            currentScreenshot = 0;
            howToPlayImage.sprite = screenShots[currentScreenshot];
            txtDescription.text = descriptions[currentScreenshot];
            btnPrevious.SetActive(false);
        }
        else
        {
            btnNext.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartNewGame()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnVolumeChange()
    {
        Utils.MasterVolume = volumeSlider.value;
        mainMenuAudio.volume = Utils.MasterVolume;
    }

    public void OnNext()
    {
        howToPlayImage.sprite = screenShots[++currentScreenshot];
        txtDescription.text = descriptions[currentScreenshot];

        if (currentScreenshot == 1)
            btnPrevious.SetActive(true);

        if (currentScreenshot == screenShots.Count - 1)
            btnNext.SetActive(false);
    }

    public void OnPrevious()
    {
        howToPlayImage.sprite = screenShots[--currentScreenshot];
        txtDescription.text = descriptions[currentScreenshot];

        if (currentScreenshot == screenShots.Count - 2)
            btnNext.SetActive(true);

        if (currentScreenshot == 0)
            btnPrevious.SetActive(false);
    }
}

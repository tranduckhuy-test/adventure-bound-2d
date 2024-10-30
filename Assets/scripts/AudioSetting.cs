using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    private void Start()
    {
        LoadVolume();
        ApplyVolumeSettings(); 
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volume);

        SFXManager.instance.MusicVolume(volume); 
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", volume);

        SFXManager.instance.SFXVolume(volume); 
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.HasKey("musicVolume") ? PlayerPrefs.GetFloat("musicVolume") : 1f;
        sfxSlider.value = PlayerPrefs.HasKey("sfxVolume") ? PlayerPrefs.GetFloat("sfxVolume") : 1f;
    }

    public void ApplyVolumeSettings()
    {
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveVolumeSettings();
        }
    }
}

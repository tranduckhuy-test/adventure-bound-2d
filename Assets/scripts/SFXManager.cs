using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource musicSource, sfxSource;
    public AudioClip background;
    public AudioClip slashSound, hitSound, deathSound, playerDeathSound, playerHitSound, clickSound, doorAppearSound, stairsDown, winGame, goblinDie, buffaloDie;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicSource.volume = 1f; 
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            sfxSource.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            sfxSource.volume = 1f; 
        }

        musicSource.clip = background;
        musicSource.Play();
    }

    public void SlashSound()
    {
        if (GameManager.instance.GetGamePauseStatus() == false)
        {
            sfxSource.PlayOneShot(slashSound);
        }
    }

    public void HitSound() => sfxSource.PlayOneShot(hitSound);
    public void DeathSound() => sfxSource.PlayOneShot(deathSound);
    public void PlayerDeath() => sfxSource.PlayOneShot(playerDeathSound);
    public void PlayerHit() => sfxSource.PlayOneShot(playerHitSound);
    public void DoorAppear() => sfxSource.PlayOneShot(doorAppearSound);
    public void ClickSound() => sfxSource.PlayOneShot(clickSound);
    public void StairsDown() => sfxSource.PlayOneShot(stairsDown);
    public void WinGame() => sfxSource.PlayOneShot(winGame);
    public void GoblinDeath() => sfxSource.PlayOneShot(goblinDie);
    public void BuffaloDeath() => sfxSource.PlayOneShot(buffaloDie);

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);  
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("sfxVolume", volume); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Linq;
   
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [Header("audio mixers")]
    [SerializeField] private AudioMixer overallMixer;

    [Header("music & ambience")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] allMusicSounds;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioSource ambience;
    [SerializeField] private AudioClip[] allAmbienceSounds;
    private AudioClip[] randomSongOrderList;
    private int randomSongIndex = 0;

    [Header("hazards")]
    [SerializeField] private AudioSource poisonHazardSource;
    [SerializeField] private AudioSource lavaHazardSource;
    [SerializeField] private AudioSource hazardWarningSource;
   

    [Header("WeaponSounds")]
    [SerializeField] private AudioSource pickUpWeaponSource;
    [SerializeField] private AudioSource electricShotgunSource;
    [SerializeField] private AudioClip[] shotgunSounds;
    [SerializeField] private AudioSource assaultRifleSource;
    [SerializeField] private AudioSource gunSource;
    [SerializeField] private AudioSource shitGunSource;
    [SerializeField] private AudioSource lobbyGunSource;
    [SerializeField] private AudioSource gwynSource;
    [SerializeField] private AudioSource grenadeSource;
    [SerializeField] private AudioSource xBombSource;
    [SerializeField] private AudioSource railGunSource;
    [SerializeField] private AudioSource grenadeLaucherSource;


    [Header("foliage")]
    [SerializeField] private AudioSource doorOpenSource;
    [SerializeField] private AudioSource doorCloseSource;
    [SerializeField] private AudioSource glassShatterSource;
	[SerializeField] private AudioSource portalSource;
    [SerializeField] private AudioSource bellRingSource;
	[SerializeField] private AudioSource trampolineSource; //ligger för närvarande på trampolinprefab. används ej av soundManager
    [SerializeField] private AudioSource startGameBellSource;
    [SerializeField] private AudioSource fireWorkSource;
    [SerializeField] private AudioClip[] allFireworkSounds;

    [Header("UI sounds")]
    [SerializeField] private AudioSource firstPlayerSpawnedInSource;
    [SerializeField] private AudioSource howerMenuSource;
    [SerializeField] private AudioSource pressMenuSource;
    [SerializeField] private AudioSource pressMenu2Source;
    [SerializeField] private AudioSource pressMenu3Source;
    [SerializeField] private AudioSource pressMenu4Source;
    [SerializeField] private AudioSource pressBackButtonSource;
    [SerializeField] private AudioSource pressingBattleButtonSource;

    [Header("End scene sounds")]
    [SerializeField] private AudioSource applauseSource;
    [SerializeField] private AudioSource drumsSource;
    //fyrverkerier ligger under foliage


    private float volLowRan = 0.3f;
    private float volHighRan = 1.0f;
    private float lowPitchRan = 0.3f;
    private float highPitchRan = 1.0f;
    private float minHumDelay = 25.0f;
    private float maxHumDelay = 200.0f;


    private float highestMasterVolume = 0.5f;
    private float highestMusicVolume = 1f;
    private float highestEffectVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        createRandomSongOrder();
        //DontDestroyOnLoad(gameObject);
    }
    
    public void SetHighestMusicVolume(float hmv)
    {
        highestMusicVolume = hmv;
    }
    public void SetHighestEffectVolume(float hmv)
    {
        highestEffectVolume = hmv;
    }
    public void FadeInMusic()
    {
        RandomClipPlayer(randomSongOrderList, musicSource);
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 2f, highestMusicVolume));
    }
    public void FadeOutMusic()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 1f, 0f));
    }
    public void FadeInEndSceneSounds()
    {
        applauseSource.Play();
        drumsSource.Play();
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "EndSceneGroup", 1f, highestEffectVolume));
    }
    public void FadeOutEndSceneSounds()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "EndSceneGroup", 2f, 0f));
    }
    public void FadeInBellSounds()
    {

        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "BellSounds", 1f, 0.4f));
    }
    
    public void FadeOutBellSounds()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "BellSounds", 3f, 0f));
    }
    //Weapons
    public void PickUpWeaponSound()
    {
        pickUpWeaponSource.pitch = Random.Range(0.94f, highPitchRan);
        pickUpWeaponSource.Play();
    }
    public void ElectricShotgunSound()
    {
        electricShotgunSource.pitch = Random.Range(0.98f, highPitchRan);
        electricShotgunSource.clip = shotgunSounds[0];
        electricShotgunSource.Play();
    }

    public void ElectricShotgunLastBulletSound()
    {
        electricShotgunSource.pitch = Random.Range(0.98f, highPitchRan);
        electricShotgunSource.clip = shotgunSounds[1];
        electricShotgunSource.Play();
    }
    public void AssaultRifleSound()
    {
        assaultRifleSource.pitch = Random.Range(0.98f, highPitchRan);
        assaultRifleSource.Play();
    }
    public void RevolverSound()
    {
        gunSource.pitch = Random.Range(0.98f, highPitchRan);
        gunSource.Play();
    }
    public void ShitGunSound()
    {
        shitGunSource.pitch = Random.Range(0.98f, highPitchRan);
        shitGunSource.Play();
    }
    public void LobbyGunSound()
    {
        lobbyGunSource.pitch = Random.Range(0.98f, highPitchRan);
        lobbyGunSource.Play();
    }
    public void GwynSound()
    {
        gwynSource.pitch = Random.Range(0.98f, highPitchRan);
        gwynSource.Play();
    }
    public void GrenadeSound()
    {
        grenadeSource.pitch = Random.Range(0.98f, highPitchRan);
        grenadeSource.Play();
    }
    public void XBombSound()
    {
        xBombSource.pitch = Random.Range(0.98f, highPitchRan);
        xBombSource.Play();
    }
    public void RailGunSound()
    {
        railGunSource.pitch = Random.Range(0.98f, highPitchRan);
        railGunSource.Play();
    }
    public void GrenadeLauncherSound()
    {
        grenadeLaucherSource.pitch = Random.Range(0.98f, highPitchRan);
        grenadeLaucherSource.Play();
    }
	
	public void PlayGunSound(string weaponName)
    {
        switch (weaponName)
        {
            case "Shotgun":
                ElectricShotgunSound();
                break;
           
            case "AR":
                AssaultRifleSound();
                break;

            case "Revolver":
                RevolverSound();
                break;

            case "ShitGun":
                ShitGunSound();
                break;
        
            case "Lobby Gun":
                LobbyGunSound();
                break;

            case "GwynBolt":
                GwynSound();
                break;

            case "Launcher":
                GrenadeSound();
                break;

            case "Xnade":
                XBombSound();
                break;

            case "RailGun":
                RailGunSound();
                break;

            case "GrenadeLauncher":
                GrenadeLauncherSound();
                break;
        }
    }
	
    //Hazards
    public void FadeInLavaHazard()
    {
        poisonHazardSource.Stop();
        lavaHazardSource.Play(); 
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 5f, highestEffectVolume));
    }
    public void FadeOutLavaHazard(float timeToFadeOut)
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", timeToFadeOut, 0f));
    }
    public void FadeInPoisionHazard() { 
        lavaHazardSource.Stop();
        poisonHazardSource.Play();
        
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 1, highestEffectVolume));
    }
    
    public void FadeOutHazard()
    {
        
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 2, 0f));
    }
    //UI & menues
    public void HazardWarningSound()
    {
        hazardWarningSource.Play();     
    }
    public void PlayerOnPlay_Sound()
    {
        bellRingSource.pitch = Random.Range(0.8f, highPitchRan);//kanske ska ha samma pitch hela tiden?
        bellRingSource.Play();
    }
    public void AllPlayersOnPlay_Sound()
    {
        startGameBellSource.Play();
    }
    public void HowerUiSound()
    {
        howerMenuSource.pitch = Random.Range(0.93f, highPitchRan);
        howerMenuSource.Play();
    }
    public void FirstPlayerSpawnedInSound()
    {
        
        firstPlayerSpawnedInSource.Play();
    }
    public void PressUiSound()
    {
        howerMenuSource.pitch = Random.Range(0.96f, highPitchRan);
        pressMenuSource.Play();
    }
    public void PressUiTwoSound()
    {
        howerMenuSource.pitch = Random.Range(0.96f, highPitchRan);
        pressMenu2Source.Play();
    }
    public void PressUiThreeSound()
    {
        howerMenuSource.pitch = Random.Range(0.96f, highPitchRan);
        pressMenu3Source.Play();
    }
    public void PressUiFourSound()
    {
        howerMenuSource.pitch = Random.Range(0.96f, highPitchRan);
        pressMenu4Source.Play();
    }
    public void PressBackButtonSound()
    {
        howerMenuSource.pitch = Random.Range(0.96f, highPitchRan);
        pressBackButtonSource.Play();
    }
    public void PressBattleButtonSound()
    {
        pressingBattleButtonSource.Play();
    }

    //foliage
    public void OpenDoorSound()
    {
        doorOpenSource.pitch = Random.Range(0.95f, highPitchRan);
        doorOpenSource.Play();
    }
    public void CloseDoorSound()
    {
        doorCloseSource.pitch = Random.Range(0.95f, highPitchRan);
        doorCloseSource.Play();
    }
    public void TrampolineSound()
    {
        trampolineSource.pitch = Random.Range(0.8f, highPitchRan);
        trampolineSource.Play();

    }
    public void FireWorkSound()
    {
        fireWorkSource.pitch = Random.Range(0.8f, highPitchRan);
        RandomClipPlayer(allFireworkSounds, fireWorkSource);
    }
    public void GlassShatterSound()
    {
        glassShatterSource.pitch = Random.Range(0.95f, highPitchRan);
        glassShatterSource.Play();
    }
	public void PortalSound()
    {
        portalSource.pitch = Random.Range(0.90f, highPitchRan);
        portalSource.Play();
    }

    
    

    /*
     private void RandomiseSoundPlayback()
     { //Gör att ett ljud körs random.
      //   if (!zombieRoarSoundSource.isPlaying)
         {
       //      zombieRoarSoundSource.pitch = Random.Range(lowPitchRan, highPitchRan);
             float t = Random.Range(minHumDelay, maxHumDelay);
       //      zombieRoarSoundSource.PlayDelayed(t);

         }
     }
    */

    public void HalfMusicVolume()
    {
        
        musicSource.volume = 0.3f;
        //Debug.Log("lower " + musicSource.volume);
    }

    public void FullMusicVolume()
    {
        musicSource.volume = 1f;
    }
   

    private void ToggleDoorSound()
    {
      //  if (!doorToggleSource.isPlaying)
        {
       //     doorToggleSource.pitch = Random.Range(0.8f, highPitchRan);
       //     doorToggleSource.PlayOneShot(doorToggleSound);
        }
    }
    private void RandomClipPlayer(AudioClip[] sounds, AudioSource source)
    {
        if (randomSongIndex >= sounds.Length)
        {
            randomSongIndex = 0;
        }
        //int randomIndex = Random.Range(0, sounds.Length);
        //  if (source.clip == sounds[randomIndex])
        //  {
        //   RandomClipPlayer(sounds, source);
        //  }
        source.clip = sounds[randomSongIndex];
        source.Play();
        randomSongIndex++;
    }

    private void createRandomSongOrder()
    {
        List<AudioClip> songList = new List<AudioClip>(allMusicSounds);
        songList = songList.OrderBy(i => Random.value).ToList();
        randomSongOrderList = songList.ToArray();
    }
    public static class FadeMixerGroup
    {
        public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
        {
            float currentTime = 0;
            float currentVol;
            audioMixer.GetFloat(exposedParam, out currentVol);
            currentVol = Mathf.Pow(10, currentVol / 20);
            float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
                audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
                yield return null;
            }
            yield break;
        }
    }
       

}

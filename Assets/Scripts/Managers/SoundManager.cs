using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
   
public class SoundManager : MonoBehaviour
{
    [Header("audio mixers")]
    [SerializeField] private AudioMixer overallMixer;

    [Header("music & ambience")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] allMusicSounds;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioSource ambience;
    [SerializeField] private AudioClip[] allAmbienceSounds;

    [Header("hazards")]
    [SerializeField] private AudioSource poisonHazardSource;
    [SerializeField] private AudioSource lavaHazardSource;
    [SerializeField] private AudioSource hazardWarningSource;
    [SerializeField] private AudioSource trampolineSource; //ligger för närvarande på trampolinprefab. används ej av soundManager
   
    [Header("foliage")]
    [SerializeField] private AudioSource doorOpenSource;
    [SerializeField] private AudioSource doorCloseSource;
    [SerializeField] private AudioSource bellRingSource;
    [SerializeField] private AudioSource startGameBellSource;
    
    [Header("UI sounds")]
    [SerializeField] private AudioSource howerMenuSource;
    [SerializeField] private AudioSource pressMenuSource;
    [SerializeField] private AudioSource pressMenu2Source;
    [SerializeField] private AudioSource pressMenu3Source;
    [SerializeField] private AudioSource pressMenu4Source;
    [SerializeField] private AudioSource pressBackButtonSource;
    [SerializeField] private AudioSource pressingBattleButtonSource;
   
    private float volLowRan = 0.3f;
    private float volHighRan = 1.0f;
    private float lowPitchRan = 0.3f;
    private float highPitchRan = 1.0f;
    private float minHumDelay = 25.0f;
    private float maxHumDelay = 200.0f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void FadeInMusic()
    {
        RandomClipPlayer(allMusicSounds, musicSource);
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 2f, 0.5f));
    }
    public void FadeOutMusic()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 1f, 0f));
    }
    public void FadeInBellSounds()
    {

        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "BellSounds", 1f, 0.4f));
    }
    
    public void FadeOutBellSounds()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "BellSounds", 3f, 0f));
    }
   

    public void FadeInLavaHazard()
    {
        lavaHazardSource.Play();
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 5f, 0.5f));
    }
    public void FadeOutLavaHazard()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 5f, 0f));
    }
    public void FadeInPoisionHazard()
    {
        poisonHazardSource.Play();
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 1, 0.5f));
    }
    public void FadeOutHazard()
    {
        
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 2, 0f));
    }
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
        int randomIndex = Random.Range(0, sounds.Length);
      //  if (source.clip == sounds[randomIndex])
      //  {
         //   RandomClipPlayer(sounds, source);
      //  }
        source.clip = sounds[randomIndex];
        source.Play();
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

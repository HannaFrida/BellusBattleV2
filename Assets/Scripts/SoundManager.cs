using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
/*
     * 
     * @Author Simon Hessling Oscarson
     * Används i Weapon.
     * Används i enemyAI.
     * Används i maleeWeapon
     */
public class SoundManager : MonoBehaviour
{
    [Header("audio mixers")]
    [SerializeField] private AudioMixer overallMixer;
    [Header("music & ambience")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] allMusicSounds;
    [SerializeField] private AudioSource hazardSource;
    [SerializeField] private AudioClip[] allHazardSounds;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioSource ambience;
    [SerializeField] private AudioClip[] allAmbienceSounds;
    [Header("foliage")]
    [SerializeField] private AudioSource doorToggleSource;
    [SerializeField] private AudioSource bellRingSource;
    [SerializeField] private AudioSource startGameBellSource;
    [Header("UI sounds")]
    [SerializeField] private AudioSource howerMenuSource;
    [SerializeField] private AudioSource pressMenuSource;
    private float volLowRan = 0.3f;
    private float volHighRan = 1.0f;
    private float lowPitchRan = 0.3f;
    private float highPitchRan = 1.0f;
    private float minHumDelay = 25.0f;
    private float maxHumDelay = 200.0f;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        //ambienceDay.time = Random.Range(0, 60);
        //pianoMusic.time = Random.Range(0, 60);

        //SoundPlaying("normalSnapshot");
       
       // FadeInMusic();
    }
    void Update()
    {
      //  RandomiseSoundPlayback();
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

    public void FadeInHazard()
    {
       // PlaySound("Hazards", hazard);
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 2, 0.5f));
    }
    public void FadeOutHazard()
    {
        
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 2, 0f));
    }
    public void HazardWarningSound()
    {
        // PlaySound("Hazards", hazard);
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "HazardMixerGroup", 2, 0.5f));
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
        howerMenuSource.pitch = Random.Range(0.95f, highPitchRan);
        howerMenuSource.Play();
    }
    public void PressUiSound()
    {
        pressMenuSource.Play();
    }
    public void PlaySound(string mixerName,AudioClip soundToPlay)
    { //hanterar vilket ljud som spelas. kallas på i andra scripts.
        if (mixerName == "Hazards")
        {
            PlayLavaHazard(soundToPlay);
        }
        //if (soundToPlay == "intenseSnapshot")
        //{
        //   intenseSnapshot.TransitionTo(0.0f);
        //   if (!intenseMusic.isPlaying)
        //      intenseMusic.Play();
        //    Debug.Log("intenseMusic");
        // }
    }
    private void PlayLavaHazard(AudioClip hazardSound)
    {
        hazardSource.PlayOneShot(hazardSound);
    }
    private void MeleeAttackSound()
    {
   //     meleeAttackSoundSource.pitch = Random.Range(0.6f, highPitchRan);//kanske ska ha samma pitch hela tiden?
    //    meleeAttackSoundSource.PlayOneShot(meleeSound);
    }
    private void PickUp()
    {
       // pickUpSoundSource.pitch = Random.Range(0.8f, highPitchRan);//kanske ska ha samma pitch hela tiden?
    //    pickUpSoundSource.PlayOneShot(pickUpSound);
    }
    private void ZombieDeathSound()
    {
        float vol = Random.Range(volLowRan, volHighRan);
    //    zombieDeathSource.pitch = Random.Range(lowPitchRan, highPitchRan);
    //    zombieDeathSource.PlayOneShot(zombieDeathSound, vol);
    }
    private void RandomiseSoundPlayback()
    { //Gör att ett ljud körs random.
     //   if (!zombieRoarSoundSource.isPlaying)
        {
      //      zombieRoarSoundSource.pitch = Random.Range(lowPitchRan, highPitchRan);
            float t = Random.Range(minHumDelay, maxHumDelay);
      //      zombieRoarSoundSource.PlayDelayed(t);

        }
    }
   

    private void ToggleDoorSound()
    {
        if (!doorToggleSource.isPlaying)
        {
            doorToggleSource.pitch = Random.Range(0.8f, highPitchRan);
       //     doorToggleSource.PlayOneShot(doorToggleSound);
        }
    }
    private void RandomClipPlayer(AudioClip[] sounds, AudioSource source)
    {
        int randomIndex = Random.Range(0, sounds.Length);
        if (source.clip == sounds[randomIndex])
        {
            RandomClipPlayer(sounds, source);
        }
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

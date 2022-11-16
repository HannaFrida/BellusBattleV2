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
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private AudioSource ambience;
    [SerializeField] private AudioClip[] allAmbienceSounds;
    [Header("foliage")]
    [SerializeField] private AudioSource doorToggleSource;
    private AudioSource subtitlesSoundSource;
    private AudioSource pickUpSoundSource;
   

    private float volLowRan = 0.3f;
    private float volHighRan = 1.0f;
    private float lowPitchRan = 0.3f;
    private float highPitchRan = 1.0f;
    private float minHumDelay = 25.0f;
    private float maxHumDelay = 200.0f;
    void Start()
    {

        //ambienceDay.time = Random.Range(0, 60);
        //pianoMusic.time = Random.Range(0, 60);

        //SoundPlaying("normalSnapshot");
       
        FadeInMusic();
    }
    void Update()
    {
      //  RandomiseSoundPlayback();
    }
    public void FadeInMusic()
    {
        RandomClipPlayer(allMusicSounds, musicSource);
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 2, 0.5f));
    }
    public void FadeOutMusic()
    {
        StartCoroutine(FadeMixerGroup.StartFade(overallMixer, "MusicMixerGroup", 2, 0f));
    }

    public void SoundPlaying(string clip)
    { //hanterar vilket ljud som spelas. kallas på i andra scripts.

        if (clip == "intenseSnapshot")
        {
         //   intenseSnapshot.TransitionTo(0.0f);
         //   if (!intenseMusic.isPlaying)
          //      intenseMusic.Play();
            Debug.Log("intenseMusic");
        }
        if (clip == "normalSnapshot")
        {
         //   normalSnapshot.TransitionTo(0.0f);
         //   intenseMusic.Stop();
            //Debug.Log("normalMusic");
        }
        if (clip == "shootSound") //Weapon
        {
            Shoot();
        }
        if (clip == "reload") //Weapon
        {
            ReloadSound();
        }
        if (clip == "meleeAttack") //Weapon
        {
            MeleeAttackSound();
            Debug.Log("meleeAttackSounds");
        }
        if (clip == "pickUp") //Interactor
        {
            PickUp();
        }
        if (clip == "subtitlesSound") //SubsScript
        {
            Subtitles();
        }
        if (clip == "zombieDeathSound") //enemyAI
        {
            ZombieDeathSound();
            Debug.Log("zombieDiedd");
        }
        if (clip == "zombieDamagedSound") //enemyAI
        {
            ZombieDamagedSound();
        }
        if (clip == "headshotSound") //enemyAI
        {
            ZombieHeadshotSound();
        }
        if (clip == "generatorOn") //Generator
        {
            GeneratorTurnedOn();
            Debug.Log("generator turned on");
        }
        if (clip == "generatorOff") //Generator
        {
            GeneratorBroke();
            Debug.Log("generator broke");
        }
        if (clip == "newWave") //Generator
        {
            NewWave();
        }
        if (clip == "danHit") //Dan take damage
        {
        //    RandomClip(danHitSounds, danHitSoundSource);
            DanHitSound();
        }
        if (clip == "kateHit") //Kate take damage
        {
       //     RandomClip(kateHitSounds, kateHitSoundSource);
            KateHitSound();
        }
        if (clip == "toggleDoor") //Doors
        {
            ToggleDoorSound();
        }
        if (clip == "zombieAttack")
        {
         //   RandomClip(zombieAttackSounds, zombieAttackSoundSource);
            ZombieAttackSound();
        }
    }
    private void Shoot()
    {
    //    shootSoundSource.pitch = Random.Range(0.6f, highPitchRan);//kanske ska ha samma pitch hela tiden?
    //    shootSoundSource.PlayOneShot(shootSound);
    }
    private void MeleeAttackSound()
    {
   //     meleeAttackSoundSource.pitch = Random.Range(0.6f, highPitchRan);//kanske ska ha samma pitch hela tiden?
    //    meleeAttackSoundSource.PlayOneShot(meleeSound);
    }
    private void PickUp()
    {
        pickUpSoundSource.pitch = Random.Range(0.8f, highPitchRan);//kanske ska ha samma pitch hela tiden?
    //    pickUpSoundSource.PlayOneShot(pickUpSound);
    }

    private void Subtitles()
    {
        subtitlesSoundSource.pitch = Random.Range(0.8f, highPitchRan);//kanske ska ha samma pitch hela tiden?
  //      subtitlesSoundSource.PlayOneShot(subtitlesSound);
    }
    private void ZombieDeathSound()
    {
        float vol = Random.Range(volLowRan, volHighRan);
    //    zombieDeathSource.pitch = Random.Range(lowPitchRan, highPitchRan);
    //    zombieDeathSource.PlayOneShot(zombieDeathSound, vol);
    }
    private void ZombieDamagedSound()
    {
        float vol = Random.Range(volLowRan, volHighRan);
   //     zombieTakesDamageSoundSource.pitch = Random.Range(lowPitchRan, highPitchRan);
  //      zombieTakesDamageSoundSource.PlayOneShot(zombieTakesDamageSound, vol);
    }
    private void ZombieHeadshotSound()
    {
        float vol = Random.Range(volLowRan, volHighRan);
   //     zombieTakesDamageSoundSource.pitch = Random.Range(0.8f, 1.2f);
    //    zombieTakesDamageSoundSource.PlayOneShot(headshotSound, vol);


    }
    private void ReloadSound()
    {
        //reloadSoundSource.pitch = Random.Range(0.6f, highPitchRan);
    //    reloadSoundSource.PlayOneShot(reloadSound);
    }
    private void NewWave()
    {
  //      newWaveAudioSource.pitch = Random.Range(0.8f, highPitchRan);
    //    newWaveAudioSource.PlayOneShot(newWave);
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
    private void GeneratorTurnedOn()
    {
      //  generatorSourceOn.pitch = Random.Range(0.8f, highPitchRan);
     //   generatorSourceOn.PlayOneShot(generatorOnSound);
    }
    private void GeneratorBroke()
    {
    //    generatorSource.pitch = Random.Range(0.8f, highPitchRan);
   //     generatorSource.time = 4;
   //     generatorSource.PlayOneShot(generatorOffSound);
    }

    private void DanHitSound()
    {
     //   danHitSoundSource.pitch = Random.Range(0.8f, highPitchRan);
     //   danHitSoundSource.PlayOneShot(danHitSoundSource.clip);
    }

    private void KateHitSound()
    {
       // kateHitSoundSource.pitch = Random.Range(0.8f, highPitchRan);
      //  kateHitSoundSource.PlayOneShot(kateHitSoundSource.clip);
    }

    private void ZombieAttackSound()
    {
      //  if (!zombieAttackSoundSource.isPlaying)
        {
      //      zombieAttackSoundSource.pitch = Random.Range(0.8f, highPitchRan);
      //      zombieAttackSoundSource.PlayOneShot(zombieAttackSoundSource.clip);
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

    //Nyman
    //Returns random clip and make sure the same clip does not repeat
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
        public void MuteSource(string sourceName)
    {
        if (sourceName == "subtitles")
        {
            subtitlesSoundSource.Stop();
        }
        else if (sourceName == "generatorOn")
        {
     //       generatorSourceOn.Stop();
        }
    }

}

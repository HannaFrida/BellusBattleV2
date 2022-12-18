using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class TransitionManager : MonoBehaviour
{

    public enum FadeType { FadeBlack, FadeSimon, FadeCircle, FadeSqure, FadeBomb, FadeRoll }
    private Animator _ac;

    [Header("Transition Settigns ")]
    [SerializeField] FadeType _fadeType = FadeType.FadeSimon;
    [Tooltip("The playbackspeed of the animator, default = 1 ie val 2 = 2 second animation")]
    [SerializeField] bool _fadeOnStart = true;
    public bool _fadeOnEnd = true;
    public float _animationSpeed = 1;

    [Header("Randomization")]
    [SerializeField] bool _randomizeFadeType = false;
    [Tooltip("Fade in same style as last fadeout while using the random FadeType")]
    [SerializeField] bool _crossFadeOnRandom = true;

    //private int _counter = 0;
    private FadeType _lastFadeTypeUsed;

    private void Start()
    {
        _ac = GetComponent<Animator>();
        _ac.speed = _animationSpeed;
        //  _lastFadeTypeUsed = _fadeType;
        //  if (_fadeOnStart) CheckRandomFadeIn(); FadeIn();
    }

    private void OnLevelWasLoaded(int level)
    {
        _lastFadeTypeUsed = _fadeType;
        if (_fadeOnStart) CheckRandomFadeIn(); FadeIn();
    }


    //Play Fade Out Animation
    public void FadeOut()
    {
        if (_fadeOnEnd == false) return;
        /*else if (_counter == 0) return;
        _counter++;*/

        //Randomize fadetype
        if (_randomizeFadeType)
        {
            _fadeType = GetRandomFadeType();
            _lastFadeTypeUsed = _fadeType;
        }

        switch (_fadeType)
        {
            case FadeType.FadeBlack:
                _ac.SetTrigger("FadeOutBlack");
                break;
            case FadeType.FadeSimon:
                _ac.SetTrigger("FadeOutSimon");
                break;
            case FadeType.FadeCircle:
                _ac.SetTrigger("FadeOutCircle");
                break;
            case FadeType.FadeSqure:
                _ac.SetTrigger("FadeOutSquare");
                break;
            case FadeType.FadeBomb:
                _ac.SetTrigger("FadeOutBomb");
                break;
            case FadeType.FadeRoll:
                _ac.SetTrigger("FadeOutRoll");
                break;
        }
    }

    //Play Fade In Animation
    public void FadeIn()
    {
        if (!_fadeOnStart) { return; }
        CheckRandomFadeIn();

        switch (_fadeType)
        {
            case FadeType.FadeBlack:
                _ac.SetTrigger("FadeInBlack");
                break;
            case FadeType.FadeSimon:
                _ac.SetTrigger("FadeInSimon");
                break;
            case FadeType.FadeCircle:
                _ac.SetTrigger("FadeInCircle");
                break;
            case FadeType.FadeSqure:
                _ac.SetTrigger("FadeInSquare");
                break;
            case FadeType.FadeBomb:
                _ac.SetTrigger("FadeInBomb");
                break;
            case FadeType.FadeRoll:
                _ac.SetTrigger("FadeInRoll");
                break;
        }
    }
    FadeType GetRandomFadeType()
    {
        return (FadeType)Random.Range(0, Enum.GetValues(typeof(FadeType)).Length);
    }

    private void CheckRandomFadeIn()
    {
        if (_randomizeFadeType && _crossFadeOnRandom)
        {
            _fadeType = _lastFadeTypeUsed;
        }
        else if (_randomizeFadeType)
        {
            _fadeType = GetRandomFadeType();
        }
    }

}


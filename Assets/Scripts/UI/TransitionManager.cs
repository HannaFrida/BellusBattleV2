using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Animator))]
public class TransitionManager : MonoBehaviour
{

    public enum FadeType {Random, FadeBlack, FadeSimon, FadeCircle, FadeSqure, FadeBomb, FadeRoll}
    private Animator _ac;

    [SerializeField] bool _fadeOnStart = true;
    [Tooltip("Fade in same style as last fadeout while using the random FadeType")]
    [SerializeField] bool _crossFadeOnRandom = true;
    [SerializeField] FadeType _fadeType = FadeType.Random;
    [Tooltip("The playbackspeed of the animator, default = 1")]
    [SerializeField] float _animationSpeed = 1;

    private FadeType lastFadeTypeUsed;

    private void Start()
    {
        _ac = GetComponent<Animator>();
        _ac.speed = _animationSpeed;
        if (_fadeOnStart) FadeIn(_fadeType);
    }

    //Play Fade Out Animation
    public void FadeOut(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.Random:
                FadeOut(lastFadeTypeUsed = GetRandomFadeType());
                break;
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
    public void FadeIn(FadeType fadeType)
    {
        switch (fadeType)
        {
            case FadeType.Random:
                if (_crossFadeOnRandom) FadeIn(lastFadeTypeUsed);
                else FadeIn(GetRandomFadeType());
                break;
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

}


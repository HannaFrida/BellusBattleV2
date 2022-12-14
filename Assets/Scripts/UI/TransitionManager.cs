using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TransitionManager : MonoBehaviour
{

    public enum FadeType { FadeBlack, FadeSimon, FadeCircle, FadeSqure, FadeBomb }
    private Animator _ac;

    [SerializeField] bool _fadeOnStart = true;
    [SerializeField] FadeType _fadeType = FadeType.FadeSimon;
    [Tooltip("The playbackspeed of the animator, default = 1")]
    [SerializeField] float _animationSpeed = 1;

    private void Start()
    {
        _ac = GetComponent<Animator>();
        _ac.speed = _animationSpeed;
        if (_fadeOnStart) FadeIn(_fadeType);
    }


    public void FadeOut(FadeType fadeType)
    {
        switch (fadeType)
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
        }
    }

    public void FadeIn(FadeType fadeType)
    {
        switch (fadeType)
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
        }
    }
}


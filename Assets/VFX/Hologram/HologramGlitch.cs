using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class HologramGlitch : MonoBehaviour
{
    public float maxGlitch;

    public float minGlitch;

    public float glitchLength;

    public float timeBetweenGlitches;
    private Renderer renderer;
    private Material mainMaterial;

    private void Start()
    {
        StartCoroutine(Glitch());
    }

    IEnumerator Glitch()
    {
        Renderer renderer = GetComponent<Renderer>();

        Material mainMaterial = renderer.material;
        print(mainMaterial);

        yield return new WaitForSeconds(timeBetweenGlitches);

        mainMaterial.SetFloat("_Glitch_Strength", Random.Range(minGlitch, maxGlitch));

        yield return new WaitForSeconds(glitchLength);

        mainMaterial.SetFloat("_Glitch_Strength", 0f);

        StartCoroutine(Glitch());
    }


}
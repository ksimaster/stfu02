using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimGif : MonoBehaviour
{
    public Texture2D[] frames;
    private float framesPerSecond = 200000f;
    private RawImage im = null;
    private Renderer rend = null;

    private void Awake()
    {
        im = GetComponent<RawImage>();
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
    float index = Time.time * framesPerSecond; 
    index = index % frames.Length;
        if (rend != null)
            rend.material.mainTexture = frames[(int)index];
        else
            im.texture = frames[(int)index];
    }



}

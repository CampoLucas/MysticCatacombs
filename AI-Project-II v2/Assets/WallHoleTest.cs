using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHoleTest : MonoBehaviour
{
    public Renderer WallRenderer;
    private Material _wallMaterial;
    
    private void Awake()
    {
        _wallMaterial = WallRenderer.sharedMaterial;
    }

    private void Update()
    {
        _wallMaterial.SetVector("_Hole", transform.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour {

    public float warpPos = -10.3f;
    public MeshRenderer Renderer;
    float speed = 5f;
    public ParticleSystem particles;

    public float dist;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
            Renderer.material.SetFloat("_WarpPos", warpPos);
        
    }
}

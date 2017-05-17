using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour {

    float warpPos = -10.3f;
    MeshRenderer Renderer;
    float speed = 5f;
    public ParticleSystem particles;

    float dist;

	// Use this for initialization
	void Start () {
        Renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (dist < 14.5f)
        {
            transform.position += new Vector3(0, 0, Time.deltaTime * speed);
            warpPos += Time.deltaTime * speed;
            Renderer.material.SetFloat("_WarpPos", warpPos);
            particles.transform.localPosition = new Vector3(0, warpPos - 0.1f, 0);

            dist += Time.deltaTime * speed;
        }
        else
        {
            particles.Stop();
        }
    }
}

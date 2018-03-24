using UnityEngine;

public class warp : MonoBehaviour
{
    public float dist;
    public ParticleSystem particles;
    public MeshRenderer Renderer;
    private float speed = 5f;

    public float warpPos = -10.3f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Renderer.material.SetFloat("_WarpPos", warpPos);
    }
}
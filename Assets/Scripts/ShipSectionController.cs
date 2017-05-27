using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSectionController : MonoBehaviour {

    public ShipController Ship { get; private set; }
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    private bool _dying;
    private ParticleSystem _explosion;

	// Use this for initialization
	void Start () {
        // store ref to the ship this is attached to
        Ship = transform.parent.GetComponent<ShipController>();

        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _explosion = GetComponentInChildren<ParticleSystem>();

        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
    }
    
    public void OnResetToStart()
    {
        if (_dying)
        {
            _dying = false;
            _meshRenderer.enabled = true;
            _collider.enabled = true;
        }
    }

    public void Die()
    {
        // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
        _dying = true;

        _collider.enabled = false;

        StartCoroutine(PlayExplosion());
    }

    public void KillSelf()
    {
        GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);

        GameObject.Destroy(this.transform.gameObject);
    }

    private IEnumerator PlayExplosion()
    {        
        _explosion.Play();
        yield return new WaitForSeconds(1f);
        _meshRenderer.enabled = false;
        yield return null;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour {

    public PanelDirection Direction;
    public float Width;
    public float Height;
    public bool Active;

    private float currentPosition;
    private float targetPosition;
    private float speed = 1500f;

    private Vector3 basePosition;

	// Use this for initialization
	void Start () {
        basePosition = transform.localPosition ;
        SetActive(Active);
        currentPosition = targetPosition;
	}
	
	// Update is called once per frame
	void Update () {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (currentPosition < targetPosition)
        {
            currentPosition += speed * Time.deltaTime;
            if (currentPosition > targetPosition)
            {
                currentPosition = targetPosition;
            }
        }
        else if (currentPosition > targetPosition)
        {
            currentPosition -= speed * Time.deltaTime;
            if (currentPosition < targetPosition)
            {
                currentPosition = targetPosition;
            }
        }
        SetPosition();
    }

    void SetPosition()
    {
        if (Direction == PanelDirection.TopDown)
        {
            transform.localPosition = new Vector3(0, -currentPosition) + basePosition;
        }
        else if (Direction == PanelDirection.BottomUp)
        {
            transform.localPosition = new Vector3(0, currentPosition) + basePosition;
        }
        else if (Direction == PanelDirection.LeftToRight)
        {
            transform.localPosition = new Vector3(currentPosition, 0) + basePosition;
        }
        else if (Direction == PanelDirection.RightToLeft)
        {
            transform.localPosition = new Vector3(-currentPosition, 0) + basePosition;
        }
    }

    public void SetActive(bool active)
    {
        Active = active;
        float size = (Direction == PanelDirection.TopDown || Direction == PanelDirection.BottomUp ? Height : Width);
            
        targetPosition = (active ? 0 : -size);
    }
}

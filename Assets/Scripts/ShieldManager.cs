﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour {

    public LineRenderer NWBeam;
    public LineRenderer NEBeam;
    public LineRenderer SWBeam;
    public LineRenderer SEBeam;

    public Color Color;

    public float Width;
    public float Height;
    public float Radius;

    private float currentWidth = 0;
    private float currentHeight = 0;

    public float speed = 0.2f;

    public MeshRenderer shieldRenderer;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        shieldRenderer.material.SetColor("_Color", Color);
        if (currentWidth != Width)
        {
            currentWidth = Width;// * Time.deltaTime * speed;
        }
        if (currentHeight != Height)
        {
            currentHeight = Height;// * Time.deltaTime * speed;
        }

        shieldRenderer.material.SetFloat("_WidthAngle", currentWidth);

        shieldRenderer.material.SetFloat("_HeightAngle", currentHeight);

        // beams
        SetBeamPosition(-currentWidth, currentHeight, NWBeam);
        SetBeamPosition(currentWidth, currentHeight, NEBeam);
        SetBeamPosition(-currentWidth, -currentHeight, SWBeam);
        SetBeamPosition(currentWidth, -currentHeight, SEBeam);
    }

    void SetBeamPosition(float angle1, float angle2, LineRenderer lr)
    {
        Vector3 p1 = new Vector3(Mathf.Cos(angle1), 0, -Mathf.Sin(angle1));
        Vector3 p2 = new Vector3(0, Mathf.Cos(angle2), Mathf.Sin(angle2));
        
        // cross product
        Vector3 cp = Vector3.Cross(p1, p2);
        cp.Normalize();
        
        int NUMPOINTS = 2;
        Vector3[] points = new Vector3[NUMPOINTS];
        points[0] = new Vector3();
        for (int i = 1; i < NUMPOINTS; i++)
        {
            points[i] = Radius * cp / i;
        }

        shieldRenderer.transform.localScale = new Vector3(Radius*2, Radius*2, Radius*2);

        lr.numPositions = NUMPOINTS;
        lr.SetPositions(points);
     
    }
}

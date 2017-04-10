using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour {

    public LineRenderer NWBeam;
    public LineRenderer NEBeam;
    public LineRenderer SWBeam;
    public LineRenderer SEBeam;

    public float width;
    public float height;
    public float radius;

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
        if (currentWidth != width)
        {
            currentWidth = width;// * Time.deltaTime * speed;
        }
        if (currentHeight != height)
        {
            currentHeight = height;// * Time.deltaTime * speed;
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
        Vector3[] points = new Vector3[2];
        points[0] = new Vector3();
        points[1] = new Vector3(0,0,radius/2f);

        shieldRenderer.transform.localScale = new Vector3(radius, radius, radius);

        lr.SetPositions(points);

        lr.transform.localEulerAngles = new Vector3( 0,0, 0);
        lr.transform.Rotate(Vector3.up, Mathf.Rad2Deg * angle1 );
        lr.transform.Rotate(Vector3.right, Mathf.Rad2Deg * angle2  * Mathf.Cos(angle1));
        

        //float x = radius * Mathf.Cos(angle1) * Mathf.Sin(angle2);
        //float y = radius * Mathf.Sin(angle1) * Mathf.Sin(angle2);
        //float z = radius * Mathf.Cos(angle2);

        
    }
}

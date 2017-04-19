using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    public float MaxAngle; // in degrees
    public float Range;

    private float lastMaxAngle;
    private float lastRange;

    public LineRenderer FireArcIndicator;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        // redraw fire arc indicator if maxangle or range has changed
        if (Mathf.Abs(lastMaxAngle - MaxAngle) > 0.001f
            || Mathf.Abs(lastRange - Range) > 0.001f)
        {
            float anglePerPoint = 5f;
            int pointsInArc = (int)(MaxAngle / anglePerPoint);
            if (pointsInArc < 2) pointsInArc = 2;
            Vector3[] points = new Vector3[pointsInArc + 2];

            // first and last points are at origin
            points[0] = new Vector3();
            points[points.Length - 1] = new Vector3();

            float angle = -MaxAngle / 2f;
            for (int i = 1; i <= points.Length-2; i++)
            {
                float angleInRads = Mathf.Deg2Rad * angle;
                points[i] = new Vector3(Range * Mathf.Cos(angleInRads), 0, Range * Mathf.Sin(angleInRads));
                angle += (MaxAngle / pointsInArc);
            }

            // set the points
            FireArcIndicator.numPositions = points.Length;
            FireArcIndicator.SetPositions(points);
        }

        lastMaxAngle = MaxAngle;
        lastRange = Range;

	}
}

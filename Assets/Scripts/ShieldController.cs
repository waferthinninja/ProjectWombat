using System;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour {

    public LineRenderer NWBeam;
    public LineRenderer NEBeam;
    public LineRenderer SWBeam;
    public LineRenderer SEBeam;
    
    public Transform RotationPoint;

    public Color Color;

    [Range(0.0f, 1.57f)]
    public float Width; // in radians - TODO convert to degrees?
    [Range(0.0f, 1.57f)]
    public float Height; // in radians
    public float Radius;

    public float MaxAngle; // how far it can rotate, in degrees

    private float currentWidth = 0;
    private float currentHeight = 0;

    public float MaxStrength = 100f;
    private float _strength = 100f; // strength at start of turn
    private const float MAX_INTENSITY = 0.3f; 

    public MeshRenderer shieldRenderer;

    public Transform Target;

    private List<Tuple<float, float>> _damageThisTurn;

    // Use this for initialization
    void Start()
    {
        // get faction of ship 
        Color = FactionColors.ShieldColor[transform.parent.GetComponent<ShipController>().Faction];
        _damageThisTurn = new List<Tuple<float, float>>();

        SetShieldParams();
        SetBeamPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            // turn to face target

            // check angle
            float angle = Vector3.Angle(transform.forward, (Target.position - transform.position));
            
            if (angle < MaxAngle / 2f)
            {
                RotationPoint.LookAt(Target);
            }
            else
            {
                // if out of angle, turn as far as possible
                RotationPoint.rotation = new Quaternion();
                // cross product will tell us which way
                Vector3 cross = Vector3.Cross(transform.forward, (Target.position - transform.position));
                RotationPoint.Rotate(0, MaxAngle/2f * (cross.y < 0 ? -1 : 1), 0);
            }
        }  
        

    }

    public float ApplyDamage(float damage)
    {
        float strBefore = GetStrength();
        _damageThisTurn.Add(new Tuple<float, float>(TimeController.Instance.GetTime(), damage));
        
        SetShieldParams();

        if (strBefore <= damage)
        {
            // shields down - deactivate collider 
            Debug.Log("Shields down");
            var collider = GetComponent<Collider>();
            collider.enabled = false;
            EnableDisable(false);

            // return any damage which continues through 
            return damage - strBefore;
        }
        return 0;
    }

    public float GetStrength()
    {
        float time = TimeController.Instance.GetTime();
        float str = _strength;

        foreach(var d in _damageThisTurn)
        {
            if (d.first <= time)
            {
                str -= d.second;
            }
        }

        return str;
    }

    private void SetShieldParams()
    {
        float alpha = (GetStrength() / MaxStrength) * MAX_INTENSITY;
        Color c = new Color(Color.r, Color.g, Color.b, alpha);
        shieldRenderer.material.SetColor("_Color", c);
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
    }

    private void EnableDisable(bool enabled)
    {
        shieldRenderer.enabled = enabled;
        NWBeam.enabled = enabled;
        NEBeam.enabled = enabled;
        SWBeam.enabled = enabled;
        SEBeam.enabled = enabled;
    }

    private void SetBeamPositions()
    {

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

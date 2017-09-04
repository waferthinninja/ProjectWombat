using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(TargetableComponentController))]
public class ShieldController : MonoBehaviour
{
    public string Name;

    public LineRenderer NWBeam;
    public LineRenderer NEBeam;
    public LineRenderer SWBeam;
    public LineRenderer SEBeam;    

    public Transform RotationPoint;

    public Color Color;

    [Range(0f, 180f)]
    public float Width; // in degrees
    [Range(0f, 180f)]
    public float Height; // in degrees
    public float Radius;

    public float MaxAngle; // how far it can rotate, in degrees

    private float currentWidth = 0;
    private float currentHeight = 0;

    public float MaxStrength = 100f;
    private float _strength;
    private float _strengthAtStartOfTurn;

    private float _lastMaxAngle;

    private const float MAX_INTENSITY = 0.3f;

    public MeshRenderer ComponentRenderer;
    public MeshRenderer ShieldRenderer;
    public LineRenderer ArcIndicator;
    private Collider _collider;
    
    private bool _dying;
    private bool _showArc;

    private TargetableComponentController _targeter;

    // Use this for initialization
    void Start()
    {
        // TODO - get renderers automatically 

        _targeter = GetComponent<TargetableComponentController>();

        // get faction of ship 
        var ship = transform.parent.parent.GetComponent<ShipController>();
        Color = FactionColors.ShieldColor[ship.Faction];

        _strength = MaxStrength;
        _strengthAtStartOfTurn = _strength;
        _collider = GetComponent<Collider>();

        SetShaderParams();
        SetBeamPositions();

        GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
        GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);
    }


    // Update is called once per frame
    void Update()
    {
        RedrawArcIfChanged();

        ShipController target = _targeter.Target;

        if (target != null)
        {
            // turn to face target

            // check angle
            float angle = Vector3.Angle(transform.forward, (target.transform.position - transform.position));
            if (angle > 180)
            {
                angle -= 360;
            }

            if (angle < MaxAngle / 2f)
            {
                RotationPoint.LookAt(target.transform);
            }
            else
            {
                // if out of angle, turn as far as possible
                RotationPoint.rotation = new Quaternion();
                // cross product will tell us which way
                Vector3 cross = Vector3.Cross(transform.forward, (target.transform.position - transform.position));
                RotationPoint.Rotate(0, MaxAngle/2f * (cross.y < 0 ? -1 : 1), 0);
            }
        }  
        
        //SetShaderParams();
        //SetBeamPositions();

    }

    internal void InitialiseFromStruct(Shield shield, ShieldType type)
    {
        Name = shield.Name;
        Width = type.Width;
        Height = type.Height;
        Radius = shield.Radius;
        MaxStrength = type.Strength;
        _strength = shield.CurrentStrength;
        MaxAngle = shield.MaxAngle;
    }

    public ShipController GetTarget()
    {
        return _targeter.Target;
    }

    public float GetRotationProportion()
    {
        float angle = RotationPoint.localEulerAngles.y;
        if (angle > MaxAngle / 2f)
        {
            angle -= 360;
        }
        return angle / (MaxAngle / 2f);
    }

    public void SetRotationProportion(float prop)
    {
        RotationPoint.localRotation = Quaternion.Euler(0,prop * (MaxAngle /2f),0);
    }

    public void RegisterForTargetCallback()
    {
        _targeter.RegisterForTargetCallback();
    }

    public void ToggleArc()
    {
        _showArc = !_showArc;
        ArcIndicator.enabled = _showArc;
    }

    public void SetArc(bool enabled)
    {
        _showArc = enabled;
        ArcIndicator.enabled = enabled;
    }

    private void RedrawArcIfChanged()
    {
        // redraw fire arc indicator if maxangle or range has changed
        if (Mathf.Abs(_lastMaxAngle - MaxAngle) > 0.001f)
        {
            RedrawArc();
        }

        _lastMaxAngle = MaxAngle;
    }

    private void RedrawArc()
    {
        Vector3[] points = new Vector3[3];

        // middle point is at origin
        points[1] = new Vector3();

        float angle = MaxAngle / 2f;
        float angleInRads = Mathf.Deg2Rad * angle;
        angleInRads += Width * Mathf.Deg2Rad;
        points[0] = new Vector3(10f * Mathf.Sin(angleInRads), 0, 10f * Mathf.Cos(angleInRads));
        points[2] = new Vector3(10f * Mathf.Sin(-angleInRads), 0, 10f * Mathf.Cos(-angleInRads));

        // set the points
        ArcIndicator.positionCount = points.Length;
        ArcIndicator.SetPositions(points);
    }

    public void Die()
    {
        // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
        _dying = true;

        ComponentRenderer.enabled = false;
        ArcIndicator.enabled = _showArc;
        EnableDisable(false);
    }

    public void OnResetToStart()
    {
        if (_dying)
        {
            ComponentRenderer.enabled = true;
            _dying = false;
        }
        _strength = _strengthAtStartOfTurn;
        EnableDisable(_strength > 0);
        ArcIndicator.enabled = _showArc;
        SetShaderParams();
        SetBeamPositions();
    }

    public void OnEndOfTurn()
    {
        _strengthAtStartOfTurn = _strength;
    }

    public void OnStartOfOutcome()
    {
        SetArc(false);
    }

    public void OnStartOfPlanning()
    {
        SetArc(true);
    }

    public float ApplyDamage(float damage)
    {
        _strength -= damage;

        SetShaderParams();

        if (_strength <= 0)
        {
            // shields down - deactivate collider             
            EnableDisable(false);

            // return any damage which continues through 
            return -_strength;
        }
        return 0;
    }

    private void SetShaderParams()
    {
        float alpha = (_strength / MaxStrength) * MAX_INTENSITY;
        Color c = new Color(Color.r, Color.g, Color.b, alpha);
        ShieldRenderer.material.SetColor("_Color", c);
        if (currentWidth != Width)
        {
            currentWidth = Width;// * Time.deltaTime * speed;
        }
        if (currentHeight != Height)
        {
            currentHeight = Height;// * Time.deltaTime * speed;
        }

        ShieldRenderer.material.SetFloat("_WidthAngle", currentWidth * Mathf.Deg2Rad);
        ShieldRenderer.material.SetFloat("_HeightAngle", currentHeight * Mathf.Deg2Rad);
    }

    private void EnableDisable(bool enabled)
    {
        _collider.enabled = enabled;
        ShieldRenderer.enabled = enabled;
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
        
        int numPoints = 2;
        Vector3[] points = new Vector3[numPoints];
        points[0] = new Vector3();
        for (int i = 1; i < numPoints; i++)
        {
            points[i] = Radius * cp / i;
        }

        ShieldRenderer.transform.localScale = new Vector3(Radius*2, Radius*2, Radius*2);

        lr.positionCount = numPoints;
        lr.SetPositions(points);     
    }
    
    public void KillSelf()
    {
        GameManager.Instance.UnregisterOnResetToStart(OnResetToStart);
        GameManager.Instance.UnregisterOnStartOfOutcome(OnStartOfOutcome);
        GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
        GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);

        GameObject.Destroy(this.transform.gameObject);
    }
}

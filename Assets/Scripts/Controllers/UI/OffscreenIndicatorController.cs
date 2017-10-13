using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenIndicatorController : MonoBehaviour {

    public Transform Target;
    private Image _image;

	// Use this for initialization
	void Start () {
        _image = GetComponent<Image>();
        // set colour based on faction 
        ShipController ship = Target.GetComponent<ShipController>();
        if (ship != null)
        {
            _image.color = FactionColors.IconColor[ship.Faction];
        }   
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 tScreenPos = Camera.main.WorldToScreenPoint(Target.position);

        if (onScreen(tScreenPos) || CameraManager.Instance.GetCurrentCamera() is FollowCameraController)
        {
            // make it invisible
            _image.enabled = false;
            return;
        }
        _image.enabled = true;

        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        float angle = Mathf.Atan2(tScreenPos.y - center.y, tScreenPos.x - center.x) * Mathf.Rad2Deg;       

        float angleCorner1 = Mathf.Atan2(Screen.height - center.y, Screen.width - center.x) * Mathf.Rad2Deg;
        float angleCorner2 = Mathf.Atan2(Screen.height - center.y, 0 - center.x) * Mathf.Rad2Deg;
        float angleCorner3 = Mathf.Atan2(0 - center.y, 0 - center.x) * Mathf.Rad2Deg + 360;
        float angleCorner4 = Mathf.Atan2(0 - center.y, Screen.width - center.x) * Mathf.Rad2Deg + 360;
        
        if (angle < 0) angle += 360;
        
        int edgeLine;
        if (angle < angleCorner1) edgeLine = 0;
        else if (angle < angleCorner2) edgeLine = 1;
        else if (angle < angleCorner3) edgeLine = 2;
        else if (angle < angleCorner4) edgeLine = 3;
        else edgeLine = 0;

        transform.position = intersect(edgeLine, center, tScreenPos);
        transform.eulerAngles = new Vector3(0, 0, angle);


    }

    bool onScreen(Vector2 input)
    {
        return !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
    }

    Vector3 intersect(int edgeLine, Vector3 line2point1, Vector3 line2point2)
    {
        float[] A1 = { -Screen.height, 0, Screen.height, 0 };
        float[] B1 = { 0, -Screen.width, 0, Screen.width };
        float[] C1 = { -Screen.width * Screen.height, -Screen.width * Screen.height, 0, 0 };

        float A2 = line2point2.y - line2point1.y;
        float B2 = line2point1.x - line2point2.x;
        float C2 = A2 * line2point1.x + B2 * line2point1.y;

        float det = A1[edgeLine] * B2 - A2 * B1[edgeLine];

        return new Vector3(
            (B2 * C1[edgeLine] - B1[edgeLine] * C2) / det, 
            (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det, 
            0);
    }
}

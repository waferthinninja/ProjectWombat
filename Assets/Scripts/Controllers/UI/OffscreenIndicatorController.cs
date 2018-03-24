using Controllers.Cameras;
using Controllers.ShipComponents;
using Managers;
using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class OffscreenIndicatorController : MonoBehaviour
    {
        private Image _image;

        public Transform Target;

        // Use this for initialization
        private void Start()
        {
            _image = GetComponent<Image>();
            // set colour based on faction 
            var ship = Target.GetComponent<ShipController>();
            if (ship != null) _image.color = FactionColors.IconColor[ship.Faction];
        }

        // Update is called once per frame
        private void Update()
        {
            var tScreenPos = Camera.main.WorldToScreenPoint(Target.position);

            if (onScreen(tScreenPos) || CameraManager.Instance.GetCurrentCamera() is FollowCameraController)
            {
                // make it invisible
                _image.enabled = false;
                return;
            }

            _image.enabled = true;

            var center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            var angle = Mathf.Atan2(tScreenPos.y - center.y, tScreenPos.x - center.x) * Mathf.Rad2Deg;

            var angleCorner1 = Mathf.Atan2(Screen.height - center.y, Screen.width - center.x) * Mathf.Rad2Deg;
            var angleCorner2 = Mathf.Atan2(Screen.height - center.y, 0 - center.x) * Mathf.Rad2Deg;
            var angleCorner3 = Mathf.Atan2(0 - center.y, 0 - center.x) * Mathf.Rad2Deg + 360;
            var angleCorner4 = Mathf.Atan2(0 - center.y, Screen.width - center.x) * Mathf.Rad2Deg + 360;

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

        private bool onScreen(Vector2 input)
        {
            return !(input.x > Screen.width || input.x < 0 || input.y > Screen.height || input.y < 0);
        }

        private Vector3 intersect(int edgeLine, Vector3 line2point1, Vector3 line2point2)
        {
            float[] A1 = {-Screen.height, 0, Screen.height, 0};
            float[] B1 = {0, -Screen.width, 0, Screen.width};
            float[] C1 = {-Screen.width * Screen.height, -Screen.width * Screen.height, 0, 0};

            var A2 = line2point2.y - line2point1.y;
            var B2 = line2point1.x - line2point2.x;
            var C2 = A2 * line2point1.x + B2 * line2point1.y;

            var det = A1[edgeLine] * B2 - A2 * B1[edgeLine];

            return new Vector3(
                (B2 * C1[edgeLine] - B1[edgeLine] * C2) / det,
                (A1[edgeLine] * C2 - A2 * C1[edgeLine]) / det,
                0);
        }
    }
}
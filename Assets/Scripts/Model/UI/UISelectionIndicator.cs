using Managers;
using Model.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Model.UI
{
    public class UISelectionIndicator : MonoBehaviour
    {
        private static Vector3[] screenSpaceCorners;
        public Camera Camera;
        public Image Indicator;
        public Text Text;

        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            if (InputManager.Instance.SelectedShip != null)
            {
                for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(true);

                var visualRect =
                    RendererBoundsInScreenSpace(InputManager.Instance.SelectedShip.GetComponentInChildren<Renderer>());

                var rt = GetComponent<RectTransform>();

                rt.position = new Vector2(visualRect.xMin, visualRect.yMin);

                rt.sizeDelta = new Vector2(visualRect.width, visualRect.height);

                Text.text = InputManager.Instance.SelectedShip.Name;
                var factionColor = Color.white;
                factionColor = InputManager.Instance.SelectedShip.Faction == Faction.Friendly ? Color.green : Color.red;

                Indicator.color = factionColor;
                Text.color = factionColor;
            }
            else
            {
                for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private Rect RendererBoundsInScreenSpace(Renderer r)
        {
            // This is the space occupied by the object's visuals
            // in WORLD space.
            var bigBounds = r.bounds;

            if (screenSpaceCorners == null)
                screenSpaceCorners = new Vector3[8];


            // For each of the 8 corners of our renderer's world space bounding box,
            // convert those corners into screen space.
            screenSpaceCorners[0] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x,
                bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[1] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x,
                bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
            screenSpaceCorners[2] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x,
                bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[3] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x,
                bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
            screenSpaceCorners[4] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x,
                bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[5] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x,
                bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
            screenSpaceCorners[6] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x,
                bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
            screenSpaceCorners[7] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x,
                bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));

            // Now find the min/max X & Y of these screen space corners.
            var min_x = screenSpaceCorners[0].x;
            var min_y = screenSpaceCorners[0].y;
            var max_x = screenSpaceCorners[0].x;
            var max_y = screenSpaceCorners[0].y;

            for (var i = 1; i < 8; i++)
            {
                if (screenSpaceCorners[i].x < min_x) min_x = screenSpaceCorners[i].x;
                if (screenSpaceCorners[i].y < min_y) min_y = screenSpaceCorners[i].y;
                if (screenSpaceCorners[i].x > max_x) max_x = screenSpaceCorners[i].x;
                if (screenSpaceCorners[i].y > max_y) max_y = screenSpaceCorners[i].y;
            }

            return Rect.MinMaxRect(min_x, min_y, max_x, max_y);
        }
    }
}
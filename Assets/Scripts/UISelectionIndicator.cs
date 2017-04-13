using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISelectionIndicator : MonoBehaviour
{

    public Camera Camera;
    public Text Text;
    public Image Indicator;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (InputController.Instance.SelectedShip != null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }

            Rect visualRect = RendererBoundsInScreenSpace(InputController.Instance.SelectedShip.GetComponentInChildren<Renderer>());
            
            RectTransform rt = GetComponent<RectTransform>();

            rt.position = new Vector2(visualRect.xMin, visualRect.yMin);

            rt.sizeDelta = new Vector2(visualRect.width, visualRect.height);

            Text.text = InputController.Instance.SelectedShip.ShipName;
            Color factionColor = Color.white;
            factionColor = (InputController.Instance.SelectedShip.Faction == Faction.Friendly ? Color.green : Color.red );

            Indicator.color = factionColor;
            Text.color = factionColor;
        }
        else
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    static Vector3[] screenSpaceCorners;
    Rect RendererBoundsInScreenSpace(Renderer r)
    {
        // This is the space occupied by the object's visuals
        // in WORLD space.
        Bounds bigBounds = r.bounds;

        if (screenSpaceCorners == null)
            screenSpaceCorners = new Vector3[8];
        

        // For each of the 8 corners of our renderer's world space bounding box,
        // convert those corners into screen space.
        screenSpaceCorners[0] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
        screenSpaceCorners[1] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
        screenSpaceCorners[2] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
        screenSpaceCorners[3] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
        screenSpaceCorners[4] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
        screenSpaceCorners[5] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));
        screenSpaceCorners[6] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z));
        screenSpaceCorners[7] = Camera.WorldToScreenPoint(new Vector3(bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z));

        // Now find the min/max X & Y of these screen space corners.
        float min_x = screenSpaceCorners[0].x;
        float min_y = screenSpaceCorners[0].y;
        float max_x = screenSpaceCorners[0].x;
        float max_y = screenSpaceCorners[0].y;

        for (int i = 1; i < 8; i++)
        {
            if (screenSpaceCorners[i].x < min_x)
            {
                min_x = screenSpaceCorners[i].x;
            }
            if (screenSpaceCorners[i].y < min_y)
            {
                min_y = screenSpaceCorners[i].y;
            }
            if (screenSpaceCorners[i].x > max_x)
            {
                max_x = screenSpaceCorners[i].x;
            }
            if (screenSpaceCorners[i].y > max_y)
            {
                max_y = screenSpaceCorners[i].y;
            }
        }

        return Rect.MinMaxRect(min_x, min_y, max_x, max_y);

    }
}

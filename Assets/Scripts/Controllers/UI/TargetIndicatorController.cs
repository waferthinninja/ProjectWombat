using UnityEngine;

namespace Controllers.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class TargetIndicatorController : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        // Use this for initialization
        private void Start()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}
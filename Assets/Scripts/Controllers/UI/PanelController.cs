using Model.UI;
using UnityEngine;

namespace Controllers.UI
{
    public class PanelController : MonoBehaviour
    {
        private Vector3 _basePosition;

        private float _currentPosition;
        private readonly float _speed = 1500f;
        private float _targetPosition;
        public bool Active;

        public PanelDirection Direction;
        public float Height;
        public float Width;

        // Use this for initialization
        private void Start()
        {
            _basePosition = transform.localPosition;
            SetActive(Active);
            _currentPosition = _targetPosition;
        }

        // Update is called once per frame
        private void Update()
        {
            MoveTowardsTarget();
        }

        private void MoveTowardsTarget()
        {
            if (_currentPosition < _targetPosition)
            {
                _currentPosition += _speed * Time.deltaTime;
                if (_currentPosition > _targetPosition) _currentPosition = _targetPosition;
            }
            else if (_currentPosition > _targetPosition)
            {
                _currentPosition -= _speed * Time.deltaTime;
                if (_currentPosition < _targetPosition) _currentPosition = _targetPosition;
            }

            SetPosition();
        }

        private void SetPosition()
        {
            if (Direction == PanelDirection.TopDown)
                transform.localPosition = new Vector3(0, -_currentPosition) + _basePosition;
            else if (Direction == PanelDirection.BottomUp)
                transform.localPosition = new Vector3(0, _currentPosition) + _basePosition;
            else if (Direction == PanelDirection.LeftToRight)
                transform.localPosition = new Vector3(_currentPosition, 0) + _basePosition;
            else if (Direction == PanelDirection.RightToLeft)
                transform.localPosition = new Vector3(-_currentPosition, 0) + _basePosition;
        }

        public void SetActive(bool active)
        {
            Active = active;
            var size = Direction == PanelDirection.TopDown || Direction == PanelDirection.BottomUp ? Height : Width;

            _targetPosition = active ? 0 : -size;
        }
    }
}
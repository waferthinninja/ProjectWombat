using System;
using Controllers;
using Managers;
using UnityEngine;

namespace Model
{
    public class SimulationPositionController : PositionController
    {
        private readonly Transform _transform; 
        private readonly Vector3[] _positionStore;
        private readonly Quaternion[] _rotationStore;
        
        public SimulationPositionController(Transform transform, PropulsionController _mob, Vector3[] positionStore, Quaternion[] rotationStore)
        {
            _transform = transform;
            _positionStore = positionStore;
            _rotationStore = rotationStore;
        }

        public override void UpdatePosition()
        {
            // apply proportion of the turn
            _transform.transform.Rotate(0, _mob.TurnProportion * _mob.MaxTurn * Time.fixedDeltaTime / GameManager.TURN_LENGTH, 0);

            // move forward in the new direction
            _transform.transform.Translate(Vector3.forward * _mob.GetSpeed() * Time.fixedDeltaTime);
            
            // store the new position
            
        }

        public override void ResetPosition()
        {
            _transform.transform.position = _positionStore[0];
            _transform.transform.rotation = _rotationStore[0];
        }

    }
}
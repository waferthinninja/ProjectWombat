using Controllers;
using Managers;
using UnityEngine;

namespace Model
{
    public class ReplayPositionController : PositionController
    {
        private readonly MobileObjectController _mob;
        
        public ReplayPositionController(MobileObjectController mob)
        {
            _mob = mob;
        }

        public override void UpdatePosition()
        {
            // apply proportion of the turn
            _mob.transform.Rotate(0, _mob.TurnProportion * _mob.MaxTurn * Time.fixedDeltaTime / GameManager.TURN_LENGTH, 0);

            // move forward in the new direction
            _mob.transform.Translate(Vector3.forward * _mob.GetSpeed() * Time.fixedDeltaTime);
        }

        public override void ResetPosition()
        {
            _mob.transform.position = _mob.GetStartPosition();
            _mob.transform.rotation = _mob.GetStartRotation();
        }
        
    }
}
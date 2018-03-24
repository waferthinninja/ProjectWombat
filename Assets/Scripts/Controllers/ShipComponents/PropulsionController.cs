namespace Controllers.ShipComponents
{
    public abstract class PropulsionController
    {
        private float _speedAtStart;
        public float Acceleration;
        public float Deceleration;
        public float MaxSpeed;
        public float MaxTurn;

        // current settings
        public float SpeedProportion { get; private set; }
        public float TurnProportion { get; private set; }

        public void Awake()
        {
            SpeedProportion = 0.5f;
        }

        
        
    }
}
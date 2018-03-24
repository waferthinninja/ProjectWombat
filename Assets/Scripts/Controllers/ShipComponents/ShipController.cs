using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.UI;
using Managers;
using Model;
using Model.Enums;
using Model.Structs;
using UnityEngine;

namespace Controllers.ShipComponents
{
    [RequireComponent(typeof(MobileObjectController))]
    public class ShipController : MonoBehaviour
    {
        private float _hullPointsAtStart;

        private MobileObjectController _mob;

        private Action _onDeath;
        private bool _showPath;
        public Faction Faction;
        public float MaxHullPoints;
        public string Name;
        public OffscreenIndicatorController OffscreenIndicator;
        public LineRenderer ProjectedPath;

        public Vector3[] ProjectedPositions;
        public Vector3[] ProjectedPositionsForLine;
        public Quaternion[] ProjectedRotations;
        public float HullPoints { get; private set; }

        public PowerController Power { get; private set; }
        public List<ShieldController> Shields { get; private set; }
        public List<WeaponController> Weapons { get; private set; }
        public List<ShipSectionController> ShipSections { get; private set; }

        public bool IsDying { get; private set; }

        public void RegisterOnDeath(Action action)
        {
            _onDeath += action;
        }

        public void UnregisterOnDeath(Action action)
        {
            _onDeath -= action;
        }

        // Use this for initialization
        public void Start()
        {
            _mob = GetComponent<MobileObjectController>();

            HullPoints = MaxHullPoints;
            _hullPointsAtStart = HullPoints;

            GameManager.Instance.RegisterOnStartOfPlanning(OnStartOfPlanning);
            GameManager.Instance.RegisterOnStartOfOutcome(OnStartOfOutcome);
            GameManager.Instance.RegisterOnResetToStart(OnResetToStart);
            GameManager.Instance.RegisterOnEndOfTurn(OnEndOfTurn);

            InitialiseProjections();
            InitialisePower();
            InitialiseShields();
            InitialiseWeapons();
            InitialiseShipSections();
        }

        // Update is called once per frame
        private void Update()
        {
            //Debug.Log(transform.rotation.ToString());
        }

        public void ActivateComponents()
        {
            foreach (var s in Shields) s.enabled = true;
        }

        private void InitialiseShipSections()
        {
            ShipSections = new List<ShipSectionController>();
            foreach (Transform t in transform)
            {
                var shipSection = t.GetComponent<ShipSectionController>();
                if (shipSection != null) ShipSections.Add(shipSection);
            }
        }

        internal void InitializeFromStruct(Ship ship, ChassisType type)
        {
            Name = ship.Name;
            transform.position = new Vector3(ship.PosX, ship.PosY, ship.PosZ);
            transform.rotation = Quaternion.Euler(ship.RotX, ship.RotY, ship.RotZ);
            _mob = GetComponent<MobileObjectController>();
            _mob.Acceleration = type.Acceleration;
            _mob.Deceleration = type.Deceleration;
            _mob.MaxSpeed = type.MaxSpeed;
            _mob.SetStartSpeed(ship.CurrentSpeed);
            _mob.SetStartPosition(transform.position, transform.rotation);
            InitialiseProjections();
            InitialisePower();
            SetTurn(0f);
            SetSpeed(0.5f);
            Faction = ship.Faction == "Friendly" ? Faction.Friendly : Faction.Enemy;
        }

        private void InitialiseWeapons()
        {
            Weapons = transform.GetComponentsInChildren<WeaponController>().ToList();


            //foreach (Transform t in transform)
            //{
            //    if (t.GetComponent<HardpointController>() != null)
            //    {
            //        foreach (Transform t1 in t)
            //        {
            //            var weapon = t1.GetComponent<WeaponController>();
            //            if (weapon != null)
            //            {
            //                Weapons.Add(weapon);
            //            }
            //        }
            //    }
            //}
        }

        private void InitialiseShields()
        {
            Shields = transform.GetComponentsInChildren<ShieldController>().ToList();
            //Shields = new List<ShieldController>();
            //foreach (Transform t in transform)
            //{
            //    if (t.GetComponent<HardpointController>() != null)
            //    {
            //        foreach (Transform t1 in t)
            //        {
            //            var shield = t1.GetComponent<ShieldController>();
            //            if (shield != null)
            //            {
            //                Shields.Add(shield);
            //            }
            //        }
            //    }
            //}
        }

        private void InitialisePower()
        {
            Power = GetComponentInChildren<PowerController>();
        }

        private void InitialiseProjections()
        {
            ProjectedPath.material.SetColor("_TintColor", FactionColors.PathColor[Faction]);
            ProjectedPositions = new Vector3[GameManager.Instance.FramesPerTurn + 1];
            ProjectedPositionsForLine = new Vector3[GameManager.Instance.FramesPerTurn / 10 + 1];
            ProjectedRotations = new Quaternion[GameManager.Instance.FramesPerTurn + 1];

            ProjectedPositions[0] = transform.position;
            ProjectedPositionsForLine[0] = transform.position;
            ProjectedRotations[0] = transform.rotation;

            RecalculateProjections();
        }

        public float ApplyDamage(float damage)
        {
            HullPoints -= damage;
            if (HullPoints <= 0)
            {
                Die();
                return -HullPoints; // return any damage which overkilled
            }

            return 0;
        }

        public void OnStartOfPlanning()
        {
            RecalculateProjections();
            SetShowPath(true);
        }

        public void OnStartOfOutcome()
        {
            SetShowPath(false);
        }

        public void SetShowPath(bool enabled)
        {
            _showPath = enabled;
            ProjectedPath.enabled = enabled;
        }

        public void OnResetToStart()
        {
            if (IsDying)
            {
                IsDying = false;

                // make visible again
                ProjectedPath.enabled = true;
            }

            HullPoints = _hullPointsAtStart;
        }

        public void OnEndOfTurn()
        {
            // actually dispose of ourselves if we died this turn
            if (IsDying) KillSelf();

            _hullPointsAtStart = HullPoints;
            ProjectedPositions[0] = transform.position;
            ProjectedPositionsForLine[0] = transform.position;
            ProjectedRotations[0] = transform.rotation;
            RecalculateProjections();
        }

        public void Die()
        {
            // since we might want to rewind, can't actually destroy the object, just set it to die at end of turn and make it invisible and trigger explosions etc
            IsDying = true;

            foreach (var shield in Shields) shield.Die();
            foreach (var weapon in Weapons) weapon.Die();
            foreach (var shipSection in ShipSections) shipSection.Die();
            ProjectedPath.enabled = false;
            OffscreenIndicator.enabled = false;

            if (_onDeath != null) _onDeath();
        }

        protected void KillSelf()
        {
            // tell child objects to kill themselves (so we unregister any callbacks)
            foreach (var shield in Shields) shield.KillSelf();

            foreach (var weapon in Weapons) weapon.KillSelf();

            foreach (var shipSection in ShipSections) shipSection.KillSelf();

            Destroy(OffscreenIndicator.transform.gameObject);

            GameManager.Instance.UnregisterOnStartOfPlanning(OnStartOfPlanning);
            GameManager.Instance.UnregisterOnEndOfTurn(OnEndOfTurn);

            GameManager.Instance.RemoveFromShipList(this);

            _mob.KillSelf();
        }

        public void RecalculateProjections()
        {
            // start at the beginning of the turn   
            var pos = ProjectedPositions[0];
            var rot = ProjectedRotations[0];

            var turn = GetTurn();
            var speed = GetSpeed();
            // now simulate the turns movement placing a point at the end of each
            for (var t = 1; t <= GameManager.Instance.FramesPerTurn; t++)
            {
                // apply proportion of the turn
                rot *= Quaternion.Euler(Vector3.up * turn * Time.fixedDeltaTime / GameManager.TURN_LENGTH);

                // move forward 1 step in the new direction
                pos += rot * Vector3.forward * Time.fixedDeltaTime * speed;

                ProjectedPositions[t] = pos;
                ProjectedRotations[t] = rot;

                if (t % 10 == 0) ProjectedPositionsForLine[t / 10] = pos;
            }

            // set the line renderer points
            if (ProjectedPath != null)
            {
                ProjectedPath.positionCount = ProjectedPositionsForLine.Length;
                ProjectedPath.SetPositions(ProjectedPositionsForLine);
            }
        }

        public float GetSpeed()
        {
            return _mob.GetSpeed();
        }

        public float GetTurn()
        {
            return _mob.GetTurn();
        }

        public void SetSpeed(float speedProportion)
        {
            _mob.SetSpeed(speedProportion);
            RecalculateProjections();
        }

        public void SetTurn(float turn)
        {
            _mob.SetTurn(turn);
            RecalculateProjections();
        }
    }
}
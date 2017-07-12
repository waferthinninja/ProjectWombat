using UnityEngine;

// struct to hold the details of a collision so it can be replayed
public struct Collision {

    public GameObject GameObjectHit;
    public float TimeHit;
    public Vector3 PositionHit;
    public bool Handled; // flag when done

   
	
}

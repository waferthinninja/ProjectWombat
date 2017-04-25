using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MobileObjectBase {
    
    public override void Start()
    {
        base.Start();

        SetAcceleration(1f);
    }

    // Update is called once per frame
    void Update()
    {
    }
    
}

﻿using System.Collections.Generic;
using UnityEngine;

public static class FactionColors
{
    public static Dictionary<Faction, Color> ShieldColor 
        = new Dictionary<Faction, Color>
        {
            { Faction.Friendly, Color.blue },
            { Faction.Enemy, Color.red }
        };
}

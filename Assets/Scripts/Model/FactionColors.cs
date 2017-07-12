using System.Collections.Generic;
using UnityEngine;

public static class FactionColors
{
    public static Dictionary<Faction, Color> IconColor
    = new Dictionary<Faction, Color>
    {
        { Faction.Friendly, Color.cyan },
        { Faction.Enemy, new Color(0.9f, 0.4f, 0.2f) }
    };

    public static Dictionary<Faction, Color> ShieldColor 
    = new Dictionary<Faction, Color>
    {
        { Faction.Friendly, Color.cyan },
        { Faction.Enemy, new Color(0.9f, 0.4f, 0.2f) }
    };

    public static Dictionary<Faction, Color> LaserColor
    = new Dictionary<Faction, Color>
    {
        { Faction.Friendly, new Color(0f, 1f, 1f, 0.5f) },
        { Faction.Enemy, new Color(1f, 0f, 0f, 0.5f) }
    };

    public static Dictionary<Faction, Color> PathColor
    = new Dictionary<Faction, Color>
    {
        { Faction.Friendly, new Color(0f, 1f, 1f, 0.5f) },
        { Faction.Enemy, new Color(1f, 0f, 0f, 0.5f) }
    };
}

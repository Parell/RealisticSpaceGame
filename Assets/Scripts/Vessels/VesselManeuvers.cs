using System.Collections.Generic;
using UnityEngine;

public class VesselManeuvers : MonoBehaviour
{
    public bool warpToNextBurn;
    public List<Maneuver> maneuvers;
    public float totalDeltaV;
    public Body referanceFrame;
}

[System.Serializable]
public class Maneuver
{
    //Tanget, Perpendicular, Fixed
    public float startTime;
    public float burnTime;
    public float acceleration;

    public Maneuver(Maneuver maneuver)
    {
        startTime = maneuver.startTime;
        burnTime = maneuver.burnTime;
    }
}

using UnityEngine;

public class VesselManeuvers : MonoBehaviour
{
    public bool warpToNextBurn;
    public Maneuver maneuvers;
    public float totalDeltaV;
    public Body referanceFrame;
    public TimeUI timeUI;
    public double precent;

    private void Update()
    {
        TimeUI.WarpTo(maneuvers.startTime);
    }
}

[System.Serializable]
public class Maneuver
{
    //Tanget, Perpendicular, Fixed
    public Vector3 deltaV;
    public double startTime;
    public float burnTime;
    public float acceleration;

    public Maneuver(Maneuver maneuver)
    {
        startTime = maneuver.startTime;
        burnTime = maneuver.burnTime;
    }
}

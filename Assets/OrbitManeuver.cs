using System.Collections.Generic;
using UnityEngine;

public class OrbitManeuver : MonoBehaviour
{
    public List<Maneuver> maneuvers;

    private void FixedUpdate()
    {
        // if (maneuver.deltaV == Vector3.zero && orbitBody.keplerian.SMA > 0)
        // {
        //     orbitController.steps = (int)(((orbitBody.keplerian.T) / 2) / orbitController.stepSize);
        // }
        // else if (maneuver.deltaV != Vector3.zero && orbitBody.keplerian.SMA >= 0)
        // {
        //     orbitController.steps = (int)(((maneuver.startTime + predictionLength) / 2) / orbitController.stepSize);
        // }

        //for (int i = 0; i < maneuvers.Count; i++)
        //{
        //    if (maneuvers[i].startTime > 0)
        //    {
        //        maneuvers[i].startTime -= Time.fixedDeltaTime * SimulationController.Instance.timeScale;
        //    }
        //}

        //if (maneuvers.Count > 0)
        //{
        //    if (maneuvers[0].startTime <= 0)
        //    {
        //        if (maneuvers[0].duration > 0)
        //        {
        //            OrbitController.Instance.orbits[0].AddForce(maneuvers[0].direction.normalized, maneuvers[0].acceleration, Time.fixedDeltaTime * OrbitController.Instance.timeScale);

        //            maneuvers[0].duration -= Time.fixedDeltaTime * OrbitController.Instance.timeScale;
        //        }
        //        else if (OrbitController.Instance.universalTime > (maneuvers[0].startTime + maneuvers[0].duration))
        //        {
        //            maneuvers.Remove(maneuvers[0]);
        //        }
        //    }
        //}
    }
}

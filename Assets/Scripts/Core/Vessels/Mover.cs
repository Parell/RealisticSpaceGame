using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Vessel), typeof(Body))]
public class Mover : MonoBehaviour
{
    [SerializeField]
    private Thruster[] thrusters;
    //[SerializeField]
    //private int forwardThrusters;
    //[SerializeField]
    //private int backwardThrusters;
    //[SerializeField]
    //private int leftThrusters;
    //[SerializeField]
    //private int rightThrusters;
    //[SerializeField]
    //private int upThrusters;
    //[SerializeField]
    //private int downThrusters;
    //[SerializeField]
    //private int mainThrusters;
    private Vessel vessel;
    private Body body;

    private void OnEnable()
    {
        vessel = GetComponent<Vessel>();
        body = GetComponent<Body>();

        thrusters = GetComponentsInChildren<Thruster>();

        //foreach (Thruster thruster in thrusters)
        //{
        //    if (thruster.mainThruster)
        //    {
        //        mainThrusters++;
        //    }
        //    else if (thruster.force.z >= 0.001f)
        //    {
        //        forwardThrusters++;
        //        continue;
        //    }
        //    else if (thruster.force.z <= -0.001f)
        //    {
        //        backwardThrusters++;
        //        continue;
        //    }
        //    else if (thruster.force.x <= -0.001f)
        //    {
        //        leftThrusters++;
        //        continue;
        //    }
        //    else if (thruster.force.x >= 0.001f)
        //    {
        //        rightThrusters++;
        //        continue;
        //    }
        //    else if (thruster.force.y >= 0.001f)
        //    {
        //        upThrusters++;
        //        continue;
        //    }
        //    else if (thruster.force.y <= -0.001f)
        //    {
        //        backwardThrusters++;
        //        continue;
        //    }
        //}
    }

    private void Update()
    {
        HandleThrusters();
    }

    private void FixedUpdate()
    {
        body.AddForce(vessel.moveInputs * vessel.vesselPerformance.thrusterForce, SimulationController.Instance.deltaTime);

        if (vessel.mainburn)
        {
            body.AddForce(Vector3.forward * vessel.vesselPerformance.mainThrusterForce, SimulationController.Instance.deltaTime);
            vessel.BurnFuel(vessel.vesselPerformance.mainThrusterMassFlowRate, SimulationController.Instance.deltaTime);
        }
    }

    private void HandleThrusters()
    {
        foreach (Thruster thruster in thrusters)
        {
            if (thruster.mainThruster)
            {
                if (vessel.mainburn == true)
                {
                    thruster.StartThruster();
                    continue;
                }
                else
                {
                    thruster.StopThruster();
                }
            }
            if (thruster.force.z != 0 && !thruster.mainThruster)
            {
                if (vessel.moveInputs.z >= 0.001f && thruster.force.z >= 0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else if (vessel.moveInputs.z <= -0.001f && thruster.force.z <= -0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else
                {
                    thruster.StopThruster();
                }
            }
            else if (thruster.force.x != 0)
            {
                if (vessel.moveInputs.x >= 0.001f && thruster.force.x <= -0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else if (vessel.moveInputs.x <= -0.001f && thruster.force.x >= 0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else
                {
                    thruster.StopThruster();
                }
            }
            else if (thruster.force.y != 0)
            {
                if (vessel.moveInputs.y >= 0.001f && thruster.force.y >= 0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else if (vessel.moveInputs.y <= -0.001f && thruster.force.y <= -0.001f)
                {
                    thruster.StartThruster();
                    continue;
                }
                else
                {
                    thruster.StopThruster();
                }
            }
        }
    }

    //public Hashtable torques;
    //public Hashtable forces;

    //void Start()
    //{
    //    torques = new Hashtable();
    //    forces = new Hashtable();
    //}

    //public void AddTorque(string identifier, Vector3 torque)
    //{
    //    if (torques.ContainsKey(identifier)) return;
    //    torques.Add(identifier, torque);
    //}

    //public void RemoveTorque(string identifier)
    //{
    //    if (!torques.ContainsKey(identifier)) return;
    //    torques.Remove(identifier);
    //}

    //public void AddForce(string identifier, Vector3 force)
    //{
    //    if (forces.ContainsKey(identifier)) return;
    //    forces.Add(identifier, force);
    //}

    //public void RemoveForce(string identifier)
    //{
    //    if (!forces.ContainsKey(identifier)) return;
    //    forces.Remove(identifier);
    //}

    //void HandleTorques()
    //{
    //    foreach (DictionaryEntry entry in torques)
    //    {
    //        Vector3 torque = (Vector3)entry.Value;
    //        //rigidbody.AddRelativeTorque(torque.normalized);
    //    }
    //}

    //void HandleForces()
    //{
    //    foreach (DictionaryEntry entry in forces)
    //    {
    //        Vector3 force = (Vector3)entry.Value;
    //        //rigidbody.AddRelativeForce(force);
    //    }
    //}
}

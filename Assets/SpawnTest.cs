using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public Body prefab;
    public Vector3d vel;
    public Vector3d pos;
    public int num;

    void Start()
    {
        for (int i = 0; i < num; i++)
        {
            SimulationController.Instance.AddBody(Instantiate(prefab), 10, vel, pos);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavballUI : MonoBehaviour
{
    public Transform navBallGimbal;
    public Transform navBall;
    public Transform vessel;
    public Transform centerBody;

    private void Update()
    {
        /*
            navBall must be a child of navBallGimbal
            Place the mesh inside navBall and give the mesh a default rotation offset to suit your need
            My mesh is rotated x0 y 90 z 90

            NavBall System
            -> NavBall Gimbal
            ->-> NavBall
            ->->-> NavBall Mesh
            -> NavBall Camera Pivot
            ->-> NavBall Camera
            ->->-> Level Indicator

            */

        // Rotate inner navball to face the player
        Quaternion faceShipBody = Quaternion.LookRotation(vessel.position - centerBody.position, centerBody.up);
        faceShipBody = Quaternion.Inverse(faceShipBody);
        faceShipBody.z = -faceShipBody.z;
        navBall.localRotation = Quaternion.Slerp(navBall.localRotation, faceShipBody, 0.15f);

        // Rotate the parent (gimbal) navball to the ship rotation
        Quaternion rotation = vessel.rotation;
        rotation.z = -rotation.z;
        navBallGimbal.localRotation = Quaternion.Slerp(navBallGimbal.localRotation, rotation, 0.15f);
    }
}

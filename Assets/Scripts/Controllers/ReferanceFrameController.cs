using System.Collections.Generic;
using UnityEngine;

public class ReferanceFrameController : MonoBehaviour
{
    public static ReferanceFrameController Instance;
    public int originThreshold = 5000;
    [SerializeField]
    private Transform localCamera;
    [SerializeField]
    private Transform scaledCamera;
    public Vector3d localPosition;
    public Vector3d localOriginPosition;
    public Vector3d scaledPosition;
    public Vector3d scaledOriginPosition;
    [SerializeField]
    private List<Transform> localTransforms;
    [SerializeField]
    private List<Transform> scaledTransforms;

    private void Start()
    {
        Instance = this;

        RegisterTransform(localCamera);
        RegisterTransformScaled(scaledCamera);
    }

    private void Update()
    {
        localPosition = (Vector3d)localCamera.position + localOriginPosition;
        scaledPosition = (Vector3d)scaledCamera.position + scaledOriginPosition;

        if (localCamera.position.magnitude > originThreshold)
        {
            MoveOrigin(localCamera.position);
        }

        if (scaledCamera.position.magnitude > originThreshold)
        {
            MoveOriginScaled(scaledCamera.position);
        }
    }

    private void MoveOrigin(Vector3 delta)
    {
        foreach (Transform target in localTransforms)
        {
            target.position -= delta;
        }

        localOriginPosition += (Vector3d)delta;
    }

    private void MoveOriginScaled(Vector3 delta)
    {
        foreach (Transform target in scaledTransforms)
        {
            target.position -= delta;
        }

        scaledOriginPosition += (Vector3d)delta;
    }

    public void RegisterTransform(Transform target)
    {
        localTransforms.Add(target);
    }

    public void RegisterTransformScaled(Transform target)
    {
        scaledTransforms.Add(target);
    }
}

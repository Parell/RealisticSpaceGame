using UnityEngine;

// Keeps local camera inside a threshold by moving the target to the world origin and other objects in the targets negitive delta
public class ReferanceFrameController : MonoBehaviour
{
    public static ReferanceFrameController Instance;

    [SerializeField] private float targetThreshold = 10;
    [SerializeField] private Transform localCamera; // Must be parent object
    [SerializeField] private Transform scaledCamera;
    public Vector3d localPosition { get; private set; }
    public Vector3d scaledPosition { get; private set; }
    public Vector3d originPosition { get; private set; }
    public Vector3d scaledOriginPosition { get; private set; }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("More then one instance: " + name);
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        // Local space objects
        localPosition = (Vector3d)localCamera.position + originPosition;

        if (localCamera.position.magnitude > targetThreshold)
        {
            originPosition += (Vector3d)localCamera.position;

            localCamera.position -= localCamera.position;
        }

        // Scaled space objects
        scaledPosition = (Vector3d)scaledCamera.position + scaledOriginPosition;

        if (scaledCamera.position.magnitude > targetThreshold)
        {
            scaledOriginPosition += (Vector3d)scaledCamera.position;
        }
    }
}

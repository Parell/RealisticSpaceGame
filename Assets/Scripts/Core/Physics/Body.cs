using UnityEngine;

public class Body : MonoBehaviour
{
    [HideInInspector]
    public int index;
    public double mass = 1d;
    public Vector3d position;
    public Vector3d velocity;
    [HideInInspector]
    public Vector3d force;
    public Transform scaledTransform;

    [ContextMenu("Generate Scaled Object")]
    public void GenerateScaledObject()
    {
        if (scaledTransform)
        {
            DestroyImmediate(scaledTransform.gameObject);
        }

        scaledTransform = new GameObject(name).transform;
        scaledTransform.localPosition = (Vector3)(position / Constant.SCALE);
        scaledTransform.localRotation = Quaternion.identity;
        scaledTransform.parent = GameObject.Find("Scaled").transform;
        scaledTransform.localScale = Vector3.one / Constant.SCALE; // meh
        scaledTransform.gameObject.layer = 6;
        scaledTransform.gameObject.AddComponent<MeshFilter>();
        scaledTransform.gameObject.AddComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        transform.position = (Vector3)(position - ReferanceFrameController.Instance.originPosition);
    }

    public void AddForce(Vector3 force, double deltaTime)
    {
        SimulationController.Instance.bodyData[index].velocity += (Vector3d)transform.TransformVector(force) / (mass / deltaTime);
    }
}

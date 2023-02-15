using UnityEngine;

public class ScaledCamera : MonoBehaviour
{
    [SerializeField] private Camera localCamera;
    [SerializeField] private float unscaledFarClipPlane = 1e14f;
    [SerializeField] private float nearClipOffset = 1f;
    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = (Vector3)(ReferanceFrameController.Instance.localPosition * Constant.INVERSE_SCALE - ReferanceFrameController.Instance.scaledOriginPosition);
        transform.rotation = localCamera.transform.rotation;

        thisCamera.nearClipPlane = localCamera.farClipPlane * nearClipOffset * Constant.INVERSE_SCALE;
        thisCamera.farClipPlane = unscaledFarClipPlane * Constant.INVERSE_SCALE;
    }
}

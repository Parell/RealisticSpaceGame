using UnityEngine;

public class ScaledCamera : MonoBehaviour
{
    [SerializeField]
    private Camera localCamera;
    [SerializeField]
    private float unscaledFarClipPlane = 1e14f;
    [SerializeField]
    private float nearClipOffset = 1f;
    private Camera thisCamera;

    private void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        transform.position = (ReferanceFrameController.Instance.localPosition.ToVector3() / Constant.SCALE) - ReferanceFrameController.Instance.scaledOriginPosition.ToVector3();
        transform.rotation = localCamera.transform.rotation;

        thisCamera.nearClipPlane = localCamera.farClipPlane * nearClipOffset / Constant.SCALE;
        thisCamera.farClipPlane = unscaledFarClipPlane / Constant.SCALE;
    }
}

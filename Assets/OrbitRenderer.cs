using UnityEngine;

[ExecuteAlways]
public class OrbitRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Color startColor = Color.white;
    public Color endColor = Color.white;
    public Material material;
    public float width;
    public AnimationCurve curve;
    public float num;

    private void Update()
    {
        width = curve.Evaluate(num);

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.widthMultiplier = width;
        lineRenderer.material = material;
    }
}

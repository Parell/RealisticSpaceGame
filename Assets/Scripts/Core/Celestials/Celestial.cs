using UnityEditor;
using UnityEngine;

public class Celestial : MonoBehaviour
{
    [SerializeField]
    private float radius = 1f;
    [SerializeField]
    private float surfaceGravity = 1f;
    [SerializeField]
    private string path = "Assets/";
    [SerializeField]
    private Material material;
    [SerializeField]
    private Material scaledMaterial;

    private void Start()
    {
        CalculateMass();
    }

    [ContextMenu("Calculate Mass")]
    private void CalculateMass()
    {
        GetComponent<Body>().mass = surfaceGravity * (radius * radius) / Constant.G;
    }

# if UNITY_EDITOR
    [ContextMenu("Generate Celestial")]
    private void GenerateCelestial()
    {
        Body body = GetComponent<Body>();

        body.GenerateScaledObject();

        ExportMesh();
    }

    [ContextMenu("Export Mesh")]
    private void ExportMesh()
    {
        MeshFilter[] meshFilters = transform.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;

            i++;
        }

        Mesh mesh = new Mesh();

        mesh.CombineMeshes(combine);

        SaveMesh(mesh, gameObject.name, true, true);

        var scaledObject = GetComponent<Body>().scaledTransform;

        if (scaledObject.GetComponent<MeshFilter>() && scaledObject.GetComponent<MeshRenderer>())
        {
            scaledObject.GetComponent<MeshFilter>().mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/" + name + ".asset", typeof(Mesh));
            scaledObject.GetComponent<MeshRenderer>().material = scaledMaterial;
        }
        else
        {
            scaledObject.gameObject.AddComponent<MeshFilter>().mesh = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/" + name + ".asset", typeof(Mesh));
            scaledObject.gameObject.AddComponent<MeshRenderer>().material = scaledMaterial;
        }
    }

    public void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string fullPath = path + name + ".asset";

        Mesh meshToSave = makeNewInstance ? Instantiate(mesh) : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, fullPath);
        AssetDatabase.SaveAssets();
    }
#endif
}

using UnityEngine;

[System.Serializable]
public class QuadSphereData
{
    public float radius;
    public float surfaceGravity;
    [Space]
    public int StartingSubdivision;
    public float[] SubdivisionDistances;
    [Space]
    public Material SphereMaterial;
    [Space]
    public bool UseNoiseForElevation;
    public FastNoise.NoiseType NoiseType;
    public bool SmoothNegativeElevations;
    public int NoiseSeed;
    public float StartingNoiseFrequency;
    public float StartingNoiseAmplitude;
}

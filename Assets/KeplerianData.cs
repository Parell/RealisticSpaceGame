using UnityEngine;

[System.Serializable]
public class KeplerianData : MonoBehaviour
{
    public Body body;
    public Body parent;
    public double semiMajorAxis;
    public double eccentricity;
    public double inclination;
    public double argumentOfPeriapsis;
    public double longitudeAscendingNode;
    public double trueAnomaly;
    public double eccentricityAnomaly;
    public double meanAnomaly;
    public double period;

    private void Update()
    {
        CartesianToKeplerian(body.velocity, body.position);
    }

    public void CartesianToKeplerian(Vector3d velocity, Vector3d position)
    {
        if (parent == null) { return; }

        double mu = parent.mass * Constant.G;

        Vector3d r_vec = position - parent.position;
        Vector3d v_vec = velocity - parent.velocity;

        Vector3d hVec = Vector3d.Cross(r_vec, v_vec);
        Vector3d eVec = (Vector3d.Cross(v_vec, hVec) / mu) - (r_vec / r_vec.magnitude);

        var h = hVec.magnitude;
        eccentricity = eVec.magnitude;

        var n = new Vector3d(-hVec.x, hVec.y, 0);

        inclination = Mathd.Acos(hVec.z / h) * (180 / Mathd.PI);

        if (eVec.z >= 0)
        {
            argumentOfPeriapsis = Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * eccentricity);
        }
        else if (eVec.z < 0)
        {
            argumentOfPeriapsis = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(n, eVec) / n.magnitude * eccentricity);
        }

        if (n.x >= 0)
        {
            longitudeAscendingNode = Mathd.Acos(n.y / n.magnitude);
        }
        else if (n.x < 0)
        {
            longitudeAscendingNode = (2 * Mathd.PI) - Mathd.Acos(n.y / n.magnitude);
        }

        if (h >= 0)
        {
            trueAnomaly = Mathd.Acos(Vector3d.Dot(eVec, r_vec) / (eccentricity * r_vec.magnitude));
        }
        else if (h < 0)
        {
            trueAnomaly = (2 * Mathd.PI) - Mathd.Acos(Vector3d.Dot(eVec, r_vec) / (eccentricity * r_vec.magnitude));
        }

        eccentricityAnomaly = Mathd.Atan2(Mathd.Tan(trueAnomaly / 2), Mathd.Sqrt((1 + eccentricity) / (1 - eccentricity)));

        meanAnomaly = eccentricityAnomaly - eccentricity * Mathd.Sin(eccentricityAnomaly);

        eccentricityAnomaly *= (180 / Mathd.PI);
        meanAnomaly *= (180 / Mathd.PI);
        trueAnomaly *= (180 / Mathd.PI);
        longitudeAscendingNode *= (180 / Mathd.PI);
        argumentOfPeriapsis *= (180 / Mathd.PI);

        semiMajorAxis = 1 / ((2 / r_vec.magnitude) - (Mathd.Pow(v_vec.magnitude, 2) / mu));

        double r = Vector3d.Distance(r_vec, parent.position);

        period = Mathd.Sqrt(4 * (Mathd.PI * Mathd.PI) / mu * Mathd.Pow(semiMajorAxis, 3));
    }
}

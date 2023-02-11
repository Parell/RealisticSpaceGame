using System.Collections.Generic;
using UnityEngine;

public enum Integrator
{
    Euler,
    BackwardEuler,
    Ralston,
    RK2_Midpoint,
    RK4,
    RKF54
}

[ExecuteAlways]
public class SimulationController : MonoBehaviour
{
    public static SimulationController Instance;
    public Integrator integrator;
    public double universalTime;
    public int timeScale = 1;
    public float deltaTime;
    [Space]
    private float predictionTimer;
    public float plotLength = 100f;
    public float stepSize = 1f;
    public int steps;
    public Body referenceFrame;
    public ReferanceFrameController endlessController;
    public float predictionInterval = 1f;
    [Space]
    public List<Body> bodies;
    public BodyData[] bodyData;
    public BodyData[] virtualBodyData;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        Instance = this;

        Time.fixedDeltaTime = deltaTime;

        FindBodies();
    }

    public void FindBodies()
    {
        bodies = FindObjectsOfType<Body>().ToList();

        UpdateIndex();
    }

    public void AddBody(Body body)
    {
        bodies.Add(body);

        UpdateIndex();
    }

    public void RemoveBody(Body body)
    {
        bodies.Remove(body);
    }

    public void UpdateIndex()
    {
        bodyData = new BodyData[bodies.Count];

        for (int i = 0; i < bodies.Count; i++)
        {
            bodies[i].index = i;

            bodyData[i] = new BodyData(i, bodies[i].mass, bodies[i].velocity, bodies[i].position);
        }
    }

    private void Update()
    {
        steps = Mathf.RoundToInt(plotLength / stepSize);

        if (Application.isPlaying)
        {
            predictionTimer -= Time.deltaTime;
            if (predictionTimer < 0)
            {
                predictionTimer = predictionInterval;
                UpdateOrbits();
            }
        }
        else
        {
            FindBodies();
            UpdateOrbits();
        }
    }

    private void FixedUpdate()
    {
        deltaTime = Time.fixedDeltaTime * timeScale;

        for (int i = 0; i < timeScale; i++)
        {
            universalTime += Time.deltaTime;

            bodyData = Propagate(bodyData, Time.fixedDeltaTime);
        }

        for (int j = 0; j < bodyData.Length; j++)
        {
            bodies[j].velocity = bodyData[j].velocity;
            bodies[j].position = bodyData[j].position;
            bodies[j].force = bodyData[j].force;
        }
    }

    private BodyData[] Propagate(BodyData[] bodyData, float stepSize)
    {
        for (int j = 0; j < bodyData.Length; j++)
        {
            BodyData tempBodyData = bodyData[j];

            tempBodyData.ApplyForces(integrator, stepSize, bodyData);

            bodyData[j] = tempBodyData;
        }
        return bodyData;
    }

    private void UpdateOrbits()
    {

        virtualBodyData = new BodyData[bodies.Count];
        Vector3[][] drawPoints = new Vector3[bodies.Count][];

        int referenceFrameIndex = 0;
        Vector3d referenceBodyInitialPosition = Vector3d.zero;

        for (int i = 0; i < bodies.Count; i++)
        {
            virtualBodyData[i] = new BodyData(i, bodies[i].mass, bodies[i].velocity, bodies[i].position);
            drawPoints[i] = new Vector3[steps];

            if (referenceFrame != null && bodies[i] == referenceFrame)
            {
                referenceFrameIndex = i;
                referenceBodyInitialPosition = virtualBodyData[i].position;
            }

            //if (orbits[i].GetComponent<OrbitManeuver>())
            //{
            //    maneuvers = new List<Maneuver>(orbits[i].GetComponent<OrbitManeuver>().maneuvers.Count);

            //    orbits[i].GetComponent<OrbitManeuver>().maneuvers.ForEach((item) =>
            //    {
            //        maneuvers.Add(new Maneuver(item));
            //    });
            //}
        }

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualBodyData[referenceFrameIndex].position : Vector3d.zero;

            virtualBodyData = Propagate(virtualBodyData, stepSize);

            for (int i = 0; i < virtualBodyData.Length; i++)
            {
                if (bodies[i].GetComponent<VesselManeuvers>())
                {
                    var maneuvers = new Maneuver(bodies[i].GetComponent<VesselManeuvers>().maneuvers[0]);

                    //for (int j = 0; j < maneuvers.Count; j++)
                    //{
                    if ((step * stepSize) >= maneuvers.startTime)
                    {
                        if (maneuvers.burnTime > 0)
                        {
                            virtualBodyData[i].AddAcceleration(maneuvers.acceleration, stepSize);

                            maneuvers.burnTime -= stepSize;
                        }
                    }
                    //}
                }

                Vector3d nextPosition = virtualBodyData[i].position;
                if (referenceFrame != null)
                {
                    var referenceFrameOffset = referenceBodyPosition - referenceBodyInitialPosition;
                    nextPosition -= referenceFrameOffset;
                }
                if (referenceFrame != null && i == referenceFrameIndex)
                {
                    nextPosition = referenceBodyInitialPosition;
                }

                if (bodies[i].scaledTransform)
                {
                    drawPoints[i][step] = (Vector3)(nextPosition / Constant.SCALE - endlessController.scaledOriginPosition);
                }
                else
                {
                    drawPoints[i][step] = (Vector3)(nextPosition - endlessController.localOriginPosition);
                }
            }
        }

        for (int bodyIndex = 0; bodyIndex < virtualBodyData.Length; bodyIndex++)
        {
            LineRenderer lineRenderer;

            if (bodies[bodyIndex].scaledTransform)
            {
                lineRenderer = bodies[bodyIndex].scaledTransform.GetComponent<LineRenderer>();
            }
            else
            {
                lineRenderer = bodies[bodyIndex].GetComponent<LineRenderer>();
            }

            if (lineRenderer)
            {
                lineRenderer.positionCount = drawPoints[bodyIndex].Length;
                lineRenderer.SetPositions(drawPoints[bodyIndex]);
            }
        }
    }
}

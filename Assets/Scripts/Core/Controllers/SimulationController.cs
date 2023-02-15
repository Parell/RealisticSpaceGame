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

    public void AddBody(Body body, double mass, Vector3d velocity, Vector3d position) //Broken 
    {
        body.mass = mass;
        body.position = position;
        body.velocity = velocity;
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

            for (int j = 0; j < bodyData.Length; j++)
            {
                transform.position = (Vector3)(bodies[j].position - endlessController.originPosition);

                if (bodies[j].scaledTransform)
                {
                    bodies[j].scaledTransform.position = (Vector3)((bodies[j].position / Constant.SCALE) - endlessController.scaledOriginPosition);
                }
            }
        }
        else
        {
            FindBodies();
            UpdateOrbits();

            for (int j = 0; j < bodyData.Length; j++)
            {
                if (bodies[j].scaledTransform)
                {
                    transform.position = bodies[j].scaledTransform.position * Constant.SCALE - (Vector3)endlessController.scaledOriginPosition;
                    bodies[j].position = (Vector3d)transform.position;
                }
            }
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

            //if (bodies[i].GetComponent<VesselManeuvers>())
            //{
            //    var maneuvers = new List<Maneuver>(bodies[i].GetComponent<VesselManeuvers>().maneuvers.Count);

            //    bodies[i].GetComponent<VesselManeuvers>().maneuvers.ForEach((item) =>
            //    {
            //        maneuvers.Add(new Maneuver(item));
            //    });
            //}
        }
        //double burnTime = bodies[2].GetComponent<VesselManeuvers>().maneuvers[0].burnTime;

        for (int step = 0; step < steps; step++)
        {
            Vector3d referenceBodyPosition = (referenceFrame != null) ? virtualBodyData[referenceFrameIndex].position : Vector3d.zero;

            virtualBodyData = Propagate(virtualBodyData, stepSize);


            for (int i = 0; i < virtualBodyData.Length; i++)
            {
                if (bodies[i].GetComponent<VesselManeuvers>())
                {
                    var maneuver = bodies[i].GetComponent<VesselManeuvers>();

                    //for (int j = 0; j < maneuvers.Count; j++)
                    //{

                    //if ((step * stepSize) >= maneuver.maneuvers[0].startTime)
                    //{
                    //    if (burnTime > 0)
                    //    {
                    //        virtualBodyData[i].AddAcceleration(maneuver.maneuvers[0].acceleration, stepSize, maneuver.maneuvers[0].deltaV);

                    //        burnTime -= stepSize;
                    //    }
                    //}
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
                    drawPoints[i][step] = (Vector3)(nextPosition - endlessController.originPosition);
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

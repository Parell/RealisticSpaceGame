using UnityEngine;

[RequireComponent(typeof(Body))]
public class Vessel : MonoBehaviour
{
    public bool isPlayer;
    [Space]
    public Vector3 lookInputs;
    public float lookMutiplier = 0.1f;
    public float deadZone = 0.15f;
    [Space]
    public Vector3 moveInputs;
    public bool mainburn;
    public float tapSpeed = 0.2f;
    public bool retroburn;
    [Space]
    public VesselPerformance vesselPerformance;
    public double totalDeltaV;
    public double deltaV;
    private float lastTapTime = 0f;
    private Body body;

    private void OnEnable()
    {
        Initialize();
    }

    public void Initialize()
    {
        body = GetComponent<Body>();

        body.mass = vesselPerformance.totalMass;

        vesselPerformance.Calculate();
    }

    private void Update()
    {
        if (isPlayer)
        {
            Inputs();
        }
        transform.Rotate(lookInputs * lookMutiplier);

        CalculateDeltaV();
    }

    private void CalculateDeltaV()
    {
        totalDeltaV = vesselPerformance.mainThrusterIsp * Constant.G0 * Mathd.Log(vesselPerformance.totalMass / vesselPerformance.emptyMass);

        if (body)
        {
            deltaV = totalDeltaV - vesselPerformance.mainThrusterIsp * Constant.G0 * Mathd.Log(vesselPerformance.totalMass / body.mass);
        }
    }

    public void BurnFuel(double massFlowRate, double deltaTime)
    {
        body.mass -= massFlowRate * deltaTime;
    }

    private void Inputs()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveInputs.z = 1;

                if ((Time.time - lastTapTime) < tapSpeed)
                {
                    mainburn = true;

                    moveInputs.z = 0;
                }
                lastTapTime = Time.time;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInputs.z = -1;

            mainburn = false;
        }
        else
        {
            moveInputs.z = 0;

            mainburn = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveInputs.x = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInputs.x = -1;
        }
        else
        {
            moveInputs.x = 0;
        }

        if (Input.GetKey(KeyCode.R))
        {
            moveInputs.y = 1;
        }
        else if (Input.GetKey(KeyCode.F))
        {
            moveInputs.y = -1;
        }
        else
        {
            moveInputs.y = 0;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            lookInputs.z = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            lookInputs.z = -1;
        }
        else
        {
            lookInputs.z = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (retroburn == true)
            {
                retroburn = false;
            }
            else
            {
                retroburn = true;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 mousePosition = Input.mousePosition;

            float yaw = (mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f);
            if (Mathf.Abs(yaw) < deadZone) yaw = yaw / 4;

            float pitch = (mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f);
            if (Mathf.Abs(pitch) < deadZone) pitch = pitch / 4;

            lookInputs = new Vector3(pitch * -1, yaw, lookInputs.z);

            if (retroburn)
            {
                lookInputs.Scale(new Vector3(-1, 1, 1));
            }
        }
        else
        {
            lookInputs = Vector3.zero;
        }
    }
}

[System.Serializable]
public class VesselPerformance
{
    public double totalMass;
    public double emptyMass;
    [Space]
    public float thrusterForce;
    public float mainThrusterForce;
    public double thrusterIsp;
    public double mainThrusterIsp;
    public double thrusterMassFlowRate;
    public double mainThrusterMassFlowRate;

    public void Calculate()
    {
        thrusterForce = (float)(thrusterMassFlowRate * thrusterIsp * Constant.G0);
        mainThrusterForce = (float)(mainThrusterMassFlowRate * mainThrusterIsp * Constant.G0);
    }
}
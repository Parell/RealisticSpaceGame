using UnityEngine;

[System.Serializable]
public class BodyData
{
    public int index;
    public double mass;
    public Vector3d position;
    public Vector3d velocity;
    public Vector3d force;

    public BodyData(int index, double mass, Vector3d velocity, Vector3d position)
    {
        this.index = index;
        this.mass = mass;
        this.velocity = velocity;
        this.position = position;
        force = Vector3d.zero;
    }

    public void ApplyForces(Integrator integrator, float stepSize, BodyData[] bodyData)
    {
        Vector3d gravity(Vector3d position)
        {
            Vector3d acceleration = Vector3d.zero;

            for (int i = 0; i < bodyData.Length; i++)
            {
                if (i == index) { continue; }

                Vector3d r = bodyData[i].position - position;

                acceleration += r * (Constant.G * bodyData[i].mass) / (r.magnitude * r.magnitude * r.magnitude);
            }

            force = acceleration / mass;

            return acceleration;
        }

        Vector3d v(Vector3d position, double stepSize)
        {
            return velocity + gravity(position) * stepSize;
        }

        Vector3d a(Vector3d velocity, double stepSize)
        {
            return gravity(position) + gravity(position + velocity * stepSize) * stepSize;
        }

        switch (integrator)
        {
            case Integrator.Euler:
                {
                    position = position + stepSize * v(position, 0);
                    velocity = velocity + stepSize * a(velocity, 0);
                    break;
                }
            case Integrator.BackwardEuler:
                {
                    velocity = velocity + stepSize * a(velocity, 0);
                    position = position + stepSize * v(position, 0);
                    break;
                }
            case Integrator.Ralston:
                {
                    Vector3d k1, k2;
                    {

                        k1 = a(velocity, 0);
                        k2 = a(velocity + stepSize * (2 / 3) * k1, stepSize * (2 / 3));

                        velocity = velocity + stepSize * (k1 * 0.25 + k2 * 0.75);
                    }
                    {
                        k1 = v(position, 0);
                        k2 = v(position + stepSize * (2 / 3) * k1, stepSize * (2 / 3));

                        position = position + stepSize * (k1 * 0.25 + k2 * 0.75);
                    }
                    break;
                }
            case Integrator.RK2_Midpoint:
                {
                    Vector3d k1, k2;
                    {

                        k1 = a(velocity, 0);
                        k2 = a(velocity + stepSize * 0.5 * k1, stepSize * 0.5);

                        velocity = velocity + stepSize * k2;
                    }
                    {
                        k1 = v(position, 0);
                        k2 = v(position + stepSize * 0.5 * k1, stepSize * 0.5);

                        position = position + stepSize * k2;
                    }
                    break;
                }
            case Integrator.RK4:
                {
                    Vector3d k1, k2, k3, k4;
                    {
                        k1 = a(velocity, 0);
                        k2 = a(velocity + stepSize * 0.5 * k1, stepSize * 0.5);
                        k3 = a(velocity + stepSize * 0.5 * k2, stepSize * 0.5);
                        k4 = a(velocity + stepSize * -k3, stepSize);

                        velocity = velocity + stepSize / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                    }
                    {
                        k1 = v(position, 0);
                        k2 = v(position + stepSize * 0.5 * k1, stepSize * 0.5);
                        k3 = v(position + stepSize * 0.5 * k2, stepSize * 0.5);
                        k4 = v(position + stepSize * -k3, stepSize);

                        position = position + stepSize / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                    }
                    break;
                }
            case Integrator.RKF54:
                {
                    Vector3d k1, k2, k3, k4, k5, k6;
                    {
                        k1 = a(velocity, 0);
                        k2 = a(velocity + stepSize * 0.5 * k1, stepSize * 0.5);
                        k3 = a(velocity + stepSize * 0.5 * k2, stepSize * 0.5);
                        k4 = a(velocity + stepSize * -k3, stepSize);
                        k5 = a(velocity + stepSize * -k3, stepSize);
                        k6 = a(velocity + stepSize * -k3, stepSize);

                        velocity = velocity + stepSize / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                    }
                    {
                        k1 = v(position, 0);
                        k2 = v(position + stepSize * 0.5 * k1, stepSize * 0.5);
                        k3 = v(position + stepSize * 0.5 * k2, stepSize * 0.5);
                        k4 = v(position + stepSize * -k3, stepSize);
                        k5 = a(velocity + stepSize * -k3, stepSize);
                        k6 = a(velocity + stepSize * -k3, stepSize);

                        position = position + stepSize / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
                    }

                    //double truncationError = Mathd.Abs((-1 / 150 * k1 + 0 * k2 + 3 / 100 * k3 + -16 / 75 * k4 + -1 / 20 * k5 + 6 / 25 * k6).magnitude);
                    //float error = 1e-8f;

                    //float nextStepSize = 0.9f * stepSize * Mathf.Pow((error / (float)truncationError), 1 / 5);

                    //if (truncationError > error)
                    //{
                    //    stepSize = nextStepSize;
                    //    // repeat step
                    //}
                    //if (truncationError <= error)
                    //{
                    //    // complete
                    //    stepSize = nextStepSize;
                    //}
                    break;
                }
        }
    }

    public void AddAcceleration(float acceleration, float deltaTime, Vector3 deltaV)
    {
        Vector3 direction = deltaV.normalized;

        velocity += (Vector3d)direction * acceleration * deltaTime;
    }
}

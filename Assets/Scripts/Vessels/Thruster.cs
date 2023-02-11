using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 force;
    public bool mainThruster;
    private ParticleSystem particle;
    private bool isOn;

    private void OnEnable()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void StartThruster()
    {
        if (!isOn)
        {
            particle.Play();
            isOn = true;
        }
    }

    public void StopThruster()
    {
        if (isOn)
        {
            particle.Stop();
            isOn = false;
        }
    }
}

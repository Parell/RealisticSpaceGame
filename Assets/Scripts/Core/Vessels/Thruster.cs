using UnityEngine;

public class Thruster : MonoBehaviour
{
    public Vector3 force;
    public bool mainThruster;
    public GameObject lightObject;
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
            if (lightObject)
            {
                lightObject.SetActive(true);
            }
            particle.Play();
            isOn = true;
        }
    }

    public void StopThruster()
    {
        if (isOn)
        {
            if (lightObject)
            {
                lightObject.SetActive(false);
            }
            particle.Stop();
            isOn = false;
        }
    }
}

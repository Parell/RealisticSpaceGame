using UnityEngine;
using UnityEngine.UI;

public class FpsCounterUI : MonoBehaviour
{
    [SerializeField]
    private float timer, refresh = 0.5f, avgFramerate;
    private string display = "{0} FPS";
    [SerializeField]
    private Text text;
    private string m_Text;

    private void Update()
    {
        text.text = m_Text;

        //Change smoothDeltaTime to deltaTime or fixedDeltaTime to see the difference
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_Text = string.Format(display, avgFramerate.ToString());
    }
}

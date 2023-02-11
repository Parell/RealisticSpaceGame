using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public SimulationController simulationController;
    public int timeScale = 0;
    public int timeIndex;
    public int maxTimeIndex = 6;
    public Text timeText;
    public int[] timeScales;
    public GameObject[] timeVisuals;
    public KeyCode fasterTimeKeyCode = KeyCode.Period;
    public KeyCode slowerTimeKeyCode = KeyCode.Comma;
    public KeyCode stopTimeKeyCode = KeyCode.Slash;

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        timeScale = timeScales[0];
        SetButtons();
    }

    private void Update()
    {
        var totalSecs = (int)simulationController.universalTime;

        var days = totalSecs / (24 * 3600);
        var years = days / 365;
        var hours = totalSecs / 3600;
        var minutes = (totalSecs % 3600) / 60;
        var seconds = totalSecs % 60;

        var timeString = string.Format("{4}y {3}d {0}:{1}:{2}", hours.ToString("D2"), minutes.ToString("D2"), seconds.ToString("D2"), days.ToString("D3"), years.ToString("D3"));

        timeText.text = "UT " + timeString;

        if (Input.GetKeyUp(fasterTimeKeyCode))
        {
            if (timeIndex == maxTimeIndex)
            {
                SetButtons();

                timeScale = timeScales[timeIndex];
            }
            else
            {
                timeIndex++;

                SetButtons();

                timeScale = timeScales[timeIndex];
            }

        }
        else if (Input.GetKeyUp(slowerTimeKeyCode))
        {
            if (timeIndex == 0)
            {
                SetButtons();

                timeScale = timeScales[timeIndex];
            }
            else
            {
                timeIndex--;

                SetButtons();

                timeScale = timeScales[timeIndex];
            }
        }
        else if (Input.GetKeyUp(stopTimeKeyCode))
        {
            timeIndex = 0;

            SetButtons();

            timeScale = timeScales[timeIndex];
        }

        if (timeIndex >= maxTimeIndex)
        {
            timeIndex = maxTimeIndex;
        }

        if (timeIndex < 0)
        {
            timeIndex = 0;
        }

        simulationController.timeScale = timeScale;
    }

    private void SetButtons()
    {
        for (int i = 0; i < timeVisuals.Length; i++)
        {
            if (i <= timeIndex)
            {
                timeVisuals[i].SetActive(true);
            }
            else
            {
                timeVisuals[i].SetActive(false);
            }
        }
    }
}
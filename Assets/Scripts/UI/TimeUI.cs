using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    public SimulationController simulationController;
    public int timeScale = 0;
    public static int timeIndex;
    public Text timeText;
    public int[] timeScales;
    public GameObject[] timeVisuals;
    public KeyCode fasterTimeKeyCode = KeyCode.Period;
    public KeyCode slowerTimeKeyCode = KeyCode.Comma;
    public KeyCode stopTimeKeyCode = KeyCode.Slash;

    private void Start()
    {
        timeScale = timeScales[0];
        SetButtons();
    }

    private void Update()
    {
        var totalSecs = (int)simulationController.universalTime;

        var days = totalSecs / (24 * 3600);
        var years = days / 365;
        var hours = (totalSecs / 3600) - days * 24;
        var minutes = (totalSecs % 3600) / 60;
        var seconds = totalSecs % 60;

        var timeString = string.Format("{4}y {3}d {2}:{1}:{0}", seconds.ToString("D2"), minutes.ToString("D2"), hours.ToString("D2"), days.ToString("D3"), years.ToString("D3"));

        timeText.text = "UT " + timeString;

        var maxTimeIndex = timeScales.Length - 1;

        if (Input.GetKeyUp(fasterTimeKeyCode))
        {
            if (timeIndex == maxTimeIndex)
            {
                SetButtons();
            }
            else
            {
                timeIndex++;

                SetButtons();
            }

        }
        else if (Input.GetKeyUp(slowerTimeKeyCode))
        {
            if (timeIndex == 0)
            {
                SetButtons();
            }
            else
            {
                timeIndex--;

                SetButtons();

            }
        }
        else if (Input.GetKeyUp(stopTimeKeyCode))
        {
            timeIndex = 0;

            SetButtons();
        }

        timeScale = timeScales[timeIndex];
        //timeScale = Mathf.RoundToInt(Mathf.MoveTowards(timeScale, timeScales[timeIndex], 1000 * Time.deltaTime));

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

    public static void WarpTo(double timeToWarpTo)
    {
        var time = SimulationController.Instance.universalTime;

        var timeLeft = timeToWarpTo - time;

        if (timeLeft > 1024)
        {
            timeIndex = 5;
        }
        else if (timeLeft > 512)
        {
            timeIndex = 4;
        }
        else if (timeLeft > 64)
        {
            timeIndex = 3;
        }
        else if (timeLeft > 16)
        {
            timeIndex = 2;
        }
        else if (timeLeft > 4)
        {
            timeIndex = 1;
        }
        else
        {
            timeIndex = 0;
        }
    }
}

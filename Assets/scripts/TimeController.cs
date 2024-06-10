using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void makeTimeDefault()
    {
        Time.timeScale = 1;
    }

    public void makeTime3x()
    {
        Time.timeScale = 3;
    }

    public void makeTime10x()
    {
        Time.timeScale = 10;
    }

    public void makeTime20x()
    {
        Time.timeScale = 20;
    }
}

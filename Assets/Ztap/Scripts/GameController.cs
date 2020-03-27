using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public DataController dataCtrl;
    public LevelController currentLvlCtrl;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (GameController.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
}

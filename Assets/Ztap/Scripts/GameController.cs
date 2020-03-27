using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [HideInInspector]
    public LevelController currentLvlCtrl;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (GameController.instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentLvlCtrl = FindObjectOfType<LevelController>();
        if (DataController.LoadString("startTime") == "" || !DataController.HasKey("startTime"))
        {
            DataController.SaveData("startTime", DateTime.Now.Ticks.ToString());
            currentLvlCtrl.ingameUiCtrl.startTime = DateTime.Now.Ticks;
        }
        if (DataController.LoadString("exitTime") == "" || !DataController.HasKey("exitTime"))
        {
            DataController.SaveData("exitTime", DateTime.Now.Ticks.ToString());
        }
        StartCoroutine(WaitForFirstFrame()); //TODO: koctul etot ybrat'
    }

    IEnumerator WaitForFirstFrame()
    {
        yield return new WaitForEndOfFrame();
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        long exitTime = Convert.ToInt64(DataController.LoadString("exitTime"));
        TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - exitTime);
        int hpLoosed = (int)elapsedSpan.TotalMinutes; //TODO: remake hp loosing

        currentLvlCtrl.players[0].Level = DataController.HasKey("lvl") ? DataController.LoadInt("lvl") : 0;
        currentLvlCtrl.players[0].Exp = DataController.HasKey("exp") ? DataController.LoadInt("exp") : 0;
        currentLvlCtrl.players[0].hp = DataController.HasKey("hp") ? DataController.LoadFloat("hp") : 100;
        currentLvlCtrl.players[0].TakeDamage(hpLoosed);
        currentLvlCtrl.ingameUiCtrl.startTime = Convert.ToInt64(DataController.LoadString("startTime"));
        currentLvlCtrl.ingameUiCtrl.maxHP = currentLvlCtrl.players[0].maxHp;
        currentLvlCtrl.ingameUiCtrl.CurrentHP = currentLvlCtrl.players[0].hp - hpLoosed;
        currentLvlCtrl.ingameUiCtrl.CurrentScore = currentLvlCtrl.players[0].Exp;
    }
    private void SavePlayerData()
    {
        DataController.SaveData("lvl", currentLvlCtrl.players[0].Level);
        DataController.SaveData("exp", currentLvlCtrl.players[0].Exp);
        DataController.SaveData("hp", currentLvlCtrl.players[0].hp);
        DataController.SaveData("exitTime", System.DateTime.Now.Ticks.ToString());
    }
    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            if (currentLvlCtrl.players.Count > 0)
            {
                SavePlayerData();
            }
        }
    }


    private void OnApplicationPause(bool pause)
    {
        if (currentLvlCtrl.players.Count > 0)
        {
            SavePlayerData();
        }
    }

    private void OnApplicationQuit()
    {
        if (currentLvlCtrl.players.Count > 0)
        {
            SavePlayerData();
        }
    }

    public void Restart()
    {
        DataController.DeleteKey("lvl");
        DataController.DeleteKey("exp");
        DataController.DeleteKey("hp");
        DataController.DeleteKey("startTime");
        DataController.DeleteKey("exitTime");

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

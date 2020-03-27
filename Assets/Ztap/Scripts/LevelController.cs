using EventAggregation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public LevelModificator mod;
    public List<EnemyCtrl> enemies = new List<EnemyCtrl>();
    public List<PlayerCharCtrl> players = new List<PlayerCharCtrl>();
    public SpawnController spawner;
    public IngameUIController ingameUiCtrl;



    private void Awake()
    {
        SubscribeToEvents();
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ExitLevel()
    {
        Application.Quit(); //TODO: change to load main menu
    }

    public void Restart()
    {
        GameController.instance.Restart();
    }

    #region Events
    private void SubscribeToEvents()
    {
        EventAggregator.Subscribe<CharacterDeath>(OnCharacterDeath);
    }

    private void UnsubscribeFromEvents()
    {
        EventAggregator.UnsubscribeAll();
    }

    private void OnCharacterDeath(IEventBase eventBase)
    {
        if (eventBase is CharacterDeath e)
        {
            Time.timeScale = 0;
        }
    }
    #endregion
}

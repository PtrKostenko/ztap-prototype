using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EventAggregation;
using Unity.Collections;
using System;

public class IngameUIController : MonoBehaviour
{
    [SerializeField] Canvas CanvasIngame;
    [SerializeField] Canvas CanvasPaused;
    [SerializeField] Canvas CanvasDeath;
    [SerializeField] Image HpBar;
    [SerializeField] TextMeshProUGUI TextHp;
    [SerializeField] TextMeshProUGUI TextScore;
    [SerializeField] TextMeshProUGUI TimeScore;
    [SerializeField] Button ButtonLvlUp;
    [SerializeField] private LevelController levelController;

    [HideInInspector] public float maxHP = 100; //перезадается при старте сцены GameControllerom

    private float currentHP;
    public float CurrentHP
    {
        get
        {
            return currentHP;
        }
        set
        {
            currentHP = value;
            HpBar.fillAmount = currentHP / maxHP;
            TextHp.text = $"{currentHP}/{maxHP}";
        }
    }

    private int currentScore;
    public int CurrentScore {
        get
        {
            return currentScore;
        }
        set
        {
            currentScore = value;
            TextScore.text = $"SCORE: {currentScore}";
        }
    }

    public long startTime;

    private void Awake()
    {
        SubscribeToEvents();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        if (levelController == null)
        {
            if (GameController.instance != null)
                levelController = GameController.instance.currentLvlCtrl;
        }
        CurrentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - startTime);
        //TODO: make survival time
        TimeScore.text = elapsedSpan.ToString(@"hh\:mm\:ss");
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    public void Pause()
    {
        CanvasPaused.gameObject.SetActive(true);
        levelController.PauseGame();
        
    }

    public void Resume()
    {
        CanvasPaused.gameObject.SetActive(false);
        levelController.ResumeGame();
    }

    public void Exit()
    {
        levelController.ExitLevel();
    }

    public void LevelUp()
    {
        levelController.players[0].LevelUp();
    }

    public void Restart()
    {
        levelController.Restart();
    }
    #region Events
    private void SubscribeToEvents()
    {
        EventAggregator.Subscribe<ZombieKilledByPlayersCharacter>(OnZombieKilledByCharacter);
        EventAggregator.Subscribe<ZombieKilledByPlayer>(OnZombieKilledByPlayer);
        EventAggregator.Subscribe<CharacterDamaged>(OnCharacterDamaged);
        EventAggregator.Subscribe<CharacterLevelUp>(OnCharacterLevelUp);
        EventAggregator.Subscribe<CharacterLevelReached>(OnCharacterLevelReached);
        EventAggregator.Subscribe<CharacterDeath>(OnCharacterDeath);
    }

    private void UnsubscribeFromEvents()
    {
        EventAggregator.UnsubscribeAll();
    }

    private void OnZombieKilledByCharacter(IEventBase eventBase)
    {
        if (eventBase is ZombieKilledByPlayersCharacter e)
        {
            if (CurrentHP > 0)
                CurrentScore += e.Exp;
        }
    }
    private void OnZombieKilledByPlayer(IEventBase eventBase)
    {
        if (eventBase is ZombieKilledByPlayer e)
        {
            if (CurrentHP > 0)
                CurrentScore += e.Exp;
        }
    }
    private void OnCharacterDamaged(IEventBase eventBase)
    {
        if (eventBase is CharacterDamaged e)
        {
            CurrentHP -= e.Value;
        }
    }
    private void OnCharacterLevelUp(IEventBase eventBase)
    {
        if (eventBase is CharacterLevelUp e)
        {
            maxHP = e.MaxHp;
            CurrentHP = maxHP;
            ButtonLvlUp.gameObject.SetActive(false);
        }
    }
    private void OnCharacterLevelReached(IEventBase eventBase)
    {
        if (eventBase is CharacterLevelReached e)
        {
            ButtonLvlUp.gameObject.SetActive(true);
        }
    }

    private void OnCharacterDeath(IEventBase eventBase)
    {
        if (eventBase is CharacterDeath e)
        {
            CanvasDeath.gameObject.SetActive(true);
        }
    }
    #endregion

}

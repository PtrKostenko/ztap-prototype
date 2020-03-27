using EventAggregation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerCharEffectsController))]
public class PlayerCharCtrl : ZCharacterController
{
    [Header("Leveling")]
    private int level = 0;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            //TODO: load progression data
            level = value;
            maxHp = 100 + 5 * level;
            hp = maxHp;
            damage = 101 + 2 * level;
            //radius = 0;
            expToNextLvl = 500 + 100 * (int)Mathf.Pow(level, 2);
        }
    }
    private int exp = 0;
    public int Exp {
        get
        {
            return exp;
        }
        set
        {
            exp = value;
            if (exp > expToNextLvl)
                EventAggregator.Publish(new CharacterLevelReached { });
        }
    }
    public int expToNextLvl = 500;


    private Animator _anim;
    private PlayerCharEffectsController _effects;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _effects = GetComponent<PlayerCharEffectsController>();
    }

    protected override void Start()
    {
        base.Start();
        tag = "Player";

        if (GameController.instance != null)
            GameController.instance.currentLvlCtrl.players.Add(this);
        SubscribeToEvents();
        StartCoroutine(CheckNear());
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (charsNear.Count > 0 && charsNear[0].tag == "Enemy")
        {
            TurnToLook(charsNear[0].transform.position);
        }
        if (charsInfront.Count > 0 && charsInfront[0].tag == "Enemy")
        {
            
            _anim.SetTrigger("attackTrigger");
        }
        
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    #region Senses
    IEnumerator CheckNear()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(LookNear());
            StartCoroutine(LookInfront());
        }
    }
    protected void TurnToLook(Vector3 pos)
    {
        //var mypos = transform.position;
        //mypos.y = 0;
        pos.y = 0;
        //var angle = Vector3.Angle(-mypos, pos);
        //transform.rotation = Quaternion.Euler(0, angle, 0);
        transform.LookAt(pos);

    }
    #endregion


    #region Combat

    public override void Attack()
    {
        base.Attack();
        StartCoroutine(DealDamage());
    }

    public void ChangeAttackType()
    {
        _anim.SetFloat("attackPower", Random.Range(1f, 2f));
        _anim.SetFloat("variants", Random.Range(0, 5));
    }

    public override void TakeDamage(float damage, ZCharacterController dealer = null)
    {
        base.TakeDamage(damage, dealer);
        CharacterDamaged damaged = new CharacterDamaged { Value = damage };
        EventAggregator.Publish(damaged);
    }
    IEnumerator DealDamage()
    {
        StartCoroutine(LookInfront());
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < charsInfront.Count; i++)
        {
            if (charsInfront[i] != null && charsInfront[i].tag == "Enemy" && charsInfront[i].isActiveAndEnabled)
            {
                charsInfront[i].TakeDamage(damage, this);
            }
        }
    }

    public override void Die(DieType type = DieType.explode)
    {
        isAlive = false;
        CharacterDeath death = new CharacterDeath { };
        EventAggregator.Publish(death);
    }
    #endregion


    #region Leveling and Data control
    public void LoadData()
    {

    }

    public void LevelUp()
    {
        Level++;
        
        _effects.LevelUp();


        var levelUp = new CharacterLevelUp
        {
            Level = Level,
            MaxHp = maxHp,
            Damage = damage,
            Radius = radius
        };
        EventAggregator.Publish(levelUp);

        if (Exp > expToNextLvl)
            EventAggregator.Publish(new CharacterLevelReached { });
    }


    #endregion


    #region Events
    private void SubscribeToEvents()
    {
        EventAggregator.Subscribe<ZombieKilledByPlayersCharacter>(OnZombieKilledByCharacter);
        EventAggregator.Subscribe<ZombieKilledByPlayer>(OnZombieKilledByPlayer);
    }

    private void UnsubscribeFromEvents()
    {
        EventAggregator.UnsubscribeAll();
    }

    private void OnZombieKilledByCharacter(IEventBase eventBase)
    {
        if (eventBase is ZombieKilledByPlayersCharacter e)
        {
            Exp += e.Exp;
        }
    }
    private void OnZombieKilledByPlayer(IEventBase eventBase)
    {
        if (eventBase is ZombieKilledByPlayer e)
        {
            Exp += e.Exp;
        }
    }
    #endregion
}

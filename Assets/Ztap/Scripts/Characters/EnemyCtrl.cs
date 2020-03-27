using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCtrl : ZCharacterController, IPointerDownHandler
{
    [Header("Following")]
    public bool isFollowing;
    public Transform targetToFollow;

    [Header("VFX")]
    public ParticleSystem DeathVFX;

    private NavMeshAgent _navAgent;
    private Animator _anim;
    private Collider _coll;
    private Rigidbody _rb;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _anim = GetComponent<Animator>();
        _coll = GetComponent<Collider>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _navAgent.enabled = true;
        _anim.enabled = true;
        _anim.SetBool("zombieAlive", true);
        _coll.enabled = true;
        for (int i = 0; i< transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        isAlive = true;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        tag = "Enemy";

        GameController.instance.currentLvlCtrl.enemies.Add(this);


        if (targetToFollow == null)
            targetToFollow = GameController.instance.currentLvlCtrl.players[0].transform;

        StartCoroutine(CheckNear());

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (isFollowing)
            Move();



        //Если впереди игрок/игроки - начинается анимация атаки
        if (targetToFollow.tag == "Player" &&
            Vector3.Distance(transform.position, targetToFollow.position) < radius)
        {
            _anim.SetTrigger("attack");
        }
        
    }

    public override void Move()
    {
        base.Move();
        if (isAlive)
        {
            _anim.SetFloat("moveVariable", _navAgent.velocity.magnitude);
            if (_navAgent.isOnNavMesh)
                _navAgent.SetDestination(targetToFollow.position);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Die();
    }

    public override void Die(DieType type = DieType.explode)
    {
        switch (type)
        {
            case DieType.anim:
                StartCoroutine(Dying());
                break;
            case DieType.explode:
                StartCoroutine(Exploding());
                break;
        }
    }

    #region DieType coroutines
    IEnumerator Dying()
    {
        _navAgent.enabled = false;
        _coll.enabled = false;
        _anim.SetBool("zombieAlive", false);
        _anim.SetTrigger("death");
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }

    IEnumerator Exploding()
    {
        _navAgent.enabled = false;
        _anim.enabled = false;
        _coll.enabled = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        DeathVFX.gameObject.SetActive(true);
        DeathVFX.Play(true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
    #endregion
    

    /// <summary>
    /// Вызывается из анимации
    /// </summary>
    public override void Attack()
    {
        base.Attack();
        if (isAlive)
            StartCoroutine(DealDamage());
    }

    IEnumerator CheckNear()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(LookInfront());
        }
    }

    IEnumerator DealDamage()
    {
        StartCoroutine(LookInfront());
        yield return new WaitForFixedUpdate();
        for (int i = 0; i < charsInfront.Count; i++)
        {
            if (charsInfront[i] != null && charsInfront[i].tag == "Player" && charsInfront[i].isActiveAndEnabled)
            {
                charsInfront[i].TakeDamage(damage, this);
            }
        }
    }

    /// <summary>
    /// Вызывается из анимации
    /// </summary>
    public void ChangeAttackType()
    {
        _anim.SetFloat("variable", Random.Range(1f, 2f));
    }
}

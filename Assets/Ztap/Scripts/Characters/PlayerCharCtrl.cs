using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharCtrl : ZCharacterController
{
    private Animator _anim;

    private void Awake()
    {
        
        _anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        tag = "Player";
        GameController.instance.currentLvlCtrl.players.Add(this);
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

    IEnumerator CheckNear()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            StartCoroutine(LookNear());
            StartCoroutine(LookInfront());
        }
    }

    public void ChangeAttackType()
    {
        _anim.SetFloat("attackPower", Random.Range(1f, 2f));
        _anim.SetFloat("variants", Random.Range(0, 5));
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

    public override void Attack()
    {
        base.Attack();
        StartCoroutine(DealDamage());
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

}

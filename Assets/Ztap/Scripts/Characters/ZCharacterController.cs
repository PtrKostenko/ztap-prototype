using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZCharacterController : MonoBehaviour
{
    [Header("Main")]
    public float maxHp = 100;
    public float hp = 100;
    [Header("Movement")]
    public float speed = 1;
    [Header("Combat")]
    public float damage = 1;
    public float radius = 1;
    [Header("Senses")]
    public CharsNearTrigger nearTriggerCollider;
    public List<ZCharacterController> charsNear;
    public CharsInfrontTrigger infrontTriggerCollider;
    public List<ZCharacterController> charsInfront;

    protected bool isAlive;

    public enum DieType
    {
        anim,
        explode
    }
    // Start is called before the first frame update
    virtual protected void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
    }


    virtual protected IEnumerator LookNear()
    {
        charsNear.Clear();
        nearTriggerCollider.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        nearTriggerCollider.gameObject.SetActive(false);
    }

    virtual protected IEnumerator LookInfront()
    {
        charsInfront.Clear();
        infrontTriggerCollider.gameObject.SetActive(true);
        yield return new WaitForFixedUpdate();
        infrontTriggerCollider.gameObject.SetActive(false);
    }



    virtual public void Move()
    {

    }

    virtual public void TakeDamage(float damage, ZCharacterController dealer = null)
    {
        hp -= damage;
        if (hp < 0)
        {
            if (dealer == null)
            {
                Die();
            }
            else
            {
                Die(DieType.anim);
            }
        }
    }


    virtual public void Die(DieType type = DieType.explode)
    {
        isAlive = false;
        gameObject.SetActive(false);
    }

    virtual public void Attack()
    {

    }
    

    
}

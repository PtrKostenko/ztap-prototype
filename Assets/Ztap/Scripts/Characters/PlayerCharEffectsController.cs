using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharEffectsController : MonoBehaviour
{
    [SerializeField] ParticleSystem levelUp;
    [SerializeField] ParticleSystem jump;
    [SerializeField] ParticleSystem death;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        jump.Play(true);
    }

    public void LevelUp()
    {
        levelUp.Play(true);
    }

    public void Death()
    {
        death.Play(true);
    }
}

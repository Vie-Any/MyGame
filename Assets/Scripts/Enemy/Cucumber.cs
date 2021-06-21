using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : Enemy, IDamageable
{

    public void GetHit(float damage)
    {
        health -= damage;
        if (health<1)
        {
            health = 0;
            isDead = true;
        }
        animator.SetTrigger("hit");
    }

    // Animation Event
    public void SetOff()
    {
        targetPoint.GetComponent<Boom>().TurnOff();
    }
}

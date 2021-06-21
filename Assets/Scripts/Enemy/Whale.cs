using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whale : Enemy, IDamageable
{

    public float scale;

    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        animator.SetTrigger("hit");
    }

    public void Swalow()
    {
        //if(transform.position.x)
        targetPoint.GetComponent<Boom>().TurnOff();
        targetPoint.gameObject.SetActive(false);

        transform.localScale *= scale;
    }
}

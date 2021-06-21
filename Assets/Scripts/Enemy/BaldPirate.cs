using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaldPirate : Enemy, IDamageable
{
    public void GetHit(float damage)
    {
        health -= damage;
        if (health < 1)
        {
            health = 0;
            isDead = true;
        }
        animator.SetTrigger("hit");
        //更新Boss的血条显示
        UIManager.instance.UpdateBossHealth(health);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : Enemy, IDamageable
{
    SpriteRenderer sprite;

    public override void Init()
    {
        base.Init();
        sprite = GetComponent<SpriteRenderer>();
    }

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

    public override void Update()
    {
        base.Update();
        if (animatorState == 0)
        {
            sprite.flipX = false;
        }
    }

    public override void SkillAction()
    {
        base.SkillAction();
        if (animator.GetCurrentAnimatorStateInfo(1).IsName("skill"))
        {
            //利用组件的属性进行X轴翻转
            sprite.flipX = true;
            if (transform.position.x > targetPoint.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.right, speed * 2 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + Vector3.left, speed * 2 * Time.deltaTime);
            }
        }
        else
        {
            sprite.flipX = false;
        }
    }
}

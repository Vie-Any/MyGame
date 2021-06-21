using UnityEngine;

public class BigGuy : Enemy, IDamageable
{

    //炸弹捡起点
    public Transform pickUpPoint;

    public float power;


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

    public void Pick()
    {
        if (targetPoint.CompareTag("Boom") && !hasBoom)
        {
            targetPoint.gameObject.transform.position = pickUpPoint.position;

            targetPoint.SetParent(pickUpPoint);

            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            hasBoom = true;

        }
    }

    public void ThrowOut()
    {
        if (hasBoom)
        {
            targetPoint.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            targetPoint.SetParent(transform.parent.parent);

            if (FindObjectOfType<PlayerController>().gameObject.transform.position.x - transform.position.x < 0)
            {
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1,1)*power,ForceMode2D.Impulse);
            }
            else
            {
                targetPoint.GetComponent<Rigidbody2D>().AddForce(new Vector2(1, 1) * power, ForceMode2D.Impulse);
            }
        }
        hasBoom = false;
    }
}

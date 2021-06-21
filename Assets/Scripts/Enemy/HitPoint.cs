using UnityEngine;

public class HitPoint : MonoBehaviour
{

    //是否可以使用炸弹
    public bool boomAvaliable;

    //方向
    int direction;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.x > other.transform.position.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().GetHit(1);
        } else if (other.CompareTag("Boom") && boomAvaliable)
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction,1)*10,ForceMode2D.Impulse);
        }
    }
}

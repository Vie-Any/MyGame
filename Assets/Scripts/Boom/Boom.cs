using UnityEngine;

public class Boom : MonoBehaviour
{
    //定义炸弹的Animator对象
    private Animator animator;
    //定义炸弹的碰撞体对象
    private Collider2D _collider2D;
    //定义炸弹的Rigidbody2D对象
    private Rigidbody2D rigidbody2D;

    //开始时间
    public float startTime;
    //需要的等待时间
    public float waitTime;
    //爆炸力
    public float bombForce;

    //需要检查的元素
    [Header("Check")]
    //检查的爆炸范围
    public float radius;
    //爆炸范围内的图层以及图层范围内的物体
    public LayerMask targetLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        //初始化animator变量，从组件中获取到Animator对象
        animator = GetComponent<Animator>();
        //初始化_collider2D变量，从组件中获取到Collider2D对象
        _collider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //记录开始时间
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("boom_off"))
        {
            if (Time.time > startTime + waitTime)
            {
                animator.Play("boom_explotion");
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius);
    }

    public void Explotion()
    {
        //将碰撞体取消勾选以使得爆炸范围内检测物体不包含碰撞体
        _collider2D.enabled = false;
        //获取爆炸范围内所有的物体数组
        Collider2D[] aroundObjects = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        //取消炸弹刚体的重力
        rigidbody2D.gravityScale = 0;

        foreach (var item in aroundObjects)
        {
            Vector3 pos = transform.position - item.transform.position;
            
            item.GetComponent<Rigidbody2D>().AddForce((-pos+Vector3.up)*bombForce,ForceMode2D.Impulse);

            if (item.CompareTag("Boom") && item.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("boom_off"))
            {
                item.GetComponent<Boom>().TurnOn();
            }
            if (item.CompareTag("Player"))
            {
                item.GetComponent<IDamageable>().GetHit(3);
            }
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    public void TurnOff()
    {
        animator.Play("boom_off");
        gameObject.layer = LayerMask.NameToLayer("NPC");
    }

    public void TurnOn()
    {
        startTime = Time.time;
        animator.Play("boom_on");
        gameObject.layer = LayerMask.NameToLayer("Boom");
    }
}

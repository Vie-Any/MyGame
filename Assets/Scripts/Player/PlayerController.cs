using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    //人物的刚体对象
    private Rigidbody2D rigidbody2D;
    //摇杆操作
    public FixedJoystick joystick; 

    //动画对象
    private Animator animator;

    //人物移动速度
    public float speed;

    //跳起力度
    public float jumpForce;

    [Header("Player State")]
    public float health;
    public bool isDead;
    public bool isHurt;

    //地面检测属性
    [Header("Ground Check")] 
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;

    //状态检查属性
    [Header("States Check")] 
    public bool isGround;
    public bool isJump;
    public bool canJump;

    //跳跃属性
    [Header("Jump FX")]
    public GameObject jumpFX;
    public GameObject landFX;

    //攻击属性
    [Header("Attack Settings")] 
    //炸弹对象
    public GameObject boomPrefab;
    //下次攻击(人物对炸弹释放的CD时间)
    public float nextAttact = 0;
    //攻击频率(CD时间值)
    public float attackRate;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        joystick = FindObjectOfType<FixedJoystick>();

        //使用观察者模式传递对象
        GameManager.instance.IsPlayer(this);
        //从数据保存区获取玩家的生命值
        health = GameManager.instance.LoadHealth();
        //更新界面显示的生命值
        UIManager.instance.UpdateHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("dead", isDead);
        if (isDead)
        {
            return;
        }
        isHurt = animator.GetCurrentAnimatorStateInfo(1).IsName("player_hit");
        CheckInput();
    }

    public void FixedUpdate()
    {
        if (isDead)
        {
            rigidbody2D.velocity = Vector2.zero;
            return;
        }
        PhysicsCheck();//状态检测

        if (!isHurt)
        {
            Movement();//移动
            Jump();//跳跃
        }
        
    }

    //输入检测
    void CheckInput()
    {
        if (isDead)
        {
            return;
        }
        //if (Input.GetButtonDown("Jump") && isGround)
        //if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        //{
        //    canJump = true;
        //}

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Attack();
        }
    }

    //人物移动
    void Movement() {
        //键盘操作
        //float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (joystick != null)
        {
            float horizontalInput = joystick.Horizontal;

            rigidbody2D.velocity = new Vector2(horizontalInput * speed, rigidbody2D.velocity.y);

            //if (horizontalInput != 0)
            //{
            //    transform.localScale = new Vector3(horizontalInput,1,1);
            //}

            if (horizontalInput > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if(horizontalInput < 0)
            {
                transform.eulerAngles = new Vector3(0,180,0);
            }
        }
    }

    //人物跳跃
    void Jump()
    {
        if (canJump)
        {
            isJump = true;
            jumpFX.SetActive(true);
            jumpFX.transform.position = transform.position + new Vector3(0,-0.45f,0);
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            rigidbody2D.gravityScale = 4;
            canJump = false;
            isGround = true;
        }
    }

    public void ButtonJump()
    {
        if (isGround)
        {
            canJump = true;
        }
    }

    //攻击事件
    public void Attack()
    {
        if (Time.time>nextAttact)
        {
            //生成炸弹
            Instantiate(boomPrefab, transform.position, boomPrefab.transform.rotation);
            //修改下次可释放炸弹的攻击时间
            nextAttact = Time.time + attackRate;
        }
    }

    //物理检测
    void PhysicsCheck()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius,groundLayer);
        if (isGround)
        {
            
            rigidbody2D.gravityScale = 1;
            //canJump = false;
            isJump = false;
        }
        else
        {
            rigidbody2D.gravityScale = 4;
        }
    }

    //落地事件
    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.75f, 0);
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position,checkRadius);
    }

    public void GetHit(float damage)
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("player_hit"))
        {
            health -= damage;
            if (health<1)
            {
                health = 0;
                isDead = true;
            }
            animator.SetTrigger("hit");

            UIManager.instance.UpdateHealth(health);
        }
    }
}

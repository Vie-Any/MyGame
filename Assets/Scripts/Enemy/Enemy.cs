using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //当前状态的状态机
    EnemyBaseState currentState;

    //动画对象
    public Animator animator;

    //动画状态
    public int animatorState;

    private SpriteRenderer renderer;

    //警告标志对象
    private GameObject alarmSign;

    [Header("Base State")]
    public float health;
    public bool isDead;
    public bool hasBoom;
    public bool isBoss;

    [Header("Movement")]
    //速度
    public float speed;
    //范围检测点
    public Transform pointA, pointB;
    //目标点
    public Transform targetPoint;

    [Header("Attack Setting")]
    //下次攻击
    private float nextAttack = 0;
    //攻击频率
    public float attackRate;
    //普通攻击距离与技能攻击距离
    public float attackRange, skillRange;

    //攻击列表
    public List<Transform> attackList = new List<Transform>();

    //巡逻状态
    public PatrolState patrolState = new PatrolState();

    //攻击状态
    public AttackState attackState = new AttackState();


    public virtual void Init() {
        animator = GetComponent<Animator>();
        //因为unity项目中Alarm Sign是Sprite Setting的第1个子对象，所以直接获取序号为0的子对象即可
        alarmSign = transform.GetChild(0).gameObject;
        renderer = GetComponent<SpriteRenderer>();

        /*** 对单例对象的使用不要在Awake前调用，有时候会触发空引用问题，原因是Awake可能会在单例初始化之前进行(有时单例初始化了，但是调用方不能及时拿到单例的引用)所以会导致空引用  ***/
        //自动注册敌人到游戏管理对象中
        //添加到Game Manager敌人列表进行管理
        //GameManager.instance.IsEnemy(this);
    }

    public void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        //自动注册敌人到游戏管理对象中
        //添加到Game Manager敌人列表进行管理
        GameManager.instance.IsEnemy(this);
        TransitionToState(patrolState);
        if (isBoss)
        {
            UIManager.instance.SetBossHealth(health);
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        animator.SetBool("dead", isDead);
        if (isDead)
        {
            GameManager.instance.EnemyDead(this);
            renderer.sortingOrder = GameManager.instance.deadObjectLayer++;
            return;
        }
        currentState.OnUpdate(this);
        animator.SetInteger("state", animatorState);
    }

    public void TransitionToState(EnemyBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    //移向目标
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        FlipDirection();
    }

    //攻击玩家
    public void AttackAction()
    {
        //
        if(Vector2.Distance(transform.position, targetPoint.position) < attackRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                animator.SetTrigger("attack");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    //对炸弹使用技能
    public virtual void SkillAction()
    {
        
        if (Vector2.Distance(transform.position, targetPoint.position) < skillRange)
        {
            if (Time.time > nextAttack)
            {
                //播放攻击动画
                animator.SetTrigger("skill");
                nextAttack = Time.time + attackRate;
            }
        }
    }

    //翻转方向
    public void FlipDirection()
    {
        if (transform.position.x < targetPoint.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }

    //切换点
    public void SwitchPoint()
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        //当攻击列表不包含当前对象并且不是炸弹并且玩家未死亡并且游戏未结束
        if (!attackList.Contains(collision.transform) && !hasBoom && !isDead && !GameManager.instance.gameOver)
        {
            attackList.Add(collision.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        attackList.Remove(collision.transform);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && !GameManager.instance.gameOver)
        {
            StartCoroutine(OnAlarm());
        }
    }

    IEnumerator OnAlarm()
    {
        alarmSign.SetActive(true);
        yield return new WaitForSeconds(alarmSign.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        alarmSign.SetActive(false);

    }
}
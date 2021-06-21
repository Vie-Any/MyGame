using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//游戏场景管理

public class Door : MonoBehaviour
{

    Animator animator;

    BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //使用观察者模式传递对象
        GameManager.instance.IsExitDoor(this);
        //场景初始将门的碰撞属性禁用
        boxCollider2D.enabled = false;
    }

    public void OpenDoor()
    {
        //播放开门动画
        animator.Play("open");
        //启用门的碰撞属性
        boxCollider2D.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Game Manager 去到下一个房间
            GameManager.instance.NextLevel();
        }
    }
}

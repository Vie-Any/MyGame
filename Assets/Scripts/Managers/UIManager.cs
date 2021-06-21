using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //静态变量只会有一个实例对象
    public static UIManager instance;

    //玩家的健康值显示条
    public GameObject healthBar;

    //游戏结束面板对象
    public GameObject gameOverPanel;

    [Header("UI Elements")]
    public GameObject pauseMenu;
    public Slider bossHealthBar;

    //单例变量初始化
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //更新血条
    public void UpdateHealth(float currentHealth)
    {
        switch (currentHealth)
        {
            case 3:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(true);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 1:
                healthBar.transform.GetChild(0).gameObject.SetActive(true);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
            case 0:
                healthBar.transform.GetChild(0).gameObject.SetActive(false);
                healthBar.transform.GetChild(1).gameObject.SetActive(false);
                healthBar.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    //暂停游戏
    public void PauseGame()
    {
        if (pauseMenu!=null)
        {
            pauseMenu.SetActive(true);//激活暂停菜单
            Time.timeScale = 0;//暂停游戏
        }
    }

    //恢复游戏
    public void ResumeGame()
    {
        if (pauseMenu!=null)
        {
            pauseMenu.SetActive(false);//隐藏暂停菜单
            Time.timeScale = 1;//恢复游戏
        }
    }

    //初始化Boss的血条值
    public void SetBossHealth(float health)
    {
        if (bossHealthBar!=null)
        {
            //令血条最大值等于当前健康值
            bossHealthBar.maxValue = health;
        }
    }

    //更新Boss的血条值显示
    public void UpdateBossHealth(float health)
    {
        if (bossHealthBar!=null)
        {
            bossHealthBar.value = health;
        }
    }

    public void GameOverUI(bool playerDead)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(playerDead);
        }
    }
}

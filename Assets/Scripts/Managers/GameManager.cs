using System.Collections;
using System.Collections.Generic;
using UnityEngine;//游戏引擎依赖
using UnityEngine.SceneManagement;//游戏场景管理

public class GameManager : MonoBehaviour
{
    //使用单例模式对游戏进行管理
    public static GameManager instance;

    //玩家对象
    private PlayerController player;

    //游戏结束界面
    public GameObject finishPanel;

    //出口
    private Door doorExit;

    //游戏是否结束
    public bool gameOver;

    //敌人列表
    public List<Enemy> enemies = new List<Enemy>();

    //死亡物体的Layer号
    public int deadObjectLayer = 0;

    public void Awake()
    {
        //单例对象初始化
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        /// 如果在发布游戏包的时候，在playerSetting中设置了禁止屏幕翻转，但是代码中设置屏幕是可自动翻转，则游戏发布出来后，任然是可翻转的。
        Screen.orientation = ScreenOrientation.AutoRotation;
        /// 下面几个bool值设置了是否可以翻转到某个方向。false代表是禁止
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        //初始化玩家对象
        player = FindObjectOfType<PlayerController>();
        //初始化出口
        doorExit = FindObjectOfType<Door>();
    }

    public void Update()
    {
        if (player != null)
        {
            gameOver = player.isDead;
            //显示游戏结束面板
            UIManager.instance.GameOverUI(gameOver);
        }
        
    }

    //添加敌人到列表
    public void IsEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    //设置玩家对象(观察者模式，由对象生成方主动设置，避免在无需此对象的情况下报空引用的错误)
    public void IsPlayer(PlayerController controller)
    {
        player = controller;
    }

    public void IsExitDoor(Door door)
    {
        doorExit = door;
    }

    public void EnemyDead(Enemy enemy)
    {
        enemies.Remove(enemy);

        if (enemies.Count == 0)
        {
            if (doorExit != null)
            {
                doorExit.OpenDoor();
            }
        }
    }

    //重新开始游戏场景
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        PlayerPrefs.DeleteKey("playerHealth");
    }

    //新游戏
    public void NewGame()
    {
        //清空原先存储的所有数据
        PlayerPrefs.DeleteAll();
        //加载第二个场景(因为第一个场景是主菜单界面)
        SceneManager.LoadScene(1);
    }

    //继续游戏
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("sceneIndex"))
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("sceneIndex"));
        }
        else
        {
            NewGame();
        }
    }

    //回到主菜单
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    //进入下一关
    public void NextLevel()
    {
        //如果当前场景号等于总场景数-1，则当前为最后一个场景，回到主界面
        if ((SceneManager.sceneCountInBuildSettings - 1) == SceneManager.GetActiveScene().buildIndex)
        {
            //清空原先存储的所有数据
            PlayerPrefs.DeleteAll();
            finishPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SaveData();
        }
    }

    //退出游戏
    public void QuitGame()
    {
        Application.Quit();
    }

    //从存储中加载玩家健康值
    public float LoadHealth()
    {
        if (!PlayerPrefs.HasKey("playerHealth"))
        {
            PlayerPrefs.SetFloat("playerHealth", 3);
        }
        //取出生命值
        float currentHealth = PlayerPrefs.GetFloat("playerHealth");

        //返回取出的生命值
        return currentHealth;
    }

    public void SaveData()
    {
        //将数据存入存储区
        PlayerPrefs.SetFloat("playerHealth", player.health);
        //保存场景序号
        PlayerPrefs.SetInt("sceneIndex",SceneManager.GetActiveScene().buildIndex + 1);
        //将存储区数据从内存flush到硬盘进行持久化存储
        PlayerPrefs.Save();
    }
}

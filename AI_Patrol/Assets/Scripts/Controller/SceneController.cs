using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SceneController : MonoBehaviour, ISceneController, IUserAction
{
    public PatrolFactory patrolsFactory;                                 // 巡逻兵工厂                           
    public CoinFactory coinFactory;                                      // 硬币工厂
    public ScoreRecorder scoreRecorder;                                  // 记分员
    public PatrolActionManager actionManager;                            // 动作管理器
    public TimeManager timeManager;
    public UserGUI userGUI;                                              // 用户界面
    public GameObject camera;                                            // 相机对象
    public GameObject player;                                            // 玩家
    public GameObject maze;                                              // 地图
    public List<GameObject> patrols = new List<GameObject>();            // 巡逻兵列表
    public List<GameObject> coins = new List<GameObject>();              // 硬币列表
    
    public bool gameOver = false;                                        // 游戏结束标志

    public int floorNumber = 0;                                          // 记录玩家当前所在房间
    public float playerSpeed = 3f;                                       // 玩家移动速度
    public int totalCoinNumber = 5;                                      // 总硬币数
    public int coinNumberGet = 0;                                        // 玩家已收集硬币数量

    private void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;

        patrolsFactory = Singleton<PatrolFactory>.Instance;
        coinFactory = Singleton<CoinFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<PatrolActionManager>() as PatrolActionManager;
        userGUI = gameObject.AddComponent<UserGUI>() as UserGUI;
        timeManager = gameObject.AddComponent<TimeManager>() as TimeManager;

        director.CurrentSceneController.LoadResource();
        gameOver = false;
}

    public void LoadResource()
    {
        maze = Object.Instantiate(Resources.Load<GameObject>("Prefabs/maze"), new Vector3(1.5f, 0.5f, 14), Quaternion.identity);
        player = Object.Instantiate(Resources.Load<GameObject>("Prefabs/player"), new Vector3(-15, 0.5f, -10), Quaternion.identity);
        floorNumber = 1;
        player.name = "player";

        camera.AddComponent<CameraFollow>();
        camera.GetComponent<CameraFollow>().player = player;

        patrols = patrolsFactory.GetPatrols();

        coins = coinFactory.GetCoins();
        totalCoinNumber = coins.Count;

        for(int i = 0; i < patrols.Count; i++)
        {
            actionManager.PatrolMove(patrols[i]);
        }
    }

    public void MovePlayer(float x, float z)
    {
        if (!gameOver)
        {
            //移动和旋转
            player.transform.Translate(0, 0, z * playerSpeed * Time.deltaTime);
            player.transform.Rotate(0, x * 135f * Time.deltaTime, 0);
            //防止碰撞带来的移动
            if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
            {
                player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
            }
            if (player.transform.position.y != 0.5f)
            {
                player.transform.position = new Vector3(player.transform.position.x, 0.5f, player.transform.position.z);
            }
        }
    }

    public void Restart()
    {
        gameOver = false;
        scoreRecorder.Reset();
        player.GetComponent<Animator>().SetBool("death", false);
        patrolsFactory.Reset();
        coinFactory.Reset();

        for (int i = 0; i < patrols.Count; i++)
        {
            actionManager.PatrolMove(patrols[i]);
        }
        coinNumberGet = 0;
        timeManager.Reset();
    }

    public string GetTime()
    {
        return timeManager.GetTimeText();
    }
    public int GetScore()
    {
        return scoreRecorder.GetScore();
    }

    public int GetCoinNumHave()
    {
        return coinNumberGet;
    }
    public int GetCoinNumNeed()
    {
        return totalCoinNumber - coinNumberGet;
    }
    public bool isGameOver()
    {
        return gameOver;
    }

    void OnEnable()
    {
        //注册事件
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameOver += GameOver;
        GameEventManager.CoinNumberChange += ReduceCoinNumber;
    }
    void OnDisable()
    {
        //取消注册事件
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameOver -= GameOver;
        GameEventManager.CoinNumberChange -= ReduceCoinNumber;
    }

    void AddScore()
    {
        scoreRecorder.AddScore();
    }

    void GameOver()
    {
        gameOver = true;
        actionManager.DestroyAll();
    }

    void ReduceCoinNumber()
    {
        coinNumberGet += 1;
    }

    void Start()
    {

    }

    void Update()
    {
        for (int i = 0; i < patrols.Count; i++)
        {
            patrols[i].gameObject.GetComponent<PatrolData>().plyerFloor = floorNumber;
        }
        if (GetCoinNumNeed() == 0)
        {
            GameOver();
        }
    }
}
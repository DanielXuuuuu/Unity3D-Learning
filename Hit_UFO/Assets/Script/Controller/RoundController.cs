using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoundController : MonoBehaviour, ISceneController, IUserAction
{
    public DiskFactory diskFactory;
    public CCActionManager actionManager;
    public ScoreRecorder scoreRecorder;
    public UserGUI userGui;

    private Queue<GameObject> diskQueue = new Queue<GameObject>();
    private List<GameObject> diskMissed = new List<GameObject>();

    private int totalRound = 3;
    private int trialNumPerRound = 10;
    private int currentRound = -1;
    private int currentTrial = -1;
    private float throwSpeed = 2f;
    private int gameState = 0; //-1：失败 0：初始状态 1：进行中 2：胜利
    private float throwInterval = 0;
    private int userBlood = 10;

    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        
        diskFactory = Singleton<DiskFactory>.Instance;
        userGui = gameObject.AddComponent<UserGUI>() as UserGUI;
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;

        scoreRecorder = new ScoreRecorder();
    }

    public void LoadResource()
    {
        diskQueue.Enqueue(diskFactory.GetDisk(currentRound));
    }

    public void ThrowDisk(int count)
    {
        while(diskQueue.Count <= count)
        {
            LoadResource();
        }
        for(int i = 0; i < count; i++)
        {
                    float position_x = 16;
        GameObject disk = diskQueue.Dequeue();
        diskMissed.Add(disk);
        disk.SetActive(true);
        //设置飞碟位置
        float ran_y = Random.Range(-3f, 3f);
        float ran_x = Random.Range(-1f, 1f) < 0 ? -1 : 1;
        disk.GetComponent<DiskData>().direction = new Vector3(ran_x, ran_y, 0);
        Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, ran_y, 0);
        disk.transform.position = position;
        //设置飞碟初始所受的力和角度
        float power = Random.Range(10f, 15f);
        float angle = Random.Range(15f, 28f);
        actionManager.diskFly(disk, angle, power);
        }
    }

    void levelUp()
    {
        currentRound += 1;
        throwSpeed -= 0.5f;
        currentTrial = 1;
    }

    void Update()
    {
        if(gameState == 1)
        {
            if(userBlood <= 0 || (currentRound == totalRound && currentTrial == trialNumPerRound))
            {
                GameOver();
                return;
            }
            else
            {
                if (currentTrial > trialNumPerRound)
                {
                    levelUp();
                }
                if (throwInterval > throwSpeed)
                {
                    int throwCount = generateCount(currentRound);
                    ThrowDisk(throwCount);
                    throwInterval = 0;
                    currentTrial += 1;
                }
                else
                {
                    throwInterval += Time.deltaTime;
                }
            }
        }
        for (int i = 0; i < diskMissed.Count; i++)
        {
            GameObject temp = diskMissed[i];
            //飞碟飞出摄像机视野且未被打中
            if (temp.transform.position.y < -8 && temp.gameObject.activeSelf == true)
            {
                diskFactory.FreeDisk(diskMissed[i]);
                diskMissed.Remove(diskMissed[i]);
                userBlood -= 1;
            }
        }
    }

    public int generateCount(int currentRound)
    {
        if(currentRound == 1)
        {
            return 1;
        }
        else if(currentRound == 2)
        {
            return Random.Range(1, 2);
        }
        else
        {
            return Random.Range(1, 3);
        }
    }


    public void StartGame()
    {
        gameState = 1;
        currentRound = 1;
        currentTrial = 1;
        userBlood = 10;
        throwSpeed = 2f;
        throwInterval = 0;
}

    public void GameOver()
    {
        if(userBlood <= 0)
        {
            gameState = -1;//失败
        }
        else
        {
            gameState = 2;//胜利
        }
    }

    public void Restart()
    {
        scoreRecorder.Reset();
        StartGame();
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool notHit = false;
        foreach (RaycastHit hit in hits)
            //射线打中物体
            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                //射中的物体要在没有打中的飞碟列表中
                for (int j = 0; j < diskMissed.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == diskMissed[j].gameObject.GetInstanceID())
                    {
                        notHit = true;
                    }
                }
                if (!notHit)
                {
                    return;
                }
                diskMissed.Remove(hit.collider.gameObject);
                //记录分数
                scoreRecorder.Record(hit.collider.gameObject);
                diskFactory.FreeDisk(hit.collider.gameObject);
                break;
            }
    }

    public int GetScore()
    {
        return scoreRecorder.GetScore();
    }

    public int GetCurrentRound()
    {
        return currentRound;
    }
    public int GetBlood()
    {
        return userBlood;
    }
    public int GetGameState()
    {
        return gameState;
    }
}

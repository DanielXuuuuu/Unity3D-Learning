using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{
    public RoleModel[] Priests = new RoleModel[3];
    public RoleModel[] Devils = new RoleModel[3];
    public RiverModel River;
    public LandModel RightLand;
    public LandModel LeftLand;
    public BoatModel Boat;
    Vector3[] PrisetsOriginPositions = new Vector3[] { new Vector3(5F, 0, 0), new Vector3(5.5F, 0, 0), new Vector3(6F, 0, 0) };
    Vector3[] DevilsOriginPositions = new Vector3[] { new Vector3(7F, 0, 0), new Vector3(7.5F, 0, 0), new Vector3(8F, 0, 0) };
    public float speed;

    public UserGUI userGui;
    public CCActionManager actionManager;
    public Judger judger;

    public stateGraph graph;        // 状态图
    public stateNode currState;     // 当前游戏状态对应的状态节点，这个状态节点只是单纯的记录，和图中真正的记录节点不同

    public bool moving { get; set; }
    public int currentState;    // 0：游戏进行中， 1：游戏胜利  -1：游戏失败
    public bool gaming = true;  // 用于判断游戏是否正在进行，由于目前还不存在起始界面，所以一开始游戏就开始了

    void Awake()
    {
        speed = 4;
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        userGui = gameObject.AddComponent<UserGUI>() as UserGUI;
        director.CurrentSceneController.LoadResource();

        judger = gameObject.AddComponent<Judger>() as Judger; 
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;

        graph = new stateGraph();
        currState = new stateNode(3, 3, true);
    }

    public void LoadResource()
    {
        for(int i = 0; i < 3; i++)
        {
            RoleModel priest = new RoleModel("priest");
            priest.SetName("priest" + i);
            priest.SetPosition(new Vector3(5 + i * 0.5F, 0, 0));
            Priests[i] = priest;

            RoleModel devil = new RoleModel("devil");
            devil.SetName("devil" + i);
            devil.SetPosition(new Vector3(7 + i * 0.5F, -0.1F, 0));
            devil.Idle();
            Devils[i] = devil;
        }

        RightLand = new LandModel("right");
        LeftLand = new LandModel("left");
        River = new RiverModel();
        Boat = new BoatModel();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            Boat.ChangeDirction();
            for (int i = 0; i < 3; i++)
            {
                Priests[i].ChangeDirction();
                Devils[i].ChangeDirction();
            }
            userGui.sign = currentState;
        }
    }
    public void Restart()
    {
        for (int i = 0; i < 3; i++)
        {
            Priests[i].Reset();
            Priests[i].SetPosition(PrisetsOriginPositions[i]);
            Devils[i].Reset();
            Devils[i].SetPosition(DevilsOriginPositions[i]);
            Devils[i].Idle();
        }
        Boat.Reset();
        gaming = true;
        currState = new stateNode(3, 3, true);  
    }

    public void MoveRole(RoleModel role)
    {
        if (!gaming || moving)
            return;
        Vector3 endPosition;

        if (role.IsOnBoat())        //上岸
        {
            //这里，为了和原游戏一致，使牧师和魔鬼在两边的排列顺序一致
            role = Boat.DeletePassenger(role);
            int id = role.GetName()[role.GetName().Length - 1] - '0';
            if (role.IsGood())
            {
                endPosition = PrisetsOriginPositions[id];
            }
            else
            {
                endPosition = DevilsOriginPositions[id];
            }
            if(role.GetSide() == -1)
            {
                endPosition.x = 0 - endPosition.x;
            }

            
            Vector3 middlePosition = new Vector3(role.GetRole().transform.position.x, endPosition.y, endPosition.z);
            actionManager.MoveRole(role.GetRole(), middlePosition, endPosition, speed);
            role.GoLand();
        }
        else                        //上船
        {
            if (Boat.IsFull() || Boat.GetSide() != role.GetSide())
            {
                return;

            }
            endPosition = Boat.getEmptyPosition();
            Vector3 middlePosition = new Vector3(endPosition.x, role.GetRole().transform.position.y, endPosition.z);
            actionManager.MoveRole(role.GetRole(), middlePosition, endPosition, speed);
            role.GoBoat(Boat);
            Boat.AddPassenger(role);
        }
    }
    public void MoveBoat()
    {
        //当船为空，或者船或人物在运动时，不允许移动船
        if (Boat.IsEmpty() || moving)
            return;
        RoleModel[] passengers = Boat.GetPassengers();
        int priest_num = 0, devil_num = 0;
        for (int i = 0; i < 2; i++)
        {
            if (passengers[i] != null)
            {
                if (passengers[i].IsGood())
                    priest_num++;
                else
                    devil_num++;
            }
        }

        currState.move(new move(priest_num, devil_num));
        actionManager.MoveBoat(Boat.GetBoat(), Boat.GetMoveDirection(), speed);
    }

    public void AI()
    {
        if (!gaming || moving)
            return;
        StartCoroutine(AIroutine());
    }

    IEnumerator AIroutine()
    {
        // 搜索图得到最佳的下一步
        move mv = graph.getNextMove(currState);
        Debug.Log(mv.priest_num);
        Debug.Log(mv.devil_num);

        // 自动执行上船下船操作

        // 先统计当前船上的人员情况
        int p_num = 0, d_num = 0;
        RoleModel[] passengers = Boat.GetPassengers();
        for (int i = 0; i < 2; i++)
        {
            if (passengers[i] != null)
            {
                if (passengers[i].IsGood())
                    p_num++;
                else
                    d_num++;
            }
        }

        // 正数表示上岸，复数表示上船
        p_num -= mv.priest_num;
        d_num -= mv.devil_num;
        int side = Boat.GetSide();

        if (p_num == 0 && d_num == 0)
        {
            MoveBoat();
        }
        else
        {
            // 先执行上岸操作，给船留出位置
            if (p_num > 0)
            {
                int temp = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (passengers[i] != null)
                    {
                        if (passengers[i].IsGood())
                        {
                            MoveRole(passengers[i]);
                            yield return new WaitForSeconds(2f);
                            temp++;
                            if (temp == p_num)
                                break;
                        }
                    }
                }
            }
            if (d_num > 0)
            {
                int temp = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (passengers[i] != null)
                    {
                        if (!passengers[i].IsGood())
                        {
                            MoveRole(passengers[i]);
                            yield return new WaitForSeconds(2f);
                            temp++;
                            if (temp == d_num)
                                break;
                        }
                    }
                }
            }
            if (p_num < 0)
            {
                int temp = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (Priests[i].GetSide() == side && !Priests[i].IsOnBoat())
                    {
                        MoveRole(Priests[i]);
                        yield return new WaitForSeconds(2f);
                        temp--;
                        if (temp == p_num)
                            break;
                    }
                }
            }
            if (d_num < 0)
            {
                int temp = 0;
                for (int i = 0; i < 3; i++)
                {
                    if (Devils[i].GetSide() == side && !Devils[i].IsOnBoat())
                    {
                        MoveRole(Devils[i]);
                        yield return new WaitForSeconds(2f);
                        temp--;
                        if (temp == d_num)
                            break;
                    }
                }
            }
            MoveBoat();
        }
    }
}

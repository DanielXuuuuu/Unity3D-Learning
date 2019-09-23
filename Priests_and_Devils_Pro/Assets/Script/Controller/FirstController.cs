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

    public bool moving { get; set; }
    public int currentState;
    public bool gaming = true; //用于判断游戏是否正在进行，由于目前还不存在起始界面，所以一开始游戏就开始了

    void Awake()
    {
        speed = 4;
        SSDirector director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        userGui = gameObject.AddComponent<UserGUI>() as UserGUI;
        director.CurrentSceneController.LoadResource();

        judger = gameObject.AddComponent<Judger>() as Judger; 
        actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
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
        actionManager.MoveBoat(Boat.GetBoat(), Boat.GetMoveDirection(), speed);
    }
}

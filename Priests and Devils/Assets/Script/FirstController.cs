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

    bool gaming = true; //用于判断游戏是否正在进行，由于目前还不存在起始界面，所以一开始游戏就开始了

    void Awake()
    {
        SSDirector director = SSDirector.GetInstance();
        //director.setFPS(60);
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResource();
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
    public int Check()
    {
        //等船停下来再check
        if (Boat.IsMoving())
            return 0;
        Boat.ChangeDirction();

        //计算两边的牧师和恶魔数量
        int rightPriestNum = 0, leftPriestNum = 0, rightDevilNum = 0, leftDevilNum = 0;
        for(int i = 0; i < 3; i++)
        {
            Priests[i].ChangeDirction();
            Devils[i].ChangeDirction();
            if(Priests[i].GetSide() == 1)
            {
                rightPriestNum++;
            }
            else
            {
                leftPriestNum++;
            }

            if (Devils[i].GetSide() == 1)
            {
                rightDevilNum++;
            }
            else
            {
                leftDevilNum++;
            }
        }
        if (leftPriestNum + leftDevilNum == 6)
        {
            for (int i = 0; i < 3; i++)
            {
                Devils[i].Lose();
            }
            gaming = false;
            return 1; //win
        }
        else if ((leftPriestNum > 0 && leftDevilNum > leftPriestNum) || (rightPriestNum > 0 && rightDevilNum > rightPriestNum))
        {
            int attackSide;
            if (leftDevilNum > leftPriestNum)
                attackSide = -1;
            else
                attackSide = 1;
            for(int i = 0; i < 3; i++)
            {
                if(Devils[i].GetSide() == attackSide)
                    Devils[i].Attack();
            }
            gaming = false;
            return -1; //lose
        }
        else
        {
            return 0; //continue
        }
    }

    public void MoveRole(RoleModel role)
    {
        if (!gaming)
            return;
        if (role.IsOnBoat())        //上岸
        {
            //如果还在前进，return
            if (Boat.IsMoving())
                return;

            //这里，为了和原游戏一致，使牧师和魔鬼在两边的排列顺序一致
            role = Boat.DeletePassenger(role);
            Vector3 direction;
            int id = role.GetName()[role.GetName().Length - 1] - '0';
            if (role.IsGood())
            {
                direction = PrisetsOriginPositions[id];
            }
            else
            {
                direction = DevilsOriginPositions[id];
            }
            if(role.GetSide() == -1)
            {
                direction.x = 0 - direction.x;
            }
            role.Move(direction);
            role.GoLand();
        }
        else                        //上船
        {
            if (Boat.IsFull() || Boat.GetSide() != role.GetSide())
            {
                return;
            }
            role.Move(Boat.getEmptyPosition());
            role.GoBoat(Boat);
            Boat.AddPassenger(role);
        }
    }

    public void MoveBoat()
    {
        //当船为空，或者船已经在动时，不允许移动
        if (!Boat.IsEmpty() && !Boat.IsMoving())
        {
            //当角色还在移动时，不允许移动，不然会出现Bug
            RoleModel[] temp = Boat.GetPassengers();
            for (int i = 0; i < 2; i++)
            {
                if (temp[i] != null && temp[i].IsMoving())
                    return;
            }

            Boat.Move();
        }
    }
}

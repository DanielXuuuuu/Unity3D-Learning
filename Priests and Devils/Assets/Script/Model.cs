using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandModel
{
    GameObject land;
    public LandModel(string position)
    {
        if(position == "left")
        {
            land = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Land"),
                                new Vector3(-6.5F, -1, 0), Quaternion.identity);
            land.name = "leftLand";
        }
        else
        {
            land = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Land"),
                                new Vector3(6.5F, -1, 0), Quaternion.identity);
            land.name = "rightLand";
        }
    }
}

public class BoatModel
{
    GameObject boat;
    //bool moving; //船是否在运动
    int side; //1表示在右边，-1表示在左边
    Vector3 rightPosition = new Vector3(3, -1, 0);
    Vector3 leftPosition = new Vector3(-3, -1, 0);
    Vector3[] rightPositions = new Vector3[] { new Vector3(2.5F, -0.8F, 0), new Vector3(3.5F, -0.8F, 0) };
    Vector3[] leftPositions = new Vector3[] { new Vector3(-2.5F, -0.8F, 0), new Vector3(-3.5F, -0.8F, 0) };
    RoleModel[] passengers = new RoleModel[2];
    Move move;
    public BoatModel()
    {
        //初始船在右边
        side = 1;
        boat = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Boat"),
                                new Vector3(3, -1, 0), Quaternion.Euler(0, 270, 0));
        boat.name = "boat";
        move = boat.AddComponent(typeof(Move)) as Move;
        for (int i = 0; i < 2; i++)
            passengers[i] = null;
    }

    public int GetSide()
    {
        return side;
    }
    public void ChangeDirction()
    {
        if(side == 1)
        {
            boat.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            boat.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    public bool IsEmpty()
    {
        for(int i = 0; i < 2; i++)
            if (passengers[i] != null)
                return false;
        return true;
    }

    public bool IsFull()
    {
        for (int i = 0; i < 2; i++)
            if (passengers[i] == null)
                return false;
        return true;
    }
    public bool IsMoving()
    {
        return move.IsMoving();
    }

    public void Reset()
    {
        side = 1;
        boat.transform.position = new Vector3(3, -1, 0);
        move.setPosition(leftPosition);
        move.SetMoveSate(0);
        passengers = new RoleModel[2];
    }

    public void Move()
    {
        if(side == 1)
        {
            move.setPosition(leftPosition);
            side = -1;
        }
        else
        {
            move.setPosition(rightPosition);
            side = 1;
        }
        //同时改变role的side
        for (int i = 0; i < 2; i++)
        {
            if (passengers[i] != null)
            {
                passengers[i].SetSide();
            }
        }
    }

    public int GetEmptyIndex()
    {
        for (int i = 0; i < passengers.Length; i++)
        {
            if (passengers[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public Vector3 getEmptyPosition()
    {
        Vector3 pos;
        int emptyIndex = GetEmptyIndex();
        if (side == 1)
        {
            pos = rightPositions[emptyIndex];
        }
        else
        {
            pos = leftPositions[emptyIndex];
        }
        return pos;
    }

    public GameObject GetBoat()
    {
        return boat;
    }

    public void AddPassenger(RoleModel passenger)
    {
        if (!IsFull()) 
            passengers[GetEmptyIndex()] = passenger;
    }

    public RoleModel DeletePassenger(RoleModel passenger)
    {
        for (int i = 0; i < 2; i++)
            if (passengers[i] != null && passengers[i].GetName() == passenger.GetName())
            {
                RoleModel role = passengers[i];
                passengers[i] = null;
                return role;
            }
        return null;
    }

    public RoleModel[] GetPassengers()
    {
        return passengers;
    }
}

public class RiverModel
{
    GameObject river;


    public RiverModel()
    {
        river = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/River"),
                                new Vector3(0, -1, 0), Quaternion.Euler(2, 0, 0));
        river.name = "river";
    }
}

public class RoleModel
{
    GameObject role;
    bool good;
    bool onBoat;
    int side;
    Click click;      
    Move move;

    public RoleModel(string roleType)
    {
        onBoat = false;
        side = 1;
        if (roleType == "priest")
        {
            role = Object.Instantiate(
                               Resources.Load<GameObject>("Prefabs/Priest"),
                               Vector3.zero, Quaternion.Euler(0, 270, 0));
            good = true;
        }
        else if (roleType == "devil")
        {
            role = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Devil"),
                                Vector3.zero, Quaternion.Euler(0, 270, 0));
            good = false;
        }
        move = role.AddComponent(typeof(Move)) as Move;
        click = role.AddComponent(typeof(Click)) as Click;
        click.SetRole(this);
    }

    public void Reset()
    {
        side = 1;
        role.transform.parent = null;
        onBoat = false;
    }

    //下面Attack、Idle、Lose三个函数为恶魔专属，用于设置恶魔动画，似乎有些不符合role的设定，用继承可能好一点
    public void Attack()
    {
        Animator anim = role.GetComponent<Animator>();
        anim.SetBool("attack", true);
    }
    public void Idle()
    {
        Animator anim = role.GetComponent<Animator>();
        anim.SetBool("attack", false);
        anim.SetBool("lose", false);
    }
    public void Lose()
    {
        Animator anim = role.GetComponent<Animator>();
        anim.SetBool("lose", true);
    }
    public bool IsGood() { return good; }
    public bool IsMoving()
    {
        return move.IsMoving();
    }
    public string GetName() { return role.name; }
    public void SetName(string name) { role.name = name; }
    public int GetSide() { return side; }
    public void SetSide(int s = 0)
    {
        if(s != 0)
        {
            side = s;
        }
        else 
            side = 0 - side;
    }
    public void ChangeDirction()
    {
        if (side == 1)
        {
            role.transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            role.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    public bool IsOnBoat() { return onBoat; }
    public void SetPosition(Vector3 pos) { role.transform.position = pos; }
    public Vector3 getPosition() { return role.transform.position; }
    public void Move(Vector3 position)
    {
        move.setPosition(position);
    }

    public void GoLand()
    {
        role.transform.parent = null;
        onBoat = false;
    }

    public void GoBoat(BoatModel boat)
    {
        role.transform.parent = boat.GetBoat().transform;
        onBoat = true;
    }
}

public class Move : MonoBehaviour
{
    float speed = 4;  //移动速度
    Vector3 endPosition, middlePosition;
    int moveState = 0;  //0表示不移动, 1表示水平移动，2表示竖直移动
    public void SetMoveSate(int state)
    {
        moveState = state;
    }
    public bool IsMoving()
    {
        return moveState != 0;
    }
    public void setPosition(Vector3 position)
    {
        endPosition = position;
        if (position.y == transform.position.y)         //y值不变：船的移动
        {
            SetMoveSate(2);
        }
        else
        {
            if (position.y < transform.position.y)      //y值减小：角色从陆地到船
            {
                middlePosition = new Vector3(position.x, transform.position.y, position.z);

            }
            else                                          //y值增大：角色从船到陆地
            {
                middlePosition = new Vector3(transform.position.x, position.y, position.z);
            }
            SetMoveSate(1);
        }
    }
    void Update()
    {
        if (moveState == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, middlePosition, speed * Time.deltaTime);
            if (transform.position == middlePosition)
                SetMoveSate(2);
        }
        else if (moveState == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
                SetMoveSate(0);
        }
    }
}

public class Click : MonoBehaviour
{
    IUserAction action;
    RoleModel role;
    public void SetRole(RoleModel r)
    {
        role = r;
    }
    void Start()
    {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }
    void OnMouseDown()
    {
        if (role == null)
        {
            return;
        }
        action.MoveRole(role);
    }
}


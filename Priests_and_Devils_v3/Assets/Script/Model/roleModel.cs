using UnityEngine;
using System.Collections;

public class RoleModel
{
    GameObject role;
    bool good;
    bool onBoat;
    int side;
    Click click;

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
        click = role.AddComponent(typeof(Click)) as Click;
        click.SetRole(this);
    }

    public GameObject GetRole()
    {
        return role;
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
    public string GetName() { return role.name; }
    public void SetName(string name) { role.name = name; }
    public int GetSide() { return side; }
    public void SetSide(int s = 0)
    {
        if (s != 0)
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
    public Vector3 GetPosition() { return role.transform.position; }
   
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
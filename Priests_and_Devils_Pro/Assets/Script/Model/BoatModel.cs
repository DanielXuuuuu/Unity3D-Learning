using UnityEngine;
using System.Collections;

public class BoatModel
{
    GameObject boat;
    int side; //1表示在右边，-1表示在左边
    Vector3 rightPosition = new Vector3(3, -1, 0);
    Vector3 leftPosition = new Vector3(-3, -1, 0);
    Vector3[] rightPositions = new Vector3[] { new Vector3(2.5F, -0.8F, 0), new Vector3(3.5F, -0.8F, 0) };
    Vector3[] leftPositions = new Vector3[] { new Vector3(-2.5F, -0.8F, 0), new Vector3(-3.5F, -0.8F, 0) };
    RoleModel[] passengers = new RoleModel[2];
    public BoatModel()
    {
        //初始船在右边
        side = 1;
        boat = Object.Instantiate(
                                Resources.Load<GameObject>("Prefabs/Boat"),
                                new Vector3(3, -1, 0), Quaternion.Euler(0, 270, 0));
        boat.name = "boat";
        for (int i = 0; i < 2; i++)
            passengers[i] = null;
    }

    public int GetSide()
    {
        return side;
    }
    public void ChangeDirction()
    {
        if (side == 1)
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
        for (int i = 0; i < 2; i++)
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

    public void Reset()
    {
        side = 1;
        boat.transform.position = new Vector3(3, -1, 0);
        passengers = new RoleModel[2];
    }

    public Vector3 GetMoveDirection()
    {
        for (int i = 0; i < 2; i++)
        {
            if (passengers[i] != null)
            {
                passengers[i].SetSide();
            }
        }
        if (side == 1)
        {
            side = -1;
            return leftPosition;
        }
        else
        {
            side = 1;
            return rightPosition;
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

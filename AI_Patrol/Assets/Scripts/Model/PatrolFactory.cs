using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolFactory : MonoBehaviour
{
    private GameObject patrolPrefab = null;
    private Vector3[] position = new Vector3[6]; //记录巡逻兵的位置
    private List<GameObject> patrols = new List<GameObject>();

    public List<GameObject> GetPatrols()
    {
        int[] pos_x = { -10, 3, 18, -11, 3, 15 };
        int[] pos_z = { -18, -15, -15, 12, 10, 13 };

        for(int i = 0; i < 6; i++)
        {
            position[i] = new Vector3(pos_x[i], 0, pos_z[i]);
            patrolPrefab = Object.Instantiate(Resources.Load<GameObject>("Prefabs/patrol2"), position[i], Quaternion.identity);
            patrolPrefab.name = "patrol" + i;
            patrolPrefab.AddComponent<PatrolData>();

            patrolPrefab.GetComponent<PatrolData>().manageFloor = i + 1;
            patrolPrefab.GetComponent<PatrolData>().initPosition = position[i];
            patrols.Add(patrolPrefab);
        }
        return patrols;
    }

    public void Reset()
    {
        for(int i = 0; i < patrols.Count; i++)
        {
            patrols[i].transform.position = position[i];
            patrols[i].GetComponent<Animator>().SetBool("shoot", false);
        }
    }

}
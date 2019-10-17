using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiskFactory : MonoBehaviour
{
    public GameObject diskPrefab = null;
    private List<DiskData> used = new List<DiskData>();
    private List<DiskData> free = new List<DiskData>();

    public GameObject GetDisk(int round)
    {
        if (free.Count > 0)
        {
            diskPrefab = free[0].gameObject;
            free.Remove(free[0]);
        }
        else
        {
            diskPrefab = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/disk"), Vector3.zero, Quaternion.identity);
            diskPrefab.AddComponent<DiskData>();
        }

        int start = 0, end = 0, diskType = 0;
        if(round == 1)
        {
            start = 0;
            end = 300;
        }
        if (round == 2) {
            start = 200;
            end = 400;
        }
        else if (round == 3)
        {
            start = 300;
            end = 500;
        }

        int temp = Random.Range(start, end);

        if (temp > 400)
        {
            diskType = 5;
        }
        else if (temp > 300)
        {
            diskType = 4;
        }
        else if (temp > 200)
        {
            diskType = 3;
        }
        else if (temp > 100)
        {
            diskType = 2;
        }
        else
        {
            diskType = 1;
        }

        //生成不同的飞碟
        switch (diskType)
        {
            case 1:
                {
                    diskPrefab.GetComponent<DiskData>().color = Color.white;
                    diskPrefab.GetComponent<DiskData>().speed = 4.0f;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    diskPrefab.GetComponent<DiskData>().direction = new Vector3(RanX, 1, 0);
                    diskPrefab.GetComponent<Renderer>().material.color = Color.white;
                    diskPrefab.GetComponent<DiskData>().score = 1;
                    diskPrefab.transform.localScale = new Vector3(2f, 0.2f, 2f);
                    break;
                }
            case 2:
                {
                    diskPrefab.GetComponent<DiskData>().color = Color.green;
                    diskPrefab.GetComponent<DiskData>().speed = 6.0f;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    diskPrefab.GetComponent<DiskData>().direction = new Vector3(RanX, 1, 0);
                    diskPrefab.GetComponent<Renderer>().material.color = Color.green;
                    diskPrefab.GetComponent<DiskData>().score = 2;
                    diskPrefab.transform.localScale = new Vector3(1.6f, 0.16f, 1.6f);
                    break;
                }
            case 3:
                {
                    diskPrefab.GetComponent<DiskData>().color = Color.blue;
                    diskPrefab.GetComponent<DiskData>().speed = 8.0f;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    diskPrefab.GetComponent<DiskData>().direction = new Vector3(RanX, 1, 0);
                    diskPrefab.GetComponent<Renderer>().material.color = Color.blue;
                    diskPrefab.GetComponent<DiskData>().score = 3;
                    diskPrefab.transform.localScale = new Vector3(1.4f, 0.14f, 1.4f);
                    break;
                }
            case 4:
                {
                    diskPrefab.GetComponent<DiskData>().color = Color.red;
                    diskPrefab.GetComponent<DiskData>().speed = 6.0f;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    diskPrefab.GetComponent<DiskData>().direction = new Vector3(RanX, 1, 0);
                    diskPrefab.GetComponent<Renderer>().material.color = Color.red;
                    diskPrefab.GetComponent<DiskData>().score = 4;
                    diskPrefab.transform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
                    break;
                }
            case 5:
                {
                    diskPrefab.GetComponent<DiskData>().color = Color.black;
                    diskPrefab.GetComponent<DiskData>().speed = 8.0f;
                    float RanX = UnityEngine.Random.Range(-1f, 1f) < 0 ? -1 : 1;
                    diskPrefab.GetComponent<DiskData>().direction = new Vector3(RanX, 1, 0);
                    diskPrefab.GetComponent<Renderer>().material.color = Color.black;
                    diskPrefab.GetComponent<DiskData>().score = 5;
                    diskPrefab.transform.localScale = new Vector3(0.8f, 0.08f ,0.8f);
                    break;
                }
            default:break;

        }

        used.Add(diskPrefab.GetComponent<DiskData>());
        diskPrefab.SetActive(false);
        return diskPrefab;
    }

    public void FreeDisk(GameObject disk)
    {
        foreach (DiskData dd in used)
        {
            if(disk.GetInstanceID() == dd.gameObject.GetInstanceID())
            {
                dd.gameObject.SetActive(false);
                used.Remove(dd);
                free.Add(dd);
                break;
            }
        }
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinFactory : MonoBehaviour
{
    private GameObject coinPrefab = null;
    private Vector3[] position = new Vector3[6]; //记录金币的位置
    private List<GameObject> coins = new List<GameObject>();

    public List<GameObject> GetCoins()
    {
        int[] pos_x = {-15, 3, 13, -7, 9};
        int[] pos_z = {-15, -3, -15, 15, 10};

        for (int i = 0; i < 5; i++)
        {
            position[i] = new Vector3(pos_x[i], 1, pos_z[i]);
            coinPrefab = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Coin"), position[i], Quaternion.identity);
            coinPrefab.name = "coin" + i;
            coins.Add(coinPrefab);
        }
        return coins;
    }

    public void Reset()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            coins[i].SetActive(true);
        }

    }

}
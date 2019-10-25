using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    private float gameTime = 90f;
    private float timer = 0;
    private string timeCounter;

    // Use this for initialization
    void Start()
    {

    }

    public void Reset()
    {
        gameTime = 90f;
    }

    public string GetTimeText()
    {
        return timeCounter;
    }

    // Update is called once per frame
    void Update()
    {
        int M = (int)(gameTime / 60);
        float S = gameTime % 60;

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0;
            gameTime--;
            timeCounter = M.ToString() + ":" + string.Format("{0:00}", S);
        }

        if (gameTime == 0)
        {
            Singleton<GameEventManager>.Instance.TimeOut();
        }
    }
}

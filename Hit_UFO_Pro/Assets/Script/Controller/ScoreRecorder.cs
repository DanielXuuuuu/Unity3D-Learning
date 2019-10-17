using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder
{
    private int score;

    public ScoreRecorder()
    {
        score = 0;
    }

    public void Record(GameObject disk)
    {
        score += disk.GetComponent<DiskData>().score;
    }

    public int GetScore()
    {
        return score;
    }

    public void Reset()
    {
        score = 0;
    }
}
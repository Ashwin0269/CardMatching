using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    const string SCORE_KEY = "score";

    public static void Save(int score)
    {
        PlayerPrefs.SetInt(SCORE_KEY, score);
        PlayerPrefs.Save();
    }

    public static int Load()
    {
        return PlayerPrefs.GetInt(SCORE_KEY, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore_Level : MonoBehaviour
{ 
    public static HighScore_Level Instance;

    public int playerScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

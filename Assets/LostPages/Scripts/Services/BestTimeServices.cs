using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestTimeServices : MonoBehaviour
{
    public static BestTimeServices instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SendGameDuration(float gameDuration, int sceneNum)
    {
        // Construire une cl� unique bas�e sur le num�ro de la sc�ne
        string sceneKey = $"BestTime_Scene_{sceneNum}";

        float existingBestTime = PlayerPrefs.GetFloat(sceneKey, float.MaxValue);

        if (gameDuration < existingBestTime)
        {
            PlayerPrefs.SetFloat(sceneKey, gameDuration);
        }
    }

    public float GetGameDuration(int sceneNum)
    {
        string sceneKey = $"BestTime_Scene_{sceneNum}";
        float existingBestTime = PlayerPrefs.GetFloat(sceneKey, -1);
        return existingBestTime;
    }
}

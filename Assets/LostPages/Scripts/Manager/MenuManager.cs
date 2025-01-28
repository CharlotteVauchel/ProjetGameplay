using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreScene1;
    [SerializeField] private TMP_Text highScoreScene2;
    [SerializeField] private TMP_Text highScoreScene3;

    // Start is called before the first frame update
    void Start()
    {
        List<TMP_Text> highScores = new List<TMP_Text>
        {
            highScoreScene1,
            highScoreScene2,
            highScoreScene3
        };

        if (BestTimeServices.instance != null)
        {
            int numScene = 1;
            foreach(TMP_Text highScore in highScores)
            {
                float bestTime = BestTimeServices.instance.GetGameDuration(numScene);

                if (bestTime == -1)
                {
                    highScore.text = "No high Score Yet";
                }
                else
                {
                    highScore.text = "High Score : " + bestTime.ToString();
                }
                numScene++;
            }
        }
    }
    public void Quit()
    {
        #if UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void LoadLevelMenu()
    {
        SceneManager.LoadScene("LevelsMenu");
    }

    public void QuitGame()
    {
        Quit();
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
}

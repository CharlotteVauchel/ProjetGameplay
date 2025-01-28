using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int nbPagesToFind = 3;
    [SerializeField] private int nbBooksToFind = 1;

    private float timeOfStart;

    private bool gameOver = false;

    private int sceneNumber;

    string score = "";
    string end_message = "";

    TMP_Text txt_time;
    TMP_Text txt_title_end;
    TMP_Text txt_score;
    TMP_Text rule;
    GameObject panel_end;

    // Start is called before the first frame update
    void Start()
    {
        txt_time = GameObject.Find("txt_time")?.GetComponent<TMP_Text>();
        panel_end = GameObject.Find("Panel_End");
        txt_title_end = GameObject.Find("txt_title_end")?.GetComponent<TMP_Text>();
        txt_score = GameObject.Find("txt_score")?.GetComponent<TMP_Text>();
        rule = GameObject.Find("txt_rule")?.GetComponent<TMP_Text>();
        panel_end.SetActive(false);

        UpdateFindings();

        UpdateAvailability(false);
        sceneNumber = Int32.Parse(SceneManager.GetActiveScene().name.Split("Level")[1]);

        ScoreManager.instance.OnScoreChanged += CheckScore;
        timeOfStart = Time.time;
    }

    private void CheckScore(int nbItem, string item)
    {
        switch (item)
        {
            case "books":
                if (nbItem == nbBooksToFind)
                    GameWon();
                break;
            case "pages":
                if (nbItem == nbPagesToFind)
                    UpdateAvailability(true);
                break;
            case "health":
                if (nbItem <= 0)
                    GameLost();
                break;
        }
    }

    private void GameWon()
    {
        gameOver = true;
        panel_end.SetActive(true);

        float time = Time.time - timeOfStart;

        end_message = "YOU WIN !";
        score = "Score : " + GetTimeFormatted(time);

        BestTimeServices.instance.SendGameDuration(time, sceneNumber);
    }

    private void GameLost()
    {
        gameOver = true;
        end_message = "GAME OVER";
        panel_end.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        txt_score.text = score;
        txt_title_end.text = end_message;

        if (gameOver)
            return;
        txt_time.text = GetTimeFormatted(Time.time - timeOfStart);
    }

    void UpdateAvailability(bool availability)
    {
        if (availability)
            rule.text = "Pages complete, find the missing book";
        // Trouver tous les objets de type Transform (inclut actifs et inactifs)
        Transform[] allObjects = Resources.FindObjectsOfTypeAll<Transform>();

        foreach (Transform obj in allObjects)
        {
            if (obj.CompareTag("Book")) // Vérifie si l'objet a le tag "Book"
            {
                obj.gameObject.SetActive(availability);
            }
        }
    }

    void UpdateFindings()
    {
        TMP_Text txtPagesPoints = GameObject.Find("txt_pages_points")?.GetComponent<TMP_Text>();
        TMP_Text txtBooksPoints = GameObject.Find("txt_books_points")?.GetComponent<TMP_Text>();

        txtPagesPoints.text = nbPagesToFind.ToString();
        txtBooksPoints.text = nbBooksToFind.ToString();

    }

    string GetTimeFormatted(float time)
    {
        // Calcul des minutes, secondes et millisecondes
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        // Formatage en chaîne
        string timeFormatted = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        return timeFormatted;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
}

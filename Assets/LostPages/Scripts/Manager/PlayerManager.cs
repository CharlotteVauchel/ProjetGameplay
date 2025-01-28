using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerManager : MonoBehaviour
{
    private TMP_Text pages;
    private TMP_Text books;

    private GameObject[] hearts = new GameObject[3];


    private void Start()
    {
        TMP_Text title = GameObject.Find("txt_title")?.GetComponent<TMP_Text>();
        pages = GameObject.Find("txt_pages_nb_points")?.GetComponent<TMP_Text>();
        books = GameObject.Find("txt_books_nb_points")?.GetComponent<TMP_Text>();

        hearts[0] = GameObject.Find("Heart_1");
        hearts[1] = GameObject.Find("Heart_2");
        hearts[2] = GameObject.Find("Heart_3");

        string sceneNumber = SceneManager.GetActiveScene().name.Split("Level")[1];

        title.text = "Level " + sceneNumber;
    }

    private void Update()
    {
        pages.text = ScoreManager.instance.ScorePages.ToString() + "/";
        books.text = ScoreManager.instance.ScoreBooks.ToString() + "/";

        int cpt = 1;
        foreach(GameObject heart in hearts)
        {
            heart.SetActive(cpt<=ScoreManager.instance.HealthNb);
            cpt++;
        }
    }

}

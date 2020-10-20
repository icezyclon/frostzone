using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    void Update()
    {

    }

    public void PlayGame()
    {
        ScoreScript score = GameObject.FindObjectOfType<ScoreScript>();
        if (score != null)
        {
            score.score = 0;
        }
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

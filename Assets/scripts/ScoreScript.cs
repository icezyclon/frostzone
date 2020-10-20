using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public uint score = 0;

    private Rect _scoreRect;
    private GUIStyle _scoreStyle;

    private static ScoreScript instance = null;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        _scoreRect = new Rect(670, 50, 500, 100); ;
        _scoreStyle = new GUIStyle();
        _scoreStyle.fontSize = 30;
        _scoreStyle.normal.textColor = Color.white;
    }

    private void OnGUI()
    {
        GUI.Label(_scoreRect, "SCORE: " + string.Format("{0}", score), _scoreStyle);
    }
}
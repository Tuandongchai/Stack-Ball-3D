using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    protected static ScoreManager instance;
    public static ScoreManager Instance { get =>instance; }

    private Text scoreText;
    [SerializeField] protected int score = 0;
    public int Score { get => score; }

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        MakeSingleton();
    }
    private void Start()
    {
        AddScore(0);
    }
    private void Update()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        }
    }
    void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        if (score > PlayerPrefs.GetInt("HighScore", 0))
            PlayerPrefs.SetInt("HighScore", score);
        scoreText.text = score.ToString();
    }
    public void ResetScore()
    {
        score = 0;
    }
}

using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public int Lives => _lives;
    public long Score => _score;
    public int Level => _level;
    
    private long _score;
    private int _lives;
    private int _level;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this; 
        }
        
        DontDestroyOnLoad(this);
        InitializeStartValues();
    }

    private void Update()
    {
        DebugText.Instance.SetText($"Lives = [{Lives}] Score = {Score}");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = (Time.timeScale == 0) ? 1f : 0f;
        }
    }

    private void InitializeStartValues()
    {
        _score = 0;
        _lives = 3;
        _level = 0; 
    }

    public void AddScore(int value)
    {
        _score += value;
    }
    
    public void KillPlayer()
    {
        _lives--; 
    }
    
}

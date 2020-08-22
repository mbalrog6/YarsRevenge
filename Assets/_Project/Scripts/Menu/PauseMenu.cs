using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TextlivesDigits;
    [SerializeField] private TextMeshProUGUI TextLevelDigits;
    [SerializeField] private TextMeshProUGUI TextScoreDigits;

    private void OnEnable()
    {
        GameManager.Instance.OnLevelChanged += UpdateLevel;
        GameManager.Instance.OnLivesChanged += UpdateLives;
        GameManager.Instance.OnScoreChanged += UpdateScore; 
    }

    private void Start()
    {
        UpdateLevel(GameManager.Instance.Level);
        UpdateLives(GameManager.Instance.Lives);
        UpdateScore(GameManager.Instance.Score);
    }

    private void UpdateLevel(int level)
    {
        TextLevelDigits.text = level.ToString();
    }

    private void UpdateLives(int lives)
    {
        TextlivesDigits.text = lives.ToString();
    }

    private void UpdateScore(long score)
    {
        TextScoreDigits.text = score.ToString();
    }
}

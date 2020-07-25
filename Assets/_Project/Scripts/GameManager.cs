using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Barrier _barrier;
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public int Lives => _lives;
    public long Score => _score;
    public int Level => _level;
    public bool IsPaused { get; private set; } 
    
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

    private void Start()
    {
        SetBarrierPosition();
    }

    private void Update()
    {
        DebugText.Instance.SetText($"Lives = [{Lives}] Score = {Score}");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = (Time.timeScale == 0) ? 1f : 0f;
            if (Time.timeScale > 0.1f)
            {
                IsPaused = false; 
            }
            else
            {
                IsPaused = true; 
            }
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

    public void SetBarrierPosition()
    {
        Rect screenBounds = ScreenHelper.Instance.ScreenBounds;
        float top, bottom;
        top = screenBounds.yMax - 1.5f - _barrier.BarrierRectContainer.Bounds.height;
        bottom = screenBounds.yMin + 1.5f;// + _barrier.BarrierRectContainer.Bounds.height / 2f;
        _barrier.BottomLimit = bottom;
        _barrier.UpperLimit = top;
        
        Vector3 position = ScreenHelper.Instance.ScreenBounds.center;
        position.x = ScreenHelper.Instance.ScreenBounds.xMax - 3f;
        position.y -= _barrier.BarrierRectContainer.Bounds.height / 2f; 
        position.z = 0f;
        _barrier.transform.position = position;
    }
    
}

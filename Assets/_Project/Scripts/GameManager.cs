using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Barrier2 _barrier;
    [SerializeField] private EntityStateMachine _entityStateMachine; 
    [SerializeField] private GameObject _barrierCell;
    [SerializeField] private LevelInfo[] levelData;
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public event Action<Barrier2> OnBarrierChanged;
    public event Action<long> OnScoreChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnLivesChanged;

    public int Lives => _lives;
    public long Score => _score;
    public int Level => _level;
    public bool IsPaused { get; private set; }

    private long _score;
    private int _lives;
    private int _level;

    private BarrierFactory _barrierFactory;
    [SerializeField] private BarrierInfo _barrierInfo;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);

            InitializeStartValues();
            _barrierFactory = new BarrierFactory(_barrierCell);

            _barrierFactory.LoadBarriersInMemory();
            _level = -1;
        }
    }

    private void Start()
    {
        AdvanceToNextLevel();
    }

    private void Update()
    {
        //DebugText.Instance.SetText($"Lives = [{Lives}] Score = {Score}");
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
        OnScoreChanged?.Invoke(_score);
    }

    public void KillPlayer()
    {
        _lives--;
        OnLivesChanged?.Invoke(_lives);
    }

    public void SetBarrierPosition()
    {
        Rect screenBounds = ScreenHelper.Instance.ScreenBounds;
        float top, bottom;
        top = screenBounds.yMax - 1.5f - _barrier.BarrierRectContainer.Bounds.height;
        bottom = screenBounds.yMin + 1.5f;
        _barrier.SetHighAndLowBoundsLimitForOscalator(bottom, top);

        Vector3 position = ScreenHelper.Instance.ScreenBounds.center;
        position.x = ScreenHelper.Instance.ScreenBounds.xMax - 3f;
        position.y -= _barrier.BarrierRectContainer.Bounds.height / 2f;
        position.z = 0f;
        _barrier.transform.position = position;
    }

    public void AdvanceToNextLevel()
    {
        _level++;
        
        _barrier = _barrierFactory.GetBarrier(levelData[_level].BarrierInfo).GetComponent<Barrier2>();
        SetBarrierPosition();
        
        _entityStateMachine.LoadNewQotileInfo(levelData[_level].QotileInfo);
        
        OnBarrierChanged?.Invoke(_barrier);
    }
}
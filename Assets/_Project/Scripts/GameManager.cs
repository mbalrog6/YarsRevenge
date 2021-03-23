using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Level Prefabs")] [SerializeField]
    private Barrier2 _barrier;

    [SerializeField] private Player _player;
    [SerializeField] private EntityStateMachine _entityStateMachine;
    [SerializeField] private Probe _probe;
    [SerializeField] private IonZone _ionZone;
    [Space(10)] [SerializeField] private GameObject _barrierCell;
    [SerializeField] private LevelInfo[] levelData;

    [SerializeField] private float barrierStartXOffset;
    public static GameManager Instance => _instance;
    private static GameManager _instance;

    public event Action<Barrier2> OnBarrierChanged;
    public event Action<long> OnScoreChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnLivesChanged;

    public bool TutorialActive { get; set; } = true;
    public int Lives => _lives;
    public long Score => _score;
    public int Level => _level;
    public bool IsPaused { get; private set; }

    private long _score;
    private int _lives;
    private int _level;
    private int _tutorialIndex;
    private float _tutorialTimer;
    private bool _tutorialTimerSet = false;
    private float _pausedTime;

    private BarrierFactory _barrierFactory;

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
            _tutorialIndex = 0;
        }
    }

    private void Start()
    {
        AdvanceToNextLevel();
        CameraShake.OnStartShake += ScreenHelper.FreezeBounds;
        CameraShake.OnEndShake += ScreenHelper.Thaw;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            AdvanceToNextLevel();
        }

        if (GameStateMachine.Instance.CurrentState == States.BRIEF_PAUSE ||
            GameStateMachine.Instance.CurrentState == States.PAUSE)
        {
            _pausedTime += Time.deltaTime;
            return;
        }
        else if (_pausedTime > 0)
        {
            _tutorialTimer += _pausedTime;
            _pausedTime = 0;
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
        position.x = ScreenHelper.Instance.ScreenBounds.xMax - barrierStartXOffset;
        position.y -= _barrier.BarrierRectContainer.Bounds.height / 2f;
        position.z = 0f;
        _barrier.transform.position = position;
    }

    public void AddLife(int value)
    {
        _lives += value;
        OnLivesChanged?.Invoke(_lives);
    }

    public void AdvanceToNextLevel()
    {
        if (_barrier != null)
        {
            Destroy(_barrier.gameObject);
        }

        _level++;

        _barrier = _barrierFactory.GetBarrier(levelData[_level].BarrierInfo).GetComponent<Barrier2>();
        SetBarrierPosition();

        _entityStateMachine.LoadNewQotileInfo(levelData[_level].QotileInfo);

        _probe.Reset(levelData[_level].ProbeInfo);
        if (levelData[_level].HasIonField)
        {
            _ionZone.gameObject.SetActive(true);
        }
        else
        {
            _ionZone.gameObject.SetActive(false);
        }

        OnBarrierChanged?.Invoke(_barrier);
    }
}
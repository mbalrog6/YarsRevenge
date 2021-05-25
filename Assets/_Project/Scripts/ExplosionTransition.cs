using DarkTonic.MasterAudio;
using UnityEngine;

public class ExplosionTransition : MonoBehaviour
{
    [SerializeField] private Color darkColor;
    [SerializeField] private Color brightColor;
    [SerializeField] private float _speed;
    [SerializeField] private float _closingSpeed;

    public bool IsActive { get; set; }

    private Color _currentColor;
    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;
    private float _timer = 0f;
    private float _currentCloseValue;
    private ExplosionTransionFinishedCommand _explosionCommnand;

    private bool _flashFinished = false;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _currentCloseValue = 0f;
        _explosionCommnand = new ExplosionTransionFinishedCommand();
        _explosionCommnand.IsFinished = true;
        IsActive = false;
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<ExplosionTransionStartCommand>(StartTransision);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IsActive = true;
            MasterAudio.PlaySoundAndForget("SwooshExplosion");
        }
        
        if (!IsActive)
            return;
        
        _timer += Time.deltaTime;
        var value = _timer / _speed;
        _renderer.GetPropertyBlock(_propBlock);
        
        if( !_flashFinished )
        {
            _propBlock.SetColor("_MainColor", Color.Lerp(darkColor, brightColor, value));
            _propBlock.SetVector("_Offset", new Vector4(_currentCloseValue, 1,0,0));
            _renderer.SetPropertyBlock(_propBlock);
            if (value >= 1f)
            {
                _flashFinished = true;
                _timer = 0f;
            }
            return; 
        }

        _currentCloseValue -= Time.deltaTime * _closingSpeed;
        if (_currentCloseValue <= -2.2f)
        {
            _currentCloseValue = 0f;
            _flashFinished = false;
            _timer = 0f;
            IsActive = false;
            Mediator.Instance.Publish(_explosionCommnand);
            CameraShake.Shake();
            return;
        }
        _propBlock.SetVector("_Offset", new Vector4(_currentCloseValue, 1,0,0));
        _renderer.SetPropertyBlock(_propBlock);
        
    }

    public void StartTransision(ExplosionTransionStartCommand explosionStartCommand)
    {
        IsActive = true;
    }
}

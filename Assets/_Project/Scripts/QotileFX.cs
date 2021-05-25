using UnityEngine;

public class QotileFX : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _duration;
    private MaterialPropertyBlock _fxProperties;
    private Material _fxMaterial;
    private AnimationCurve _easeInOut;
    private float _direction = 1;
    private float _shineLocation = 0;
    private float _timer = 0f;
    public bool IsPaused { get; private set; }


    private void Awake()
    {
        _fxMaterial = _spriteRenderer.material;
        _fxProperties = new MaterialPropertyBlock();
        _easeInOut = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        DisableFX();
    }

    private void Update()
    {
        if (IsPaused)
            return; 
        
        _spriteRenderer.GetPropertyBlock(_fxProperties);
        _fxProperties.SetFloat("_ShineLocation", GetShineLocation());
        _spriteRenderer.SetPropertyBlock(_fxProperties);
    }

    private float GetShineLocation()
    {
        _timer += Time.deltaTime * _direction;
        _shineLocation = _easeInOut.Evaluate(_timer / _duration);
        Mathf.Clamp01(_shineLocation);

        _direction = (_timer > _duration) ? -1 : (_timer < 0) ? 1 : _direction;
        return _shineLocation;
    }

    public void EnableFX()
    {
        if (IsPaused == false)
            return;

        _fxMaterial.EnableKeyword("OUTBASE_ON");
        _fxMaterial.EnableKeyword("SHINE_ON");
        IsPaused = false;
    }

    public void DisableFX()
    {
        if (IsPaused == true)
            return;
        
        _fxMaterial.DisableKeyword("OUTBASE_ON");
        _fxMaterial.DisableKeyword("SHINE_ON");
        IsPaused = true;
    }
}

using UnityEngine;

public class FXFadeOut : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private float duration;
    private Material rendererMaterial; 
    private bool _isRunning = false;
    private float _timer = 0f;
    private float _value;

    private void Awake()
    {
        rendererMaterial = renderer.material;
    }

    private void Update()
    {
        if (!_isRunning) 
            return;
        
        _timer += Time.deltaTime;
        _value = _timer / duration;
        Mathf.Clamp(_value, 0, 1f);
            
        rendererMaterial.SetFloat("_FadeAmount", _value);

        if (_value >= 1f)
        {
            _isRunning = false;
            _timer = 0f; 
            rendererMaterial.SetFloat("_FadeAmount", 0);
        }
    }

    public void StartFade() => _isRunning = true;
}

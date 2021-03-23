using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flicker : MonoBehaviour
{
    private Material _spriteMaterial;
    private Color _originalColor;
    private Color _workingColor;

    [SerializeField] private int min = -2;
    [SerializeField] private int max = 2;
    [Range(1, 100)]
    [SerializeField] private int percentage = 50; 

    private float _h;
    private float _s;
    private float _v;
    private int _startValue = 0;
    private bool _direction = true; 
    private void Awake()
    {
        _spriteMaterial = GetComponent<SpriteRenderer>().material;
        _originalColor = _spriteMaterial.color;
        _workingColor = _originalColor;
        Color.RGBToHSV(_originalColor, out _h, out _s, out _v);
    }

    private void Update()
    {
        if (Random.Range(0, 101) > percentage)
        {
            _startValue = _direction ? _startValue + 1 : _startValue - 1;
            
            if (_startValue > max)
            {
                _startValue = max;
                _direction = !_direction;
            }

            if (_startValue < min)
            {
                _startValue = min;
                _direction = !_direction;
            }
            
            _spriteMaterial.color = Color.HSVToRGB(_h, _s, _v + _startValue * 0.01f);
        }
        
    }
}

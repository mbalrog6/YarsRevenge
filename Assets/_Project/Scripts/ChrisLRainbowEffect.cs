using System;
using UnityEngine;

public class ChrisLRainbowEffect : MonoBehaviour
{
    [SerializeField] private KeyCode _secretKey;
    private ActiveText _rainbowEnabler;
    public bool IsRainbow { get; private set; } 

    private void Awake()
    {
        _rainbowEnabler = GetComponent<ActiveText>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_secretKey))
        {
            if (_rainbowEnabler != null)
            {
                if (IsRainbow == false)
                {
                    _rainbowEnabler.AddRainbowToAll();
                    IsRainbow = true;
                }
                else
                {
                    _rainbowEnabler.UpdateAllUiText();
                    IsRainbow = false;
                }
            }
        }
    }
}

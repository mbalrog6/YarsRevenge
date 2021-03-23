using UnityEngine;
using UnityEngine.UI;

public class AmmoUpdater : MonoBehaviour
{
    private Material _material;
    private int _floatIndex;
    [SerializeField] private int _maxValue;
    public int ammo;

    private void Awake()
    {
        _material = GetComponent<Image>().material;
        _floatIndex = Shader.PropertyToID("_percentage");
        _material.SetFloat(_floatIndex, 0f);
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<AmmoUpdateCommand>(SetPercentace);
    }


    public void SetPercentace(AmmoUpdateCommand ammoUpdate)
    {
        var percent = Mathf.Clamp(ammoUpdate.AmmoValue, 0, _maxValue);
        ammo = percent;
        _material.SetFloat(_floatIndex, (float)percent / _maxValue);
    }
}
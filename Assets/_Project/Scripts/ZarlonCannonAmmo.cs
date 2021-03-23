using System;
using System.Collections.Generic;
using UnityEngine;

public class ZarlonCannonAmmo : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int initialNumberOfIcons;
    private Stack<GameObject> _displayedIcons;
    private Stack<GameObject> _inactiveIcons;
    private RectTransform _parentTransform;
    private int _differenceInAmmont;

    private void Awake()
    {
        _displayedIcons = new Stack<GameObject>();
        _inactiveIcons = new Stack<GameObject>();

        _parentTransform = GetComponent<RectTransform>();
        
        CreateMoreIcons(initialNumberOfIcons);
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<UpdateZarlonCannonCommand>(UpdateZarlonCannon);
    }

    public int HowManyIconsDisplayed => _displayedIcons.Count;

    private void AddIcon()
    {
        if (_inactiveIcons.Count <= 0)
        {
            CreateMoreIcons(initialNumberOfIcons);
        }

        var go = _inactiveIcons.Pop();
        go.SetActive(true);
        _displayedIcons.Push(go);
    }

    private void RemoveIcon()
    {
        if (_displayedIcons.Count == 0)
            return;

        var go = _displayedIcons.Pop();
        go.SetActive(false);
        _inactiveIcons.Push(go);
    }

    public void UpdateZarlonCannon(UpdateZarlonCannonCommand zarlonCannonCommand)
    {
        if (HowManyIconsDisplayed < zarlonCannonCommand.NumberOfCannonShots)
        {
            _differenceInAmmont = zarlonCannonCommand.NumberOfCannonShots - HowManyIconsDisplayed;
            for (var i = 0; i < _differenceInAmmont; i++)
            {
                AddIcon();
            }
        }

        if (HowManyIconsDisplayed > zarlonCannonCommand.NumberOfCannonShots)
        {
            _differenceInAmmont = HowManyIconsDisplayed - zarlonCannonCommand.NumberOfCannonShots;
            for (var i = 0; i < _differenceInAmmont; i++)
            {
                RemoveIcon();
            }
        }
    }

    private void CreateMoreIcons(int ammountToCreate)
    {
        GameObject icon;
        for (var i = 0; i < ammountToCreate; i++)
        {
            icon = Instantiate(prefab, _parentTransform);
            icon.SetActive(false);
            _inactiveIcons.Push(icon);
        }
    }
}
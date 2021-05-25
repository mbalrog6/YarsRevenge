using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ActiveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textUI;

    [SerializeField] private bool _addToStaticList;
    [SerializeField] private bool _startsActive;
    [SerializeField] private string _text;
    [SerializeField] private string _activePrefix;
    [SerializeField] private string _inActivePrefix;
    private const string _rainbow_pre = "<rainb>";
    private const string _rainbow_suf = "</rainb>";

    private static HashSet<ActiveText> _listOfActiveTexts = new HashSet<ActiveText>();

    private string _activeText;
    private string _inActiveText;

    private bool _isActive;
    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }

    private void Awake()
    {
        if (_addToStaticList)
        {
            _listOfActiveTexts.Add(this);
        }
        
        if (_startsActive)
            IsActive = true;
        UpdateText();
    }

    [ContextMenu("UpdateText")]
    private void UpdateText()
    {
        BuildTextFields(_activePrefix, _inActivePrefix);
    }

    private void BuildTextFields(string activePrefix, string inactivePrefix)
    {
        var sb = new StringBuilder();

        if(activePrefix.Trim().Length != 0)
        {
            sb.Append(activePrefix);
            sb.Append(_text);
            sb.Append("</");
            sb.Append(activePrefix.Substring(1));
            _activeText = sb.ToString();
            sb.Clear();
        }
        else
        {
            _activeText = _text;
        }
        
        if(inactivePrefix.Trim().Length != 0)
        {
            sb.Append(inactivePrefix);
            sb.Append(_text);
            sb.Append("</");
            sb.Append(inactivePrefix.Substring(1));
            _inActiveText = sb.ToString();
            sb.Clear();
        }
        else
        {
            _inActiveText = _text + ' ';
        }
        
        UpdateUIText();
    }

    [ContextMenu("RainbowStyle")]
    public void RainbowStyle()
    {
        _activeText = _rainbow_pre + _activeText + _rainbow_suf;
        _inActiveText = _rainbow_pre + _inActiveText + _rainbow_suf;
        UpdateUIText();
    }

    public void UpdateUIText()
    {
        _textUI.text = IsActive ? _activeText : _inActiveText;
    }

    [ContextMenu("RainbowStyle All")]
    public void AddRainbowToAll()
    {
        foreach (var activeText in _listOfActiveTexts)
        {
            activeText.UpdateText();
            activeText.RainbowStyle();
        }
    }
    
    [ContextMenu("UpdateText All")]
    public void UpdateAllUiText()
    {
        foreach (var activeText in _listOfActiveTexts)
        {
            activeText.UpdateText();
            activeText.UpdateUIText();
        }
    }
}

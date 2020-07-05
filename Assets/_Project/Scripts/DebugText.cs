using TMPro;
using UnityEngine;
public class DebugText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    public static DebugText Instance { get; private set; } 

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
        text.text = "[Not Set]";
    }

    public void SetText(string message)
    {
        if (text != null)
        {
            text.text = message ?? "[Message was null]";
        }
    }
}

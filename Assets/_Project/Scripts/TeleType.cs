using System.Collections;
using TMPro;
using UnityEngine;

public class TeleType : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TMPUiGui;

    public IEnumerator Start()
    {
        
        _TMPUiGui = gameObject.GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();

        _TMPUiGui.ForceMeshUpdate();
        int totalVisibleCharacters = _TMPUiGui.textInfo.characterCount;
        int counter = 10;
        

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            _TMPUiGui.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
                Debug.Log("Waiting for 1.0 Seconds");
            }

            counter += 1;
            yield return new WaitForSeconds(0.05f);
            Debug.Log("Waiting for .05 seconds");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Ink.Runtime;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextAsset[] _inkJSONs;
    
    [Header("Audio")]
    [SerializeField] private SimpleAudioEvent typingSound;
    private AudioSource _audioSource;
    public bool IsVisible { get; private set; }

    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;
    
    private TextMeshProUGUI _dialogueText;
    private TextMeshProUGUI _continueButton;
    private Coroutine _teletypeCoroutine;
    private Story _story;
    private int _storyIndex;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);

            IsVisible = false;
            _dialogueText = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();
            _continueButton = dialoguePanel.GetComponentsInChildren<TextMeshProUGUI>()[1];
            _story = new Story(_inkJSONs[_storyIndex].text);
            dialoguePanel.SetActive(false);
            _audioSource = AudioManager.Instance.RequestOneShotAudioSource();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Mediator.Instance.Subscribe<NextDialogueChunk>(HandleNextDialogueChunk);
        Mediator.Instance.Subscribe<ShowDialogueCommand>(HandleShowDialogue);
        Mediator.Instance.Subscribe<HideDialogueCommand>(HandleHideDialogue);
    }

    private void HandleHideDialogue(HideDialogueCommand command)
    {
        StartCoroutine(DelayedIsVisible(.6f));
    }

    private void HandleShowDialogue(ShowDialogueCommand command)
    {
        IsVisible = true;
    }

    public void ShowNextStoryElement()
    {
        if (_teletypeCoroutine != null)
        {
            StopCoroutine(_teletypeCoroutine);
            _dialogueText.maxVisibleCharacters = 99999;
            _teletypeCoroutine = null;
            return;
        }

        _dialogueText.text = LoadStoryChunk();
        if (_dialogueText.text == "")
        {
            if (_story.currentChoices.Count > 0)
            {
                _story.ChooseChoiceIndex(0);
                _dialogueText.text = LoadStoryChunk();
                _teletypeCoroutine = StartCoroutine(TeleType(_dialogueText, 0.03f));
            }
            else
            {
                Mediator.Instance.Publish(new HideDialogueCommand());
                if (_inkJSONs.Length > _storyIndex)
                {
                    _storyIndex++;
                    if (_storyIndex < _inkJSONs.Length)
                    {
                        _story = new Story(_inkJSONs[_storyIndex].text);
                    }
                }
                Debug.Log("Hiding the Dialogue");
            }
        }
        else
        {
            _teletypeCoroutine = StartCoroutine(TeleType(_dialogueText, 0.03f));
        }
        
        _continueButton.text = GetOption();
    }

    public string LoadStoryChunk()
    {
        var text = "";
        if (_story.canContinue)
        {
            text = _story.Continue();
        }

        return text;
    }

    private void HandleNextDialogueChunk(NextDialogueChunk command)
    {
        ShowNextStoryElement();
    }

    private string GetOption()
    {
        if (_story.currentChoices.Count > 0)
        {
            return _story.currentChoices[0].text;
        }

        return "Close";
    }

    private IEnumerator TeleType(TextMeshProUGUI text, float typeSpeed)
    {
        bool stillTyping = true;
        _dialogueText.ForceMeshUpdate();
        int totalVisibleCharacters = text.textInfo.characterCount;
        int counter = 0;
        

        while (stillTyping)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);

            text.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                stillTyping = false;
                _teletypeCoroutine = null;
                Debug.Log("Finished Typing");
            }

            counter += 1;
            if( counter % 2 == 0)
                typingSound.PlayOneShot(_audioSource);
            yield return new WaitForSeconds(typeSpeed);
            Debug.Log("Waiting for .05 seconds");
        }
    }

    private IEnumerator DelayedIsVisible(float time)
    {
        yield return new WaitForSeconds(time);
        IsVisible = false;
    }
}
using System;
using DG.Tweening;
using UnityEngine;

public class DialogueHandler : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePannel;
    [SerializeField] private MenuManager _menuManager;

    public bool IsShown { get; private set; }    

    private RectTransform pannelRectTransform;
    private float _timer;
    private bool timerRunning = false;

    private void Awake()
    {
        pannelRectTransform = dialoguePannel.GetComponent<RectTransform>();
        pannelRectTransform.localScale = new Vector3(.02f , .02f, .02f );
    }

    public void Start()
    {
        Mediator.Instance.Subscribe<ShowDialogueCommand>(OnShowDialgueCommand);
        Mediator.Instance.Subscribe<HideDialogueCommand>(OnHideDialgueCommand);
    }

    public void Update()
    {
        if (!timerRunning)
            return;

        if (Time.time > _timer)
        {
            dialoguePannel.SetActive(false);
            timerRunning = false;
            IsShown = false;
        }

    }

    private void OnShowDialgueCommand(ShowDialogueCommand command)
    {
        _menuManager.HasFocus = true;
        dialoguePannel.SetActive(command.ShowDialogue);
        IsShown = true;
        pannelRectTransform.DOScale(new Vector3(1f, 1f, 1f), .5f).SetEase(Ease.OutElastic);
    }
    
    private void OnHideDialgueCommand(HideDialogueCommand command)
    {
        _menuManager.HasFocus = false;
        pannelRectTransform.DOScale(new Vector3(.02f , .02f, .02f ), .5f).SetEase(Ease.InOutQuart);
        _timer = Time.time + .5f;
        timerRunning = true;
    }
}
using UnityEngine;

public class ExplosionTransitionActivator : MonoBehaviour
{
    private GameObject[] _go;
    private Renderer[] _renderers;
    private void Awake()
    {
        _go = new GameObject[2];
        _renderers = GetComponentsInChildren<Renderer>();
        _go[0] = _renderers[0].gameObject;
        _go[1] = _renderers[1].gameObject;
        //DeActivateTransition(new ExplosionTransionFinishedCommand());
    }

    void Start()
    {
        Mediator.Instance.Subscribe<ExplosionTransionStartCommand>(ActivateTransition);
        Mediator.Instance.Subscribe<ExplosionTransionFinishedCommand>(DeActivateTransition);
    }

    public void ActivateTransition(ExplosionTransionStartCommand esc)
    {
        //_go[0].SetActive(true);
       // _go[1].SetActive(true);
       _renderers[0].enabled = true;
       _renderers[1].enabled = true;
    }

    public void DeActivateTransition(ExplosionTransionFinishedCommand efc)
    {
        //_go[0].SetActive(false);
        //_go[0].SetActive(true);
        _renderers[0].enabled = false;
        _renderers[1].enabled = false;
    }
}

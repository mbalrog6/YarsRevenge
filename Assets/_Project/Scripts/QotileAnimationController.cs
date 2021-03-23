using UnityEngine;

public class QotileAnimationController : MonoBehaviour
{
    [SerializeField] private Animator qotileAnimator;

    public static QotileAnimationController Instance => _instance;
    private static readonly int _IsClosed = Animator.StringToHash("IsClosed");
    private static QotileAnimationController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void TriggerOpenGate()
    {
        if (qotileAnimator != null)
        {
            qotileAnimator.SetBool(_IsClosed, false);
        }
    }

    public void TriggerCloseGate()
    {
        if (qotileAnimator != null)
        {
            qotileAnimator.SetBool(_IsClosed, true);
        }
    }
}

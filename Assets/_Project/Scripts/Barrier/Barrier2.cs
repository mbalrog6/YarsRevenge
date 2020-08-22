using UnityEngine;

public class Barrier2 : MonoBehaviour
{
    public BarrierComponent BarrierComponent => _barrierComponent;
    
    public RectContainer BarrierRectContainer => _barrierRectContainer;
    public int Score(int index) => _barrierComponent.Barrier[index].Score;
    public bool IsCellActive(int index) => _barrierComponent.Barrier[index].isActiveAndEnabled;
    public Vector3 WarlordSpawnPoint => _warlordSpawnPoint;
   
    private BarrierComponent _barrierComponent;
    private OsalateMover _mover;
    private BarrierCellShifter _shifter;
    private RectContainer _barrierRectContainer;
    private Vector3 _centerPointOffsetForRectContainer;
    private Vector3 _warlordSpawnPoint;

    private float _timer;
    private float _pauseDelay;

    private void Awake()
    {
        _barrierComponent = GetComponent<BarrierComponent>();
        _mover = new OsalateMover(this.gameObject, 5, -5, 5f);
        _shifter = new BarrierCellShifter(ref _barrierComponent.Barrier, _barrierComponent.Width, _barrierComponent.Height);
        CalculateBarriorBounds();
    }
    
    private void Update()
    {
        if (GameStateMachine.Instance.CurrentState == States.PAUSE || 
        GameStateMachine.Instance.CurrentState == States.BRIEF_PAUSE)
        {
            _pauseDelay += Time.deltaTime;
            return;
        }

        if (_pauseDelay > 0)
        {
            _timer += _pauseDelay;
            _pauseDelay = 0f;
        }
        
        _mover.Tick();
        var rectContainerPosition = transform.position + _centerPointOffsetForRectContainer;
        _barrierRectContainer.UpdatePosition(rectContainerPosition);
        SetWarlordSpawnPoint();
        _shifter.Pattern = _barrierComponent.BarrierInfo.ShiftPattern;

        if (Time.time > _timer)
        {
            _shifter.Tick();
            _timer = Time.time + _barrierComponent.BarrierInfo.BarrierShiftPulseTime;
        }
    }
    
    private void CalculateBarriorBounds()
    {
        var width = _barrierComponent.Width * _barrierComponent.CellWidth;
        var height = _barrierComponent.Height * _barrierComponent.CellHeight;
        var left = transform.position.x - (_barrierComponent.CellWidth / 2);
        var bottom = transform.position.y - (_barrierComponent.CellHeight / 2);

        if (BarrierRectContainer == null)
        {
            _barrierRectContainer = new RectContainer(gameObject, left, bottom, width, height);
            var x = (width / 2) - (_barrierComponent.CellWidth / 2);
            var y = (height / 2) - (_barrierComponent.CellHeight / 2);
            _centerPointOffsetForRectContainer = new Vector3(x, y, 0f);
        }
    }

    private void SetWarlordSpawnPoint()
    {
        var spawnPoint = _barrierRectContainer.Bounds.center;
        spawnPoint.x += _barrierRectContainer.Bounds.width / 2 - .5f;
        _warlordSpawnPoint = spawnPoint;
    }

    private void SetWarlordSpawnPoint(Vector3 point)
    {
        _warlordSpawnPoint = point;
    }
    
    public int? GetCellFromVector3(Vector3 screenPoint)
    {
        if (!_barrierRectContainer.Bounds.Contains(screenPoint)) return null;

        var point = (Vector2) screenPoint -
                    new Vector2(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMin);
        int x = (int) Mathf.Floor(point.x / _barrierComponent.CellWidth);
        int y = (int) Mathf.Floor(point.y / _barrierComponent.CellHeight);

        int? index = x + (y * _barrierComponent.Width);
        if (!_barrierComponent.Barrier[index.Value].isActiveAndEnabled)
        {
            index = null;
        }

        return index;
    }
    
    public void SetCellColor(int index, Color color)
    {
        _barrierComponent.Barrier[index].GetComponent<MeshRenderer>().material.color = color;
    }

    public void DisableCell(int index)
    {
        _barrierComponent.Barrier[index].gameObject.SetActive(false);
    }

    public void EnableCell(int index)
    {
        _barrierComponent.Barrier[index].gameObject.SetActive(true);
    }

    public void SetCellActive(int index, bool flag)
    {
        if (index > _barrierComponent.Barrier.Length - 1 || index < 0)
            return; 
        
        _barrierComponent.Barrier[index].gameObject.SetActive(flag);
    }

    public void SetHighAndLowBoundsLimitForOscalator(float low, float high)
    {
        _mover.LowLimit = low;
        _mover.HighLimit = high;
    }
    
    public void DisableCellsInPlusPattern(int index)
    {
        var totalCells = BarrierComponent.Barrier.Length;
        var above = index + BarrierComponent.Width;
        var right = index + 1;
        var left = index - 1;
        var bellow = index - BarrierComponent.Width;

        if (index != 0)
        {
            if (index % (BarrierComponent.Width) == 0)
            {
                left = -1;
            }

            if (index % (BarrierComponent.Width) == BarrierComponent.Width - 1)
            {
                right = -1;
            }
        }

        DisableCell(index);

        if (above >= 0 && above < totalCells)
        {
            DisableCell(above);
        }

        if (bellow >= 0 && bellow < totalCells)
        {
            DisableCell(bellow);
        }

        if (right >= 0 && right < totalCells)
        {
            DisableCell(right);
        }

        if (left >= 0 && left < totalCells)
        {
            DisableCell(left);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_barrierRectContainer == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMin, 0f),
            new Vector3(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_barrierRectContainer.Bounds.xMax, _barrierRectContainer.Bounds.yMin, 0f),
            new Vector3(_barrierRectContainer.Bounds.xMax, _barrierRectContainer.Bounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMin, 0f),
            new Vector3(_barrierRectContainer.Bounds.xMax, _barrierRectContainer.Bounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMax, 0f),
            new Vector3(_barrierRectContainer.Bounds.xMax, _barrierRectContainer.Bounds.yMax, 0f));
        
        Gizmos.DrawCube(_warlordSpawnPoint, Vector3.one * .5f );
    }
}

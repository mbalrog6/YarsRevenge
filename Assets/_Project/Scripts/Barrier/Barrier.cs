﻿using UnityEngine;

public class Barrier : MonoBehaviour, IBarrier
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float rotationPulseTime = .5f;

    public int Score(int index) => _cells[index].Score;
    public bool IsReflective => true; 
    public float WidthOfCell { get; private set; }
    public float HeightOfCell { get; private set; }
    public bool IsCellActive(int index) => _cells[index].isActiveAndEnabled;
    public BarrierCell[] GetCellArray => _cells;

    private BarrierCell[] _cells;
    private RectContainer _barrierRectContainer;
    public RectContainer BarrierRectContainer => _barrierRectContainer;
    private Vector3 _rectOffset;

    private float _timer; 
    private Transform _warlordSpawnPoint;
    private OsalateMover _mover;
    private BarrierCellShifter _shifter;

    private void Awake()
    {
        var numberOfCells = CalculateDimensionsBasedOnCellPrefab();
        _cells = new BarrierCell[numberOfCells];
        FillBarrierWithCells(numberOfCells);
        CalculateBarriorBounds();
        CalculateWarlordSpawnOffset();
        _timer = Time.time + rotationPulseTime;
        _mover = new OsalateMover(this.gameObject,5f,  -5f, 3f );
        _shifter = new BarrierCellShifter(this, ref _cells, width, height);
        _shifter.Pattern = BarrierShiftPatterns.SnakeUp;
    }

    private void Update()
    {
        _mover.Tick();
        var position = transform.position + _rectOffset;
        _barrierRectContainer.UpdatePosition(position);

        if (Time.time > _timer)
        {
            _shifter.Tick();
            _timer = Time.time + rotationPulseTime;
        }
    }

    private void CalculateWarlordSpawnOffset()
    {
        _warlordSpawnPoint = gameObject.transform.GetChild(0);
        Vector3 spawnLocalPosition = new Vector3();
        spawnLocalPosition.x = (WidthOfCell * width) / 2f;
        spawnLocalPosition.y = (HeightOfCell * height) / 2f;
        _warlordSpawnPoint.localPosition = spawnLocalPosition;
    }

    private void FillBarrierWithCells(int numberOfCells)
    {
        for (int i = 0; i < numberOfCells; i++)
        {
            Vector3 position = DetermineCellPosition(i);
            var cell = GameObject.Instantiate(cellPrefab, transform).GetComponent<BarrierCell>();
            cell.gameObject.name = $"Cell {i.ToString()}";
            cell.transform.localPosition = position;
            _cells[i] = cell;
        }
    }

    private int CalculateDimensionsBasedOnCellPrefab()
    {
        WidthOfCell = cellPrefab.transform.localScale.x;
        HeightOfCell = cellPrefab.transform.localScale.y;

        if (width < 1)
            width = 1;
        if (height < 1)
            height = 1;

        var numberOfCells = width * height;
        return numberOfCells;
    }

    public void SetCellColor(int index, Color color)
    {
        _cells[index].GetComponent<MeshRenderer>().material.color = color;
    }

    public void DisableCell(int index)
    {
        _cells[index].gameObject.SetActive(false);
    }

    public void EnableCell(int index)
    {
        _cells[index].gameObject.SetActive(true);
    }

    public void SetCellActive(int index, bool flag)
    {
        if (index > _cells.Length - 1 || index < 0)
            return; 
        
        _cells[index].gameObject.SetActive(flag);
    }

    public void DisableCellsInPlusPattern(int index)
    {
        var totalCells = width * height;
        var above = index + width;
        var right = index + 1;
        var left = index - 1;
        var bellow = index - width;

        if (index != 0)
        {
            if (index % (width) == 0)
            {
                left = -1;
            }

            if (index % (width) == width - 1)
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

    private void CalculateBarriorBounds()
    {
        var width = this.width * WidthOfCell;
        var height = this.height * HeightOfCell;
        var left = transform.position.x - (WidthOfCell / 2);
        var bottom = transform.position.y - (HeightOfCell / 2);

        if (BarrierRectContainer == null)
        {
            _barrierRectContainer = new RectContainer(gameObject, left, bottom, width, height);
            var x = ((this.width * WidthOfCell) / 2) - (WidthOfCell / 2f);
            var y = ((this.height * HeightOfCell) / 2) - (HeightOfCell / 2f);
            _rectOffset = new Vector3(x, y, 0f);
        }
    }

    private Vector3 DetermineCellPosition(int i)
    {
        return new Vector3((i % width) * WidthOfCell, Mathf.Floor(i / width) * WidthOfCell, 0f);
    }

    public int? GetCellFromVector3(Vector3 screenPoint)
    {
        if (!_barrierRectContainer.Bounds.Contains(screenPoint)) return null;

        var point = (Vector2) screenPoint -
                    new Vector2(_barrierRectContainer.Bounds.xMin, _barrierRectContainer.Bounds.yMin);
        int x = (int) Mathf.Floor(point.x / WidthOfCell);
        int y = (int) Mathf.Floor(point.y / HeightOfCell);

        int? index = x + (y * width);
        if (!_cells[index.Value].isActiveAndEnabled)
        {
            index = null;
        }

        return index;
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
    }
}
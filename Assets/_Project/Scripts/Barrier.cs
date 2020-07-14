﻿using System;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int width;
    [SerializeField] private int height; 
    
    public float WidthOfCell { get; private set; }        
    public float HeightOfCell { get; private set; }

    private BarrierCell[] _cells;
    public Rect BarriorBounds { get; private set; }

    private void Awake()
    {
        WidthOfCell = cellPrefab.transform.localScale.x;
        HeightOfCell = cellPrefab.transform.localScale.y;
        
        if (width < 1)
            width = 1;
        if (height < 1)
            height = 1; 
        
        var numberOfCells = width * height;
        
        _cells = new BarrierCell[numberOfCells];
        
        for (int i = 0; i < numberOfCells; i++)
        {
            Vector3 position = DetermineCellPosition(i);
            var cell = GameObject.Instantiate(cellPrefab, transform).GetComponent<BarrierCell>();
            cell.gameObject.name = $"Cell {i.ToString()}";
            cell.transform.localPosition = position;
            _cells[i] = cell;
        }

        CalculateBarriorBounds();
    }

    private void Update()
    {
        CalculateBarriorBounds();
        var text = GetCellFromVector3(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (text.HasValue)
        {
            SetCellColor(text.Value, Color.yellow);
        }
    }

    public void SetCellColor(int index, Color color)
    {
        _cells[index].GetComponent<MeshRenderer>().material.color = color;
    }

    public void DisableCell(int index)
    {
        _cells[index].gameObject.SetActive(false);
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

        BarriorBounds = new Rect(left, bottom, width, height);
    }

    private Vector3 DetermineCellPosition(int i)
    {
        return new Vector3((i % width) * WidthOfCell, Mathf.Floor(i / width) * WidthOfCell, 0f );
    }

    public int? GetCellFromVector3(Vector3 screenPoint)
    {
        if (!BarriorBounds.Contains(screenPoint)) return null;

        var point = (Vector2)screenPoint - new Vector2(BarriorBounds.xMin, BarriorBounds.yMin);
        int x = (int)Mathf.Floor(point.x / WidthOfCell);
        int y = (int)Mathf.Floor(point.y / HeightOfCell);

        int? index = x + (y * width);
        if (!_cells[index.Value].isActiveAndEnabled)
        {
            index = null; 
        }
        return index;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3( BarriorBounds.xMin, BarriorBounds.yMin, 0f), new Vector3( BarriorBounds.xMin, BarriorBounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( BarriorBounds.xMax, BarriorBounds.yMin, 0f), new Vector3( BarriorBounds.xMax, BarriorBounds.yMax, 0f));
        Gizmos.DrawLine(new Vector3( BarriorBounds.xMin, BarriorBounds.yMin, 0f), new Vector3( BarriorBounds.xMax, BarriorBounds.yMin, 0f));
        Gizmos.DrawLine(new Vector3( BarriorBounds.xMin, BarriorBounds.yMax, 0f), new Vector3( BarriorBounds.xMax, BarriorBounds.yMax, 0f));
    }
}
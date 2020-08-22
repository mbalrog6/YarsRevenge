using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierFactory
{
    public float WidthOfCell { get; private set; }
    public float HeightOfCell { get; private set; }
    
    private GameObject _cellPrefab;
    private List<GameObject> _barriers;
    private BarrierDeffintionCreator _barrierDeffintionCreator;

    public BarrierFactory( GameObject cellPrefab )
    {
        _barriers = new List<GameObject>();
        _barrierDeffintionCreator = new BarrierDeffintionCreator();
        _cellPrefab = cellPrefab;
        WidthOfCell = cellPrefab.transform.localScale.x;
        HeightOfCell = cellPrefab.transform.localScale.y;
    }

    public void LoadBarriersInMemory()
    {
        GameObject obj; 
        for (int i = 0; i < _barrierDeffintionCreator.Count; i++)
        {
            obj = new GameObject();    
            obj.name = $"Barrier - {i}";
            var barrierComponent = obj.AddComponent<BarrierComponent>();
            

            barrierComponent.Height = _barrierDeffintionCreator[i].Height;
            barrierComponent.Width = _barrierDeffintionCreator[i].Width;
            barrierComponent.Barrier = new BarrierCell[barrierComponent.Height * barrierComponent.Width];

            FillBarrierWithCells(_barrierDeffintionCreator[i].NumberOfCells, ref obj, ref barrierComponent);
            _barrierDeffintionCreator.CreateBarrier(ref barrierComponent.Barrier, i);
            _barriers.Add(obj);
            obj.SetActive(false);
        }
    }
    
    private void FillBarrierWithCells(int numberOfCells, ref GameObject obj, ref BarrierComponent barrierComponent)
    {
       
        for (int i = 0; i < numberOfCells; i++)
        {
            Vector3 position = new Vector3((i % barrierComponent.Width) * WidthOfCell,
                Mathf.Floor(i / barrierComponent.Width) * HeightOfCell,
                0f);
            var cell = GameObject.Instantiate(_cellPrefab, obj.transform).GetComponent<BarrierCell>();
            cell.gameObject.name = $"Cell - {i.ToString()}";
            cell.transform.localPosition = position;
            barrierComponent.Barrier[i] = cell;
        }

        barrierComponent.CellHeight = _cellPrefab.transform.localScale.y;
        barrierComponent.CellWidth = _cellPrefab.transform.localScale.x;
    }

    public GameObject GetBarrier(BarrierInfo barrierInfo)
    {
        var obj = GameObject.Instantiate(_barriers[barrierInfo.BarrierIndex]);
        obj.SetActive(true);
        obj.GetComponent<BarrierComponent>().SetBarrierInfo(barrierInfo);
        obj.AddComponent<Barrier2>();
        return obj;
    }

}

public class LoadedBarriers
{
    public BarrierCell[] Barrier;
    public int Height;
    public int Width;
}

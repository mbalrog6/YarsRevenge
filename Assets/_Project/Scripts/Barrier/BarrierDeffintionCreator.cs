using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BarrierDeffintionCreator
{
    public int Count => _patterns.Count;
   private List<BarrierPatternDefinition> _patterns;
    public BarrierDeffintionCreator()
    {
        _patterns = new List<BarrierPatternDefinition>();
       LoadBarrierData();
    }

    public BarrierPatternDefinition this[int index]
    {
        get => _patterns[index];
    }

    public void CreateBarrier(ref BarrierCell[] barrierArray, int index)
    {
        int cellType;

        foreach (var cell in barrierArray)
        {
            cellType = _patterns[index].Pattern.Pop();
            if (cellType == 0)
            {
                cell.gameObject.SetActive(false);
            }
        }
    }

    private void LoadBarrierData()
    {
        var files = Directory.GetFiles(Application.streamingAssetsPath, "*.txt");

        foreach (var file in files)
        {
            _patterns.Add(GetPatternDefinition(file));
        }
        
    }

    private BarrierPatternDefinition GetPatternDefinition(string path)
    {
        BarrierPatternDefinition barrierPatternDefinition = new BarrierPatternDefinition();
        var stack = new Stack<int>();
        var contents = File.ReadAllLines(path);
        barrierPatternDefinition.Height = contents.Length;
        barrierPatternDefinition.Width = contents[0].Length;

        foreach (var line in contents)
        {
            foreach (var character in line)
            {
                if (character == '1')
                {
                    stack.Push(1);
                }
                else
                {
                    stack.Push(0);
                }
            }
        }

        barrierPatternDefinition.Pattern = stack;
        barrierPatternDefinition.NumberOfCells = barrierPatternDefinition.Height * barrierPatternDefinition.Width;

        return barrierPatternDefinition;
    }
}
public enum BarrierShiftPatterns
{
    None,
    SnakeDown, 
    SnakeUp, 
    BubbleForward,
}

public class BarrierCellShifter : IShifter
{
    public bool IsCellActive(int index) => _cells[index].isActiveAndEnabled;
    public BarrierShiftPatterns Pattern { get; set; } = BarrierShiftPatterns.None;

    private Barrier _barrier;
    private BarrierCell[] _cells;

    private int width;
    private int height;
    private float cellWidth;
    private float cellHeight;

    private int index;
    private bool currentValue;
    private bool beginValue;
    private bool lastValue;
    private bool nextValue;
    
    public BarrierCellShifter(Barrier barrier, ref BarrierCell[] cellArray, int width, int height)
    {
        _barrier = barrier;
        _cells = cellArray;
        this.width = width;
        this.height = height;
        cellWidth = _cells[0].Width;
        cellHeight = _cells[1].Height;
    }

    public void Tick()
    {
        switch (Pattern)
        {
            case BarrierShiftPatterns.None:
                break;
            case BarrierShiftPatterns.BubbleForward:
                BubbleForward();
                break;
            case BarrierShiftPatterns.SnakeDown:
                SnakePatternTopToDown();
                break;
            case BarrierShiftPatterns.SnakeUp:
                SnakePatternBottomToTop();
                break;
        }
    }
    
    public void SetCellActive(int index, bool flag)
    {
        if (index > _cells.Length - 1 || index < 0)
            return; 
        
        _cells[index].gameObject.SetActive(flag);
    }

    private void SnakePatternBottomToTop()
    {
        ResetStartValues();

        for (int y = 0; y < height; y++)
        {
            if (y == 0)
            {
                index = GetWrapAroundIndexForBarrier();
                currentValue = IsCellActive(index);
                index = 0;
            }
            else
            {
                index = y * width;
                currentValue = IsCellActive(index);
            }

            if (y % 2 == 0)
            {
                lastValue = ShiftCellToRight(index, beginValue, currentValue, lastValue);
            }
            else
            {
                beginValue = ShiftCellToLeft(index, lastValue, beginValue);
            }
        }
    }

    private void ResetStartValues()
    {
        index = 0;
        currentValue = false;
        lastValue = false;
        beginValue = false;
        nextValue = false;
    }

    private int GetWrapAroundIndexForBarrier()
    {
        if (height % 2 == 0)
        {
            index = _cells.Length - width;
        }
        else
        {
            index = _cells.Length - 1;
        }

        return index;
    }

    private bool ShiftCellToLeft(int index, bool lastValue, bool beginValue)
    {
        for (int x = 0; x < width; x++)
        {
            if (x == width - 1)
            {
                SetCellActive(index, lastValue);
            }
            else
            {
                if (x == 0)
                {
                    beginValue = IsCellActive(index);
                }

                SetCellActive(index, IsCellActive(index + 1));
            }

            index++;
        }

        return beginValue;
    }

    private bool ShiftCellToRight(int index, bool beginValue, bool currentValue, bool lastValue)
    {
        for (int x = 0; x < width; x++)
        {
            nextValue = IsCellActive(index);
            if (x == 0 && index != 0)
            {
                SetCellActive(index, beginValue);
            }
            else
            {
                SetCellActive(index, currentValue);
            }

            index++;

            currentValue = nextValue;
            lastValue = currentValue;
        }

        return lastValue;
    }

    private void SnakePatternTopToDown()
    {
        ResetStartValues();

        for (int y = height - 1; y >= 0; y--)
        {
            if (y == height - 1)
            {
                index = width - 1;
                beginValue = IsCellActive(index);
                lastValue = beginValue;
                index = y * width;
            }
            else
            {
                index = y * width;
                currentValue = IsCellActive(index);
            }


            if (y % 2 == 0)
            {
                lastValue = ShiftCellToRightDown(index, beginValue, currentValue, lastValue);
            }
            else
            {
                beginValue = ShiftCellToLeft(index, lastValue, beginValue);
            }
        }
    }

    private bool ShiftCellToRightDown(int index, bool beginValue, bool currentValue, bool lastValue)
    {
        for (int x = 0; x < width; x++)
        {
            nextValue = IsCellActive(index);
            if (x == 0 && index != width - 1)
            {
                SetCellActive(index, beginValue);
            }
            else
            {
                SetCellActive(index, currentValue);
            }

            index++;

            currentValue = nextValue;
            lastValue = currentValue;
        }

        return lastValue;
    }

    private void BubbleForward()
    {
        index = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = width - 1; x > 0; x--)
            {
                index = y * width + x;
                if (IsCellActive(index) == true && IsCellActive(index - 1) == false)
                {
                    SetCellActive(index - 1, true);
                    SetCellActive(index, false);
                }
            }
        }
    }
}
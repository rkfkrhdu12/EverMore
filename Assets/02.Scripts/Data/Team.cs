
public class Team
{
    public string Name { set; get; } = "";

    public const int UnitCount = 6;

    public UnitStatus[] _units;
    public int Length { get { return _units.Length; } }

    public void Init(eTeam team = eTeam.PLAYER)
    {
        if (_units == null)
            _units = new UnitStatus[UnitCount];

        for (int i = 0; i < Length; ++i)
        {
            if (_units[i] == null)
                _units[i] = new UnitStatus();

            _units[i].Init(team);
        }
    }

    public ref UnitStatus GetUnit(int index)
    {
        if(_units[index]._equipedItems == null) { _units[index].Init(); }

        return ref _units[index];
    }

    public void SetEquipedItems(int index, int[] items)
    {
        if(_units[index]._equipedItems == null) { _units[index].Init(); }

        _units[index]._equipedItems = items;
    }
    
    public void UpdateItems()
    {
        for (int i = 0; i < Length; ++i)
        {
            _units[i].UpdateItems();
        }
    }
}

class BusLogic
{
    private List<BusModel> _busses;

    static public BusModel? CurrentBus{ get; private set; }
    public List<BusModel> Busses {get => _busses;} // Readonly


    public BusLogic()
    {
        _busses = DataAccess<BusModel>.LoadAll("busses");
    }

    public void UpdateList(BusModel bus)
    {
        //Find if there is already an model with the same id
        int index = _busses.FindIndex(p => p.Id == bus.Id);

        if (index != -1)
        {
            //update existing model
            _busses[index] = bus;
        }
        else
        {
            //add new model
            _busses.Add(bus);
        }
        DataAccess<BusModel>.WriteAll(_busses, "busses");
    }

    public BusModel GetById(int id)
    {
        return _busses.Find(p => p.Id == id);
    }

    public List<BusModel> GetAllBusses()
    {
        List<BusModel> list_Busses = _busses;
        return list_Busses;
    }

}



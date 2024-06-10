using System.Data;

public class BusLogic : AbstractLogic<BusModel>
{
    private static RouteLogic routeLogic = new();
    private List<BusModel> _busses;
    

    public static BusModel? CurrentBus{ get; private set; }


    public BusLogic()
    {
        _busses = DataAccess<BusModel>.LoadAll();
    }

    public override void UpdateList(BusModel bus)
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
        DataAccess<BusModel>.WriteAll(_busses);
    }

    public override BusModel GetById(int id)
    {
        return _busses.Find(p => p.Id == id);
    }

    public override int GenerateNewId() 
    {
        if (_busses == null || _busses.Count == 0)
        {
            return 1;
        }
       return _busses.Max(bus => bus.Id) + 1;
    } 
    public override List<BusModel> GetAll() => _busses = DataAccess<BusModel>.LoadAll();

    public void UpdateBusRoutes()
    {
        for(int i = 0; i < _busses.Count(); i++)
        {
            for(int j = 0; j < _busses[i].Route.Count(); j++)
            {
                List<RouteModel> _routes = routeLogic.GetAll();
                if (_busses[i].Route.Count() != 0)
                {
                    RouteModel Route = _routes.Where(r => r.Id == _busses[i].Route[j].Id).FirstOrDefault();
                    _busses[i].Route[j] = Route;
                }
                UpdateList(_busses[i]);
            }
        }
    }
}




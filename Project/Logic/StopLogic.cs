public class StopLogic : AbstractLogic<StopModel>
{
    private List<StopModel> _stops;
    public static RouteModel? CurrentRoute { get; private set; }

    public StopLogic()
    {
        _stops = DataAccess<StopModel>.LoadAll();
    }

    public override void UpdateList(StopModel stop)
    {
        //Find if there is already an model with the same id
        int index = _stops.FindIndex(s => s.Id == stop.Id);

        if (index != -1)
        {
            //update existing model
            _stops[index] = stop;
        }
        else
        {
            //add new model
            _stops.Add(stop);
        }
        DataAccess<StopModel>.WriteAll(_stops);
    }

    public override StopModel GetById(int id)
    {
        return _stops.Find(i => i.Id == id);
    }

    public override int GenerateNewId() 
    {
        if (_stops == null || _stops.Count == 0)
        {
            return 1;
        }
       return _stops.Max(stop => stop.Id) + 1;
    }

    public override List<StopModel> GetAll() => _stops;
}
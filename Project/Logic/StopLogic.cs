using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class StopLogic
{
    private List<StopModel> _stops;
    static public StopModel? CurrentRoute { get; private set; }

    public StopLogic()
    {
        _stops = StopAccess.LoadAll();
    }

    public void UpdateList(StopModel stop)
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
        StopAccess.WriteAll(_stops);
    }

    public StopModel GetById(int id)
    {
        return _stops.Find(i => i.Id == id);
    }

    public List<StopModel> GetAllStops()
    {
        List<StopModel> listStops = _stops;
        return listStops;
    }
}
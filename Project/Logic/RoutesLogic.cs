using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class RoutesLogic
{
    private List<RoutesModel> _routes;
    static public RoutesModel? CurrentRoute { get; private set; }

    public RoutesLogic()
    {
        _routes = RoutesAccess.LoadAll();
    }

    public void UpdateList(RoutesModel route)
    {
        //Find if there is already an model with the same id
        int index = _routes.FindIndex(s => s.Id == route.Id);

        if (index != -1)
        {
            //update existing model
            _routes[index] = route;
        }
        else
        {
            //add new model
            _routes.Add(route);
        }
        RoutesAccess.WriteAll(_routes);
    }

    public RoutesModel GetById(int id)
    {
        return _routes.Find(i => i.Id == id);
    }

    public List<RoutesModel> GetAllRoutes()
    {
        List<RoutesModel> list_Routes = _routes;
        return list_Routes;
    }
}
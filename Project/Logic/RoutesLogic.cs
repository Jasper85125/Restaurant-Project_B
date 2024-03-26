using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class RoutesLogic
{
    private List<RouteModel> _routes;
    static public RouteModel? CurrentRoute { get; private set; }

    public RoutesLogic()
    {
        _routes = RoutesAccess.LoadAll();
    }

    public void UpdateList(RouteModel route)
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

    public RouteModel GetById(int id)
    {
        return _routes.Find(i => i.Id == id);
    }

    public List<RouteModel> GetAllRoutes()
    {
        List<RouteModel> list_Routes = _routes;
        return list_Routes;
    }
}
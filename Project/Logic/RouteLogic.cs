using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class RouteLogic : AbstractLogic<RouteModel>
{
    private static List<RouteModel> _routes;
    public static RouteModel? CurrentRoute { get; private set; }

    public RouteLogic()
    {
        _routes = DataAccess<RouteModel>.LoadAll("routes");
    }

    public override void UpdateList(RouteModel route)
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
        DataAccess<RouteModel>.WriteAll(_routes, "routes");
    }

    public override RouteModel GetById(int id)
    {
        return _routes.Find(i => i.Id == id);
    }

    public override int GenerateNewId() 
    {
        if (_routes == null || _routes.Count == 0)
        {
            return 1;
        }
       return _routes.Max(price => price.Id) + 1;
    }

    public override List<RouteModel> GetAll() => _routes;

}
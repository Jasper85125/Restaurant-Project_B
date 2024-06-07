using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class RouteLogic : AbstractLogic<RouteModel>
{
    private List<RouteModel> _routes;
    public static RouteModel? CurrentRoute { get; private set; }
     private static BusLogic busLogic = new();

    public RouteLogic()
    {
        _routes = DataAccess<RouteModel>.LoadAll();
    }

    public override void UpdateList(RouteModel route)
    {
        _routes = DataAccess<RouteModel>.LoadAll();
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
        DataAccess<RouteModel>.WriteAll(_routes);
        RouteLogic.busLogic.UpdateBusRoutes();
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
       return _routes.Max(route => route.Id) + 1;
    }

    public override List<RouteModel> GetAll()=>  _routes = DataAccess<RouteModel>.LoadAll();

    public static RouteModel AddToRoute(StopModel stop, RouteModel inputRoute)
    {
        inputRoute.Stops.Add(stop);
        return inputRoute;
    }

    public static RouteModel RemoveFromRoute(StopModel stop, RouteModel inputRoute)
    {
        inputRoute.Stops.Remove(stop);
        return inputRoute;
    }
}

class PriceLogic
{
    private List<PriceModel> _prices;
    private List<int> IDsList = new List<int>();
    static public PriceModel? CurrentPrice{ get; private set; }
    public List<PriceModel> GetPrices {get => _prices;} // Readonly


    public PriceLogic()
    {
        _prices = PricesAccess.LoadAll();
    }

    public void UpdateList(PriceModel price)
    {
        //Find if there is already an model with the same id
        int index = _prices.FindIndex(p => p.ID == price.ID);

        if (index != -1)
        {
            //update existing model
            _prices[index] = price;
        }
        else
        {
            //add new model
            _prices.Add(price);
        }
        PricesAccess.WriteAll(_prices);
    }

    public PriceModel GetById(int id)
    {
        return _prices.Find(p => p.ID == id);
    }

    public int GenerateNewId()
    {
        foreach(PriceModel price in _prices)
        {
            IDsList.Add(price.ID);
        }
        int newId = 1;
        while (IDsList.Contains(newId))
        {
            newId++;
        }

        return newId;
    }

}



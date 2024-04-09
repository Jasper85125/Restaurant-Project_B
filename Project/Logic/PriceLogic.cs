class PriceLogic
{
    private List<PriceModel> _prices;
    private List<int> IDsList = new List<int>();
    static public PriceModel? CurrentPrice{ get; private set; }

    public PriceLogic()
    {
        _prices = PricesAccess.LoadAll();
    }

    public void UpdateList(PriceModel priceModel)
    {
        //Find if there is already an model with the same id
        int index = _prices.FindIndex(p => p.ID == priceModel.ID);

        if (index != -1)
        {
            //update existing model
            _prices[index] = priceModel;
        }
        else
        {
            //add new model
            _prices.Add(priceModel);
        }
        PricesAccess.WriteAll(_prices);
    }

    public PriceModel GetById(int id)
    {
        return _prices.Find(p => p.ID == id);
    }

    public void DeletePriceCategory(int id)
    {
        int index = _prices.FindIndex(p => p.ID == id);

        if (index != -1)
        {
            // Hier verwijder ik het price model uit de lijst
            _prices.RemoveAt(index);
            // Hier worden de IDs bijv 1,2,3 in plaats van 1,2,4
            // IDsCorrection();
            PricesAccess.WriteAll(_prices);
        }
        else
        {
            Console.WriteLine("Prijs categorie met het opgegeven ID bestaat niet.");
        }
    }
    public int GenerateNewId() 
    {
        if (_prices == null || _prices.Count == 0)
        {
            return 1;
        }
       return _prices.Max(price => price.ID) + 1;
    } 

    public List<PriceModel> GetAll() => _prices;

    // public void IDsCorrection()
    // {
    //     for (int i = 0; i < _prices.Count; i++)
    //     {
    //         _prices[i].ID = i + 1;
    //     }
    // }
}



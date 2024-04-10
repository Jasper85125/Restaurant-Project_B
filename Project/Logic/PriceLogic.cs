class PriceLogic
{
    private List<PriceModel> _prices;
    static public PriceModel? CurrentPrice{ get; private set; }

    public PriceLogic()
    {

       //  _prices = PricesAccess.LoadAll();
       _prices = DataAccess<PriceModel>.LoadAll("prices");
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
        // PricesAccess.WriteAll(_prices);
        DataAccess<PriceModel>.WriteAll(_prices, "prices");
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
            
            // PricesAccess.WriteAll(_prices);
            DataAccess<PriceModel>.WriteAll(_prices, "prices");
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

}

public class PriceLogic : AbstractLogic<PriceModel>
{
    private List<PriceModel> _prices;
    public static PriceModel? CurrentPrice{ get; private set; }

    public PriceLogic()
    {

       _prices = DataAccess<PriceModel>.LoadAll("prices");
    }

    public override void UpdateList(PriceModel priceModel)
    {
        //Find if there is already an model with the same id
        int index = _prices.FindIndex(p => p.Id == priceModel.Id);

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
        DataAccess<PriceModel>.WriteAll(_prices, "prices");
    }

    public override PriceModel GetById(int id)
    {
        return _prices.Find(p => p.Id == id);
    }

    public void DeletePriceCategory(int id)
    {
        int index = _prices.FindIndex(p => p.Id == id);

        if (index != -1)
        {
            // Hier verwijder ik het price model uit de lijst
            _prices.RemoveAt(index);
            
            DataAccess<PriceModel>.WriteAll(_prices, "prices");
        }
        else
        {
            Console.WriteLine("Prijs categorie met het opgegeven ID bestaat niet.");
        }
    }
    public override int GenerateNewId() 
    {
        if (_prices == null || _prices.Count == 0)
        {
            return 1;
        }
       return _prices.Max(price => price.Id) + 1;
    } 

    public override List<PriceModel> GetAll() => _prices;

}

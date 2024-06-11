public abstract class AbstractLogic<T>
{
    public abstract void UpdateList(T item);
    public abstract T GetById(int id);
    public abstract int GenerateNewId();
    public abstract List<T> GetAll();
}
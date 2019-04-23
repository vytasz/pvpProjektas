namespace WebApplication2.Services
{
    public interface ICacheService
    {
        bool AddItemToCache(string key, object value, int duration);

        object GetItemFromCache(string key);
    }
}
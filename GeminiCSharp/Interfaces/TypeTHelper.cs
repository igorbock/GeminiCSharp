using System.Threading.Tasks;

namespace GeminiCSharp.Interfaces
{
    public interface TypeTHelper<TypeT, ResponseType>
    {
        Task<ResponseType> GetResponseFromTypeTAsync(TypeT entity);
    }
}

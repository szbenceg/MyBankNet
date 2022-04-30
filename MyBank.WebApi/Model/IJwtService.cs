

using MyBank.Persistence.Dao;

namespace MyBank.WebApi.Model
{ 
    public interface IJwtService
    {
        string GenerateJWTToken(Employee user);
    }
}

using System.Data;

namespace Sem._5.API;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
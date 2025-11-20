using System.Data.Common;

namespace ownerMicroservice.Infrastructure.Connection;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();

    string GetProviderName();
}

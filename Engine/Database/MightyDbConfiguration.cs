using Npgsql;
using System.Data.Entity;

namespace MightyGm2.Engine.Database
{
	public class MightyDbConfiguration : DbConfiguration
	{
		public MightyDbConfiguration()
		{
			var name = "Npgsql";

			SetProviderFactory(providerInvariantName: name,
							   providerFactory: NpgsqlFactory.Instance);

			SetProviderServices(providerInvariantName: name,
								provider: NpgsqlServices.Instance);

			SetDefaultConnectionFactory(connectionFactory: new NpgsqlConnectionFactory());
		}
	}
}

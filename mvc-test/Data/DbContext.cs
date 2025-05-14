using System.Data;
using System.Data.SQLite;

namespace DataAccess.AppContext;

public class DbContext(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public IDbConnection CreateConnection() {
        return new SQLiteConnection(_configuration.GetConnectionString(name:"DefaultConnection"));
    }
}
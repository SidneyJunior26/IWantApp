using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration _configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(_configuration["ConnectionString:IWantDb"]);
        var query = $@"SELECT c.ClaimValue as Name, u.Email
                FROM AspNetUsers AS u
                INNER JOIN AspNetUserClaims AS c ON u.Id = c.UserId
                WHERE c.ClaimType = 'Name'
                ORDER BY Name
                OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        return await db.QueryAsync<EmployeeResponse>(
            query,
            new { page, rows }
            );
    }
}

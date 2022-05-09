using Dapper;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryProductsMostBought
{
    private readonly IConfiguration _configuration;

    public QueryProductsMostBought(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<IEnumerable<ProductsSoldResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(_configuration["ConnectionString:IWantDb"]);
        var query = $@"SELECT P.ID, P.NAME, COUNT(P.ID) AS AMOUNT
                FROM PRODUCTS P
                INNER JOIN ORDERPRODUCTS AS O ON P.ID = O.PRODUCTSID
                GROUP BY P.ID, P.NAME
                ORDER BY AMOUNT DESC
                OFFSET (@page - 1) * @rows ROWS FETCH NEXT @rows ROWS ONLY";

        return await db.QueryAsync<ProductsSoldResponse>(
            query,
            new { page, rows }
            );
    }
}

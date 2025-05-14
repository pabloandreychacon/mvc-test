using Dapper;
using DataAccess.AppContext;
using mvc_test.Models;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace mvc_test.Data
{
    public class ExpensesDataAccess(DbContext context)
    {
        public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
        {
            const string sql = @"
                SELECT * FROM [Expenses];
            ";
            using var connection = context.CreateConnection();
            var expenses = await connection.QueryAsync<Expense>(sql);            
            return [.. expenses];
        }        

        public async Task<Expense?> GetExpenseAsync(int? id)
        {
            const string sql = @"
                SELECT * FROM [Expenses]
                WHERE Id=@Id;
            ";
            using var connection = context.CreateConnection();
            var expense = await connection.QuerySingleOrDefaultAsync<Expense>(
                sql,
                new
                {
                    Id = id
                }
            );
            return expense;
        }

        public async Task<int> CreateExpenseAsync(Expense expense)
        {
            const string sql = @"INSERT INTO [Expenses] 
                    ([Price], [Description]) 
                VALUES
                    (@Price, @Description)";
            using var connection = context.CreateConnection();
            var created = await connection.ExecuteAsync(sql, new {
                expense.Price,
                expense.Description,
            });
                
            return created;
        }

        public async Task<int> UpdateExpenseAsync(Expense expense)
        {
            const string sql = @"UPDATE [Expenses] 
                                SET [Price] = @Price ,[Description] = @Description
                                WHERE Id=@Id;";
            using var connection = context.CreateConnection();
            var updated = await connection.ExecuteAsync(sql, expense);
            return updated;
        }

        public async Task<int> DeleteExpenseAsync(int id)
        {
            const string sql = "DELETE FROM [Expenses] WHERE Id=@Id";
            using var connection = context.CreateConnection();
            var deleted = await connection.ExecuteAsync(sql, new { Id = id });
            return deleted;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using aspnetcore_rest_api_with_dapper.Models;
using Dapper;

namespace aspnetcore_rest_api_with_dapper.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private  IDbConnection _connection { get { return new SqliteConnection(_connectionString); }}

        public ProductRepository()
        {
            // TODO: It will be refactored...
            _connectionString = "Data Source=RESTfulSampleDb.db";
        }

        public async Task<Product> GetAsync(long id)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"SELECT [Id] ,[CategoryId]
                                ,[Name]
                                ,[Description]
                                ,[Price]
                                ,[CreatedDate]
                                FROM [Products]
                                WHERE [Id] = @Id";

                var product = await dbConnection.QueryFirstOrDefaultAsync<Product>(query, new{ @Id = id });

                return product;
            }
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            //TODO: Paging...
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"SELECT [Id] ,[CategoryId]
                                ,[Name]
                                ,[Description]
                                ,[Price]
                                ,[CreatedDate]
                                FROM [Products]";

                var product = await dbConnection.QueryAsync<Product>(query);

                return product;
            }
        }

        public async Task AddAsync(Product product)
        {
            using (IDbConnection dbConnection = _connection)
            {
                string query = @"INSERT INTO [Products] (
                                [Id],
                                [CategoryId],
                                [Name],
                                [Description],
                                [Price],
                                [CreatedDate]) VALUES (
                                @Id,
                                @CategoryId,
                                @Name,
                                @Description,
                                @Price,
                                @CreatedDate)";

                await dbConnection.ExecuteAsync(query, product);
            }
        }
    }
}
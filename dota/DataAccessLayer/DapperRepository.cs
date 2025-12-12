using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace DataAccessLayer
{
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string _connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=DotaDB;Integrated Security=True";

        // БАЗОВЫЕ CRUD МЕТОДЫ

        public void Add(T item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO Heroes (Name, Role, Attribute, Complexity) 
                           VALUES (@Name, @Role, @Attribute, @Complexity)";
                connection.Execute(sql, item);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM Heroes WHERE Id = @Id";
                connection.Execute(sql, new { Id = id });
            }
        }

        public void Update(T item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE Heroes 
                           SET Name = @Name, Role = @Role, 
                               Attribute = @Attribute, Complexity = @Complexity 
                           WHERE Id = @Id";
                connection.Execute(sql, item);
            }
        }

        public T GetById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Heroes WHERE Id = @Id";
                return connection.QuerySingleOrDefault<T>(sql, new { Id = id });
            }
        }

        public IEnumerable<T> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Heroes";
                return connection.Query<T>(sql);
            }
        }

        // МЕТОДЫ ПАГИНАЦИИ (ДОБАВЛЯЕМ)

        public IEnumerable<T> GetPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                    SELECT * FROM Heroes 
                    ORDER BY Id 
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var offset = (pageNumber - 1) * pageSize;

                return connection.Query<T>(sql, new
                {
                    Offset = offset,
                    PageSize = pageSize
                });
            }
        }

        public int GetTotalCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT COUNT(*) FROM Heroes";
                return connection.ExecuteScalar<int>(sql);
            }
        }
    }
}
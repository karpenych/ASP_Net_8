using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Dapper_proj.Models
{
    public class Tennisist
    {
        public int       Tennisist_id { get; set; }
        public string?   Sername { get; set; }
        public string?   Name { get; set; }
        public string?   Citizenship { get; set; }
        public int?      Global_rating { get; set; }
        public DateTime? Birth { get; set; }
    }

    public interface IMyRepository 
    {
        void             Create(Tennisist client);
        void             Delete(int id);
        Tennisist        Get(int id);
        List<Tennisist>  GetAll();
        void             Update(Tennisist client);
    }

    public class MyRepository : IMyRepository
    {
        string? connectionString = null;

        public MyRepository(string conn)
        {
            connectionString = conn;
        }

        public List<Tennisist> GetAll()
        {
            using (IDbConnection db = new SqlConnection(connectionString))
            {
                return db.Query<Tennisist>("SELECT * FROM tennisists").ToList();
            } 
        }

        public Tennisist Get(int id) 
        {
            using IDbConnection db = new SqlConnection(connectionString);
            Tennisist? player =
                db.Query<Tennisist>("SELECT * FROM tennisists WHERE tennisist_id = @id", new { id })
                .FirstOrDefault() ?? null;

            return player;
        }

        public void Create(Tennisist tennisist)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            var sqlQuery = "INSERT INTO tennisists (sername, name, citizenship, global_rating, birth) " +
                           "VALUES(@Sername, @Name, @Citizenship, @Global_rating, @Birth)";

            db.Execute(sqlQuery, tennisist);
        }
        
        public void Update(Tennisist tennisist)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            var sqlQuery = "UPDATE tennisists SET " +
                "sername = @Sername, name = @Name, citizenship = @Citizenship, global_rating = @Global_rating, birth = @Birth " +
                "WHERE tennisist_id = @Tennisist_id";

            db.Execute(sqlQuery, tennisist);
        }

        public void Delete(int id)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            var sqlQuery = "DELETE FROM tennisists WHERE tennisist_id = @Tennisist_id";

            db.Execute(sqlQuery, new { id });
        }
    }
}

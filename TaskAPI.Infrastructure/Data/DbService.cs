using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using TaskAPI.Core.Interfaces;

namespace TaskAPI.Infrastructure.Data {
    public class DbService : IDbContext {

        private NpgsqlConnection con;

        public DbService(IConfiguration config) {

            con = new NpgsqlConnection(config.GetConnectionString("postgresConnection"));

        }

        public async Task<TObject> AddObject<TObject>(string query, TObject obj) {
            await con.ExecuteAsync(query, obj);
            return obj;
        }

        public async Task<T> GetObjectByUsername<T>(string query, string username) {
            var res = await con.QueryAsync<T>(query, new { username });

            if (res.ToList().Count == 0) {
                return default;
            }
            return res.ToList().FirstOrDefault();
        }

        public async Task UpdateObject<TObject>(string query, TObject obj) {
            await con.ExecuteAsync(query, obj);
        }

        public async Task DeleteUser(string query, string username) {
            await con.ExecuteAsync(query, new { username });
        }

        public async Task<IEnumerable<TObject>> GetAllObjects<TObject>(string query) {
            return await con.QueryAsync<TObject>(query);
        }
    }
}

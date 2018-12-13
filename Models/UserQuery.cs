using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace chatroom.Models
{
    public class UserQuery
    {

        public readonly AppDb Db;
        public UserQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<User> FindOneAsync(int id)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT `id`, `name` FROM `User` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<User>> AllAsync()
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `name` FROM `User` ORDER BY `id` DESC";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task<User> FindOneByNameAsync(string name)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT `id` , `name` FROM `User` WHERE LOWER(`name`) = @name";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@name",
                DbType = DbType.String,
                Value = name.ToLower(),
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task DeleteAllAsync()
        {
            var txn = await Db.Connection.BeginTransactionAsync();
            try
            {
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"DELETE FROM `User`";
                await cmd.ExecuteNonQueryAsync();
                await txn.CommitAsync();
            }
            catch
            {
                await txn.RollbackAsync();
                throw;
            }
        }

        private async Task<List<User>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new User(Db)
                    {
                        id = await reader.GetFieldValueAsync<int>(0),
                        name = await reader.GetFieldValueAsync<string>(1),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
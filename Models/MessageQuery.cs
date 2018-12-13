using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace chatroom.Models
{
    public class MessageQuery
    {

        public readonly AppDb Db;
        public MessageQuery(AppDb db)
        {
            Db = db;
        }

        public async Task<Message> FindOneAsync(int id)
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT `id`, `userid`, `text` FROM `Message` WHERE `id` = @id";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }

        public async Task<List<Message>> AllAsync()
        {
            var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `id`, `userid`, `text` FROM `Message` ORDER BY `id` ASC LIMIT 10;";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }

        public async Task DeleteAllAsync()
        {
            var txn = await Db.Connection.BeginTransactionAsync();
            try
            {
                var cmd = Db.Connection.CreateCommand();
                cmd.CommandText = @"DELETE FROM `Message`";
                await cmd.ExecuteNonQueryAsync();
                await txn.CommitAsync();
            }
            catch
            {
                await txn.RollbackAsync();
                throw;
            }
        }

        private async Task<List<Message>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<Message>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new Message(Db)
                    {
                        id = await reader.GetFieldValueAsync<int>(0),
                        userid = await reader.GetFieldValueAsync<int>(1),
                        text = await reader.GetFieldValueAsync<string>(2)
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
    }
}
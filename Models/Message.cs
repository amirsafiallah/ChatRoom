using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace chatroom.Models
{
    public class Message
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string text { get; set; }

        [JsonIgnore]
        public AppDb Db { get; set; }

        public Message(AppDb db=null)
        {
            Db = db;
        }

        public async Task InsertAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO `Message` (`userid`, `text`) VALUES (@userid, @text);";
            BindParams(cmd);
            await cmd.ExecuteNonQueryAsync();
            id = (int) cmd.LastInsertedId;
        }

        public async Task UpdateAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE `Message` SET `userid` = @userid, `text` = @text WHERE `id` = @id;";
            BindParams(cmd);
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync()
        {
            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM `Message` WHERE `id` = @id;";
            BindId(cmd);
            await cmd.ExecuteNonQueryAsync();
        }

        private void BindId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@id",
                DbType = DbType.Int32,
                Value = id,
            });
        }

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@text",
                DbType = DbType.String,
                Value = text,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userid",
                DbType = DbType.Int32,
                Value = userid,
            });
        }

    }
}
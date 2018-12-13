using System;
using MySql.Data.MySqlClient;

namespace chatroom
{
	public class AppDb : IDisposable
	{
        private static string connectionString;

		public static void Initialize(string cnStr)
		{
			connectionString = cnStr;
		}

		public MySqlConnection Connection;

        public AppDb()
		{
			Connection = new MySqlConnection(connectionString);
		}

		public void Dispose()
		{
			Connection.Close();
		}
	}
}
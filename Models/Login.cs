using System;
using System.Data;
using System.Data.SqlClient;

namespace api
{
	public class Login
	{
		public string APIKey;
		public string Error;
		public string FirstName;
		public string LastName;
		public string Username;

		public Login(string user, string pass, string type = "user")
		{
			using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("SQLAZURECONNSTR_bit4454database")))
			{
				SqlCommand command;
				if (type == "admin")
				{
					command = new SqlCommand("SELECT APIKey, FirstName, LastName, Username, Password FROM Admins, Managers WHERE Username = @user AND Admins.Manager_ID = Managers.ID", connection);
				}
				else
				{
					command = new SqlCommand("SELECT APIKey, FirstName, LastName, Username, AuthKey FROM Users, Buyers WHERE Username = @user AND Users.Buyer_ID = Buyers.ID", connection);
				}
				command.Parameters.Add("@user", SqlDbType.VarChar).Value = user;
				connection.Open();
				var reader = command.ExecuteReader();
				reader.Read();
				if (reader.HasRows)
				{
					if ((string)reader[3] == user)
					{
						if ((string)reader[4] == pass)
						{
							APIKey = (string)reader[0];
							FirstName = (string)reader[1];
							LastName = (string)reader[2];
							Username = (string)reader[3];
						}
						else
						{
							Username = user;
							Error = "Invalid password";
						}
					}
					else
					{
						Username = user;
						Error = "Invalid username";
					}
				}
				else
				{
					Username = user;
					Error = "Invalid username";
				}
			}
		}
	}
}

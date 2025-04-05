using MySql.Data.MySqlClient;
using Rocket.Core.Logging;

namespace ArenaKits
{
    public class DatabaseMgr
    {
        private readonly ArenaKitsPlugin _arenaKits;

        internal DatabaseMgr(ArenaKitsPlugin arenaKits)
        {
            _arenaKits = arenaKits;
            CheckSchema();
        }

        private void CheckSchema()
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection() ?? throw new Exception("Database connection failure");
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlConnection.Open();
                mySqlCommand.CommandText = string.Concat(
                "CREATE TABLE IF NOT EXISTS `",
                _arenaKits.Configuration.Instance.ArenaKitsTableName,
                "` (",
                "`steamId` VARCHAR(32) NOT NULL,",
                "`selectedKit` VARCHAR(32),",
                "PRIMARY KEY (`steamId`)",
                ");"
            );
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"[ArenaKits] Database Crashed by Console when trying to create or check existing table {_arenaKits.Configuration.Instance.ArenaKitsTableName}, reason: {exception.Message}");
            }
        }

        public MySqlConnection? CreateConnection()
        {
            try
            {
                return new MySqlConnection(string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", _arenaKits.Configuration.Instance.DatabaseAddress, _arenaKits.Configuration.Instance.DatabaseName, _arenaKits.Configuration.Instance.DatabaseUsername, _arenaKits.Configuration.Instance.DatabasePassword, _arenaKits.Configuration.Instance.DatabasePort));
            }
            catch (Exception exception)
            {
                Logger.LogError($"[ArenaKits] Instance Connection Database Crashed, reason: {exception.Message}");
            }
            return null;
        }

        /// <summary>
        /// Add a new player to the ArenaKits table if not exist
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="balance"></param>
        public void AddNewPlayer(string playerId)
        {
            try
            {
                // Instanciate connection
                MySqlConnection mySqlConnection = CreateConnection() ?? throw new Exception("Database connection failure"); ;
                // Instanciate command
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                // Command: Insert new player only if not exist the same steamId
                mySqlCommand.CommandText = string.Concat("Insert ignore into `", _arenaKits.Configuration.Instance.ArenaKitsTableName, "` (`steamId`) VALUES (@playerId);");
                // Add parameter for playerId
                mySqlCommand.Parameters.AddWithValue("@playerId", playerId);
                // Try to connect
                mySqlConnection.Open();
                // Execute the command
                mySqlCommand.ExecuteNonQuery();
                // Close connection
                mySqlConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.LogError($"[ArenaKits] Database Crashed by {playerId} from function AddNewPlayer, reason: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns the player kit from the table arenaKits
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public string? GetKit(string playerId)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection() ?? throw new Exception("Database connection failure");
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = string.Concat("select `selectedKit` from `", _arenaKits.Configuration.Instance.ArenaKitsTableName, "` where `steamId` = @playerId;");
                mySqlCommand.Parameters.AddWithValue("@playerId", playerId);
                mySqlConnection.Open();
                object obj = mySqlCommand.ExecuteScalar();

                if (obj == null)
                {
                    mySqlConnection.Close();
                    return null;
                }

                string result = obj.ToString();

                mySqlConnection.Close();

                return result;
            }
            catch (Exception exception)
            {
                Logger.LogError($"[ArenaKits] Database Crashed by {playerId} from function GetKit, reason: {exception.Message}");
            }
            return null;
        }

        /// <summary>
        /// Set the player kit
        /// </summary>
        /// <param name="id"></param>
        /// <param name="kit"></param>
        public void SetKit(string id, string kit)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection() ?? throw new Exception("Database connection failure");
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = $"update `{_arenaKits.Configuration.Instance.ArenaKitsTableName}` set `selectedKit` = @kit where `steamId` = @id;";
                mySqlCommand.Parameters.AddWithValue("@kit", kit);
                mySqlCommand.Parameters.AddWithValue("@id", id);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"[ArenaKits] Database Crashed by {id} from function SetKit, reason: {exception.Message}");
            }
        }

        /// <summary>
        /// Remove the player kit
        /// </summary>
        /// <param name="id"></param>
        public void RemoveKit(string id)
        {
            try
            {
                MySqlConnection mySqlConnection = CreateConnection() ?? throw new Exception("Database connection failure");
                MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
                mySqlCommand.CommandText = $"update `{_arenaKits.Configuration.Instance.ArenaKitsTableName}` set `selectedKit` = @kit where `steamId` = @id;";
                mySqlCommand.Parameters.AddWithValue("@kit", null);
                mySqlCommand.Parameters.AddWithValue("@id", id);
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
            }
            catch (Exception exception)
            {
                Logger.LogError($"[ArenaKits] Database Crashed by {id} from function RemoveKit, reason: {exception.Message}");
            }
        }
    }
}
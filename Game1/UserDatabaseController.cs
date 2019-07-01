using System;
using System.Collections.Generic;
using SQLite;

namespace Game1
{
    /// <summary>
    /// this class provides a tool for activities to access the databse and perform specific quries:
    /// - get user
    /// - get all users
    /// - add user
    /// - add game
    /// - get all games
    /// - update high score 
    /// </summary>
    public class UserDatabaseController
    {
        private SQLiteConnection database;

        public enum UserCheckErr
        {
            USERNAME_ERR = -1,
            PASSWORD_ERR = -2
        }
        public UserDatabaseController(bool isAndroid = true)
        {
            SQLite_Android androidDbConnection = new SQLite_Android();
            database = androidDbConnection.GetConnection();
            database.CreateTable<User>();
        }
        /// <summary>
        /// Checks if the username is unique
        /// </summary>
        /// <returns><c>true</c>, if user was checked, <c>false</c> otherwise.</returns>
        /// <param name="p_Username">P username.</param>
        public bool checkUsername(string p_Username)
        {
            database.CreateTable<User>();
            return (database.Table<User>().Where(i => i.UserName == p_Username).FirstOrDefault() != null);
        }

        public int checkUserPassword(string p_Username, string p_Password)
        {
            database.CreateTable<User>();
            User thisUser = database.Table<User>().Where(i => i.UserName == p_Username).FirstOrDefault();
            if (thisUser == null)
                return (int)UserCheckErr.USERNAME_ERR;
            if (thisUser.Password != p_Password)
                return (int)UserCheckErr.PASSWORD_ERR;
            else
                return thisUser.Id;
        }

        public User GetUser(int UserId)
        {
            database.CreateTable<User>();
            User thisUser = database.Table<User>().Where(i => i.Id == UserId).FirstOrDefault();
            return (thisUser);
        }

        public List<User> GetUsers ()
        {
            database.CreateTable<User>();
            return database.Table<User>().ToList();
        }

        public int saveUser(string p_Username, string p_Password, bool p_bIsAdmin = false, int p_HighScore = 0)
        {
            if (checkUsername(p_Username)) 
                return 0;
            User newUser = new User();
            newUser.Password = p_Password;
            newUser.UserName = p_Username;
            newUser.IsAdmin = p_bIsAdmin;
            newUser.HighScore = p_HighScore;
            if (database.Insert(newUser) == 0)
                return 0;
             
            return checkUserPassword(p_Username, p_Password);   //return Id of new user
        }

        public void UpdateHighScore(int UserId, int HighSCore)
        {
            database.CreateTable<User>();
            User currUser = GetUser(UserId);
            currUser.HighScore = HighSCore;
            database.InsertOrReplace(currUser);
        }

        public User GetLastUser()
        {
            database.CreateTable<Game>();
            User lastUser = null;
            int userId;
            string query = "SELECT * FROM GAME WHERE _id = (SELECT MAX(_id)  FROM GAME)";
            SQLiteCommand command = new SQLiteCommand(database);
            command.CommandText = query;
            List<Game> games = command.ExecuteQuery<Game>();
            if (games.Count != 0)
            {
                userId = games[0].UserId;
                lastUser = GetUser(userId);
            }
            return lastUser;
        }

        public List<Game> GetGames()
        {
            database.CreateTable<Game>();
            return database.Table<Game>().ToList();
        }


        public int InsertGame(int UserId, int Score)
        {
            database.CreateTable<Game>();
            Game game = new Game();
            game.FinalScore = Score;
            game.UserId = UserId;
            return (database.Insert(game));
        }

    }
    /// <summary>
    /// this class defines the collumns for the game entry
    /// to add an attribute update here
    /// </summary>
    public class Game
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FinalScore { get; set; }
        public Game()
        {
            Id = 0;
            UserId = 0;
            FinalScore = 0;
        }
    }

    /// <summary>
    /// this class defines the collumns for the user entry
    /// to add an attribute update here
    /// </summary>
    public class User
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [MaxLength(8)]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int HighScore { get; set; }
        public bool IsAdmin { get; set; }
        public User()
        {
            UserName = "";
            Password = "";
            HighScore = 0;
            IsAdmin = false;
        }
    }
}

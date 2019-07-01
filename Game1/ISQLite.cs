using System;
using SQLite;

namespace Game1
{
    /// <summary>
    /// this interface options a different GetConnection implementation for ios and android (currently only android)
    /// </summary>
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}

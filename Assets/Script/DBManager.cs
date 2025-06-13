using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;

public static class DBManager
{
    private static IDbConnection conn;

    private static string dbPath = Path.Combine(Application.streamingAssetsPath, "game.db");

    public static void Init()
    {
        if (conn == null)
        {
            conn = new SqliteConnection("URI=file:" + dbPath);
            conn.Open();
        }
    }

    public static void Close()
    {
        if (conn != null)
        {
            conn.Close();
            conn = null;
        }
    }

    public static void test()
    {

    }
}

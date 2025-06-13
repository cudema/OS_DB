using System.IO;
using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using UnityEngine.SocialPlatforms.Impl;

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

    public static void AddRanking(GameMode mode, string userName, SizeType size, MoveType move, int score)
    {
        string modeName;
        if (mode == GameMode.HitScan)
        {
            modeName = "HitscanRankingTBL";
        }
        else
        {
            modeName = "TrrakingRankingTBL";
        }

        int ranking = SetRaking(modeName, score);

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"INSERT INTO {modeName} VALUES({ranking}, @name, @size, @speed, @score)";

        var param1 = cmd.CreateParameter();
        param1.ParameterName = "@name";
        param1.Value = userName;
        cmd.Parameters.Add(param1);

        var param2 = cmd.CreateParameter();
        param2.ParameterName = "@size";
        param2.Value = size.ToString();
        cmd.Parameters.Add(param2);

        var param3 = cmd.CreateParameter();
        param3.ParameterName = "@speed";
        param3.Value = move.ToString();
        cmd.Parameters.Add(param3);

        var param4 = cmd.CreateParameter();
        param4.ParameterName = "@score";
        param4.Value = score;
        cmd.Parameters.Add(param4);

        cmd.ExecuteNonQuery();
    }

    static int SetRaking(string mode, int score)
    {
        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT Ranking, Score FROM {mode} ORDER BY Ranking";

        var reader = cmd.ExecuteReader();

        if (!reader.Read())
        {
            return 1;
        }

        int currentRanking;

        do
        {
            currentRanking = int.Parse(reader["Ranking"].ToString());
            Debug.Log(currentRanking);
            if (score > int.Parse(reader["Score"].ToString()))
            {
                reader.Close();

                cmd.CommandText = $"UPDATE {mode} SET Ranking = Ranking + 1 WHERE Ranking >= {currentRanking}";
                cmd.ExecuteNonQuery();
                return currentRanking;
            }
        } while (reader.Read());

        return currentRanking + 1;
    }

    public static string[][] GetRankingBoard(string mode)
    {
        string[][] temp = new string[5][];

        string modeName;
        if (mode == GameMode.HitScan.ToString())
        {
            modeName = "HitscanRankingTBL";
        }
        else
        {
            modeName = "TrrakingRankingTBL";
        }

        var cmd = conn.CreateCommand();
        cmd.CommandText = $"SELECT * FROM {modeName} ORDER BY Ranking LIMIT 5";

        var reader = cmd.ExecuteReader();

        for (int i = 0; i < 5; i++)
        {
            if (!reader.Read())
            {
                break;
            }

            temp[i] = new string[reader.FieldCount];

            for (int j = 0; j < reader.FieldCount; j++)
            {
                temp[i][j] = reader[j].ToString();
            }
        }

        return temp;
    }
}

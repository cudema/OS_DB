using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Mono.Data.Sqlite;

public static class RunTimeDB
{
    public static void AddOrUpdateRunTime(string userId, int seconds)
    {
        DBManager.Init();
        string today = DateTime.Now.ToString("yyyy-MM-dd");

        var conn = GetConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM RunningTBL WHERE UserID = @id AND Date = @date";

        AddParam(cmd, "@id", userId);
        AddParam(cmd, "@date", today);

        int count = Convert.ToInt32(cmd.ExecuteScalar());

        if (count > 0)
            cmd = conn.CreateCommand().WithCommand("UPDATE RunningTBL SET RunTime = RunTime + @sec WHERE UserID = @id AND Date = @date");
        else
            cmd = conn.CreateCommand().WithCommand("INSERT INTO RunningTBL(UserID, Date, RunTime) VALUES(@id, @date, @sec)");

        AddParam(cmd, "@id", userId);
        AddParam(cmd, "@date", today);
        AddParam(cmd, "@sec", seconds);
        cmd.ExecuteNonQuery();
    }

    public static List<(string date, int seconds)> GetMyRecentRunTimes(string userId)
    {
        DBManager.Init();
        var list = new List<(string, int)>();

        var conn = GetConnection();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT Date, RunTime FROM RunningTBL WHERE UserID = @id ORDER BY Date DESC LIMIT 5";
        AddParam(cmd, "@id", userId);

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
                list.Add((reader.GetString(0), reader.GetInt32(1)));
        }

        return list;
    }

    private static IDbConnection GetConnection() =>
        typeof(DBManager).GetField("conn", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).GetValue(null) as IDbConnection;

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }

    // 커맨드 텍스트 바로 설정하는 도우미
    private static IDbCommand WithCommand(this IDbCommand cmd, string text)
    {
        cmd.CommandText = text;
        return cmd;
    }
}

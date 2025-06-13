using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class RunTimeRankingUI : MonoBehaviour
{
    public List<TMP_Text> lines;

    void OnEnable()
    {
        string userId = RunTimeTracker.Instance.userId;
        var records = RunTimeDB.GetMyRecentRunTimes(userId);

        foreach (var line in lines)
        {
            line.text = "";
        }

        for (int i = 0; i < records.Count && i < lines.Count; i++)
        {
            string date = records[i].date;
            int seconds = records[i].seconds;
            string time = FormatTime(seconds);

            lines[i].text = $"{i + 1} / {userId} / {date} / {time}";
        }
    }

    string FormatTime(int sec)
    {
        int h = sec / 3600;
        int m = (sec % 3600) / 60;
        int s = sec % 60;
        return $"{h:D2}:{m:D2}:{s:D2}";
    }
}

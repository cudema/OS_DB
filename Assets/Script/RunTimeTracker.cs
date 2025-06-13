using UnityEngine;

public class RunTimeTracker : MonoBehaviour
{
    public static RunTimeTracker Instance;
    public string userId;

    private float elapsedTime = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    void OnApplicationQuit()
    {
        int seconds = Mathf.FloorToInt(elapsedTime);
        RunTimeDB.AddOrUpdateRunTime(userId, seconds);
        Debug.Log($"플레이타임 저장 완료: {seconds}초");
    }
}

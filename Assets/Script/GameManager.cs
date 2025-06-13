using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    static GameManager gameManager;
    [SerializeField]
    TargetSpawner targetSpawner;

    [SerializeField]
    GameObject allUI;
    [SerializeField]
    GameObject aim;
    [SerializeField]
    GameObject currentScore;
    [SerializeField]
    GameObject scoreB;

    [SerializeField]
    Gun gun;

    int score;
    [SerializeField]
    int maxGameTime = 60;

    float currentGameTime = 0;

    public GameMode gameMode;

    public string currentUserName;

    public int Score
    {
        private set => score = value;
        get => score;
    }

    public bool IsGamePlsying { private set; get; }

    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DBManager.Init();
    }

    private void Update()
    {
        if (IsGamePlsying && Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }

        if (currentGameTime >= maxGameTime)
        {
            currentGameTime = 0;
            EndGame();
        }

        if (IsGamePlsying)
        {
            currentGameTime += Time.deltaTime;
        }
    }

    public void BStartGame()
    {
        StartGame();
    }

    public void BChangeModeToHitscan()
    {
        targetSpawner.ChangeGameMode(GameMode.HitScan);
    }

    public void BChangeModeToTraking()
    {
        targetSpawner.ChangeGameMode(GameMode.Traking);
    }

    //시작시 초기화
    public void StartGame()
    {
        targetSpawner.StartShootTarget();
        allUI.SetActive(false);
        aim.SetActive(true);
        currentScore.SetActive(true);
        gun.ResetGun();
        IsGamePlsying = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        score = 0;
    }

    //게임 종료시 실행
    public void EndGame()
    {
        DBManager.AddRanking(gameMode, currentUserName, targetSpawner.sizeType, targetSpawner.moveType, Score);
        targetSpawner.EndShootTarget();
        currentScore.SetActive(false);
        aim.SetActive(false);
        scoreB.SetActive(true);
        SetScoreB();
        IsGamePlsying = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResetGame()
    {
        allUI.SetActive(true);
        scoreB.SetActive(false);
    }

    public void AddScore(int addScore)
    {
        score += addScore;
    }

    void SetScoreB()
    {
        scoreB.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"Score: {score}";
    }
}
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
    GameObject ScoreBoard;

    [SerializeField]
    int score;

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
    }

    private void Update()
    {
        if (IsGamePlsying && Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        targetSpawner.StartShootTarget();
        allUI.SetActive(false);
        aim.SetActive(true);
        ScoreBoard.SetActive(true);
        IsGamePlsying = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        score = 0;
    }

    public void EndGame()
    {
        targetSpawner.EndShootTarget();
        allUI.SetActive(true);
        aim.SetActive(false);
        ScoreBoard.SetActive(false);
        IsGamePlsying = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void AddScore()
    {
        score++;
    }
}

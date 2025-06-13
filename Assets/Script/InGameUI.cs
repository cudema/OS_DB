using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    TMP_Text score;
    [SerializeField]
    TMP_Text time;
    [SerializeField]
    TMP_Text highScore;

    private void Update()
    {
        score.text = $"Score : {gameManager.Score}";
        time.text = $"{gameManager.currentGameTime.ToString("00.00")}";
        highScore.text = $"High Score : {DBManager.GetHighScore(gameManager.gameMode, gameManager.currentUserName)}";
    }
}

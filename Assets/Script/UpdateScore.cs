using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour // 스코어 표시
{
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    GameManager manager;

    void Update()
    {
        score.text = $"Score: {manager.Score}";
    }
}

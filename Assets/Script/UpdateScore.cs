using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour
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

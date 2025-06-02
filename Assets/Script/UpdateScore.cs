using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour // ���ھ� ǥ��
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

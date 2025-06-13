using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum GameMode { HitScan = 0, Traking }

public class TargetSpawner : MonoBehaviour
{
    [System.Serializable]
    class StageData
    {
        [SerializeField]
        public List<Vector3> targetVectorList = new List<Vector3>();   //타겟 위치 원본
        [SerializeField]
        public GameObject targetPrefap;
        [SerializeField]
        [Range(0, 8)]
        public int spawnCount = 1;
    }

    [SerializeField]
    StageData[] stageData;
    StageData currentStageData = null;

    [SerializeField]
    public float EtinctionTime = 4;

    public MoveType moveType;
    public SizeType sizeType;

    [SerializeField]
    TMP_Dropdown[] moveTypeDropdown = new TMP_Dropdown[2];
    [SerializeField]
    TMP_Dropdown[] sizeTypeDropdown = new TMP_Dropdown[2];

    List<Vector3> SpawndTargetList = new List<Vector3>();   //타겟 위치 복사본

    [SerializeField]
    public GameManager manager;

    MemoryPool memoryPool;

    private void Awake()
    {

    }

    //시작시 지정한 수 만큼의 타겟 소환
    public void StartShootTarget()
    {
        if (currentStageData == null)
        {
            Debug.Log("게임 모드 오류");
            return;
        }

        memoryPool = new MemoryPool(currentStageData.targetPrefap, currentStageData.spawnCount);
        SpawndTargetList = currentStageData.targetVectorList.ToList();
        SpawnTarget();
    }

    //끝날 때 타겟 삭제
    public void EndShootTarget()
    {
        SpawndTargetList.Clear();
        memoryPool.DestroyObjects();
    }

    void SpawnTarget()
    {
        ChangeMoveMode();
        for (int i = 0; i < currentStageData.spawnCount; i++)
        {
            int index = Random.Range(0, SpawndTargetList.Count);

            GameObject cloen = memoryPool.ActivatePoolItem();
            cloen.transform.position = SpawndTargetList[index];
            cloen.GetComponent<Target>().resetPosition = SpawndTargetList[index];
            cloen.GetComponent<Target>().SetMode(moveType, sizeType);
            SpawndTargetList.RemoveAt(index);
        }
    }

    //타겟이 사라지고 다시 나타날 때 위치 설정
    public void ChangeTargetVector(Target target)
    {
        int index = Random.Range(0, SpawndTargetList.Count);
        Vector3 temp = target.resetPosition;

        target.transform.position = SpawndTargetList[index];
        target.resetPosition = SpawndTargetList[index];
        SpawndTargetList.RemoveAt(index);
        SpawndTargetList.Add(temp);
    }

    public void ChangeGameMode(GameMode mode)
    {
        currentStageData = stageData[(int)mode];
        manager.gameMode = mode;
    }

    public void ChangeMoveMode()
    {
        moveType = (MoveType)moveTypeDropdown[(int)manager.gameMode].value;
        sizeType = (SizeType)sizeTypeDropdown[(int)manager.gameMode].value;
    }
}
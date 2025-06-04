using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderGraph.Configuration;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField]
    List<Vector3> targetVectorList = new List<Vector3>();   //타겟 위치 원본
    List<Vector3> SpawndTargetList = new List<Vector3>();   //타겟 위치 복사본
    [SerializeField]
    GameObject targetPrefap;
    [SerializeField]
    [Range(0, 8)]
    int spawnCount = 1;
    [SerializeField]
    public float EtinctionTime = 4;
    [SerializeField]
    public GameManager manager;

    MemoryPool memoryPool;

    private void Awake()
    {

    }

    //시작시 지정한 수 만큼의 타겟 소환
    public void StartShootTarget()
    {
        memoryPool = new MemoryPool(targetPrefap, spawnCount);
        SpawndTargetList = targetVectorList.ToList();
        SpawnTarget();
    }

    //끝날 때 타겟 삭제
    public void EndShootTarget()
    {
        SpawndTargetList.Clear();
        memoryPool.DestroyObjects();
    }

    //타겟을 소환
    void SpawnTarget()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            int index = Random.Range(0, SpawndTargetList.Count);

            GameObject cloen = memoryPool.ActivatePoolItem();
            cloen.transform.position = SpawndTargetList[index];
            SpawndTargetList.RemoveAt(index);
        }
    }

    //타겟이 사라지고 다시 나타날 때 위치 설정
    public void ChangeTargetVector(Target target)
    {
        int index = Random.Range(0, SpawndTargetList.Count);
        Vector3 temp = target.transform.position;

        target.transform.position = SpawndTargetList[index];
        SpawndTargetList.Add(temp);
        SpawndTargetList.RemoveAt(index);
    }
}
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    TargetSpawner spawner;

    private void Awake()
    {
        spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
    }

    private void Start()
    {
        StartCoroutine(EtinctionTarget());
    }

    //타겟을 맞췄을 때 실행
    public void Hit()
    {
        spawner.ChangeTargetVector(this);
        spawner.manager.AddScore();
        StopAllCoroutines();
        StartCoroutine(EtinctionTarget());
    }

    //타겟이 자동 삭제되는 부분
    IEnumerator EtinctionTarget()
    {
        yield return new WaitForSeconds(spawner.EtinctionTime);

        spawner.ChangeTargetVector(this);
        StartCoroutine(EtinctionTarget());
    }
}
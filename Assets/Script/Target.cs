using System.Collections;
using UnityEngine;

public enum MoveType { Min = 0, Middle, Max, Random }
public enum SizeType { Min = 0, Middle, Max, Random }

public class Target : MonoBehaviour
{
    TargetSpawner spawner;
    public int HP;
    [SerializeField]
    int currentHP;

    [HideInInspector]
    public Vector3 resetPosition;

    [SerializeField]
    float minMoveSpeed;
    [SerializeField]
    float maxMoveSpeed;
    [SerializeField]
    float movePositionRange;

    [SerializeField]
    float minSize;
    [SerializeField]
    float maxSize;

    public MoveType movetype;
    public SizeType sizetype;

    float currentMoveSpeed;

    bool isMoveX = true;

    [SerializeField]
    bool isMove = false;

    float thisScore = 1;

    private void Awake()
    {
        spawner = GameObject.Find("TargetSpawner").GetComponent<TargetSpawner>();
    }

    private void Start()
    {
        StartCoroutine(EtinctionTarget());
        SetTarget();
    }

    private void Update()
    {
        MoveTarget();
    }

    //타겟을 맞췄을 때 실행
    public void Hit()
    {
        currentHP--;

        if (currentHP != 0)
        {
            return;
        }

        spawner.ChangeTargetVector(this);
        spawner.manager.AddScore((int)thisScore);
        StopAllCoroutines();
        StartCoroutine(EtinctionTarget());
        SetTarget();
    }

    //타겟이 자동 삭제되는 부분
    IEnumerator EtinctionTarget()
    {
        yield return new WaitForSeconds(spawner.EtinctionTime);

        spawner.ChangeTargetVector(this);
        StartCoroutine(EtinctionTarget());
        SetTarget();
    }

    void SetTarget()
    {
        isMoveX = Random.Range(0.0f, 1.0f) < 0.5f ? true : false;
        currentHP = HP;

        switch (sizetype)
        {
            case SizeType.Min:
                transform.localScale = new Vector3(1, 1, 1) * minSize;
                thisScore = 500;
                break;
            case SizeType.Middle:
                thisScore = 300;
                break;
            case SizeType.Max:
                transform.localScale = new Vector3(1, 1, 1) * maxSize;
                thisScore = 100;
                break;
            case SizeType.Random:
                transform.localScale = new Vector3(1, 1, 1) * Random.Range(minSize, maxSize);
                if (transform.localScale.x < 0.75f)
                {
                    thisScore = 500;
                }
                else if (transform.localScale.x < 1.5f)
                {
                    thisScore = 300;
                }
                else
                {
                    thisScore = 100;
                }
                break;
            default:
                Debug.Log("SizeType 오류");
                break;
        }

        if (isMove || Random.Range(0.0f, 1.0f) < 0.2f)
        {
            switch (movetype)
            {
                case MoveType.Min:
                    currentMoveSpeed = minMoveSpeed;
                    thisScore *= 2;
                    break;
                case MoveType.Middle:
                    currentMoveSpeed = Mathf.Lerp(minMoveSpeed, maxMoveSpeed, 0.5f);
                    thisScore *= 1.5f;
                    break;
                case MoveType.Max:
                    currentMoveSpeed = maxMoveSpeed;
                    break;
                case MoveType.Random:
                    currentMoveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
                    if (currentMoveSpeed > minMoveSpeed * 1.65f)
                    {
                        thisScore *= 2;
                    }
                    else if (currentMoveSpeed > minMoveSpeed * 1.3f)
                    {
                        thisScore *= 1.5f;
                    }
                    break;
                default:
                    Debug.Log("MoveType 오류");
                    break;
            }
        }
        else
        {
            currentMoveSpeed = 0;
        }
    }

    void MoveTarget()
    {
        if (isMoveX)
        {
            transform.position += new Vector3(currentMoveSpeed * Time.deltaTime, 0, 0);
            if (transform.position.x < resetPosition.x - movePositionRange || transform.position.x > resetPosition.x + movePositionRange)
            {
                currentMoveSpeed = -currentMoveSpeed;
            }
        }
        else
        {
            transform.position += new Vector3(0, currentMoveSpeed * Time.deltaTime, 0);
            if (transform.position.y < resetPosition.y - movePositionRange || transform.position.y > resetPosition.y + movePositionRange)
            {
                currentMoveSpeed = -currentMoveSpeed;
            }
        }
    }

    public void SetMode(MoveType type, SizeType size)
    {
        movetype = type;
        sizetype = size;
    }
}
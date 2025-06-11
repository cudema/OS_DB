using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("�ѱ� �Ķ����")]
    public float fireRate = 10f;               // �ʴ� �߻��
    public int maxAmmo = 30;                   // ��ź��
    public float reloadTime = 1.5f;            // ���� �ð�

    [Range(1f, 2000f)]
    public float range = 500f;                 // Inspector���� 1~2000 �Է�, ���� ��Ÿ��� RANGE_SCALE ��
    private const float RANGE_SCALE = 100f;    // ���� ���� ��Ÿ� ����

    public float bulletRadius = 0.05f;         // ���� �� ������(���� 0.1)

    [Header("����(Tracer) �ɼ�")]
    public bool useTracer = true;              // Inspector���� ���� ON/OFF
    public GameObject tracerPrefab;            // LineRenderer ������ (������ Debug.DrawLine)

    [Header("���� �÷���(�ѱ� ȭ��) �ɼ�")]
    public bool useMuzzleFlash = true;         // Inspector���� ON/OFF
    public GameObject muzzleFlashPrefab;       // ���� �÷��� ������
    public float muzzleFlashDuration = 0.06f;  // �ѱ�ȭ�� ���ӽð�

    [Header("����")]
    public Camera mainCam;                     // Main Camera ����
    public Transform muzzlePoint;              // �ѱ� ��ġ (Gun �ڽ� MuzzlePoint ����)

    private int currentAmmo;
    private float fireCooldown = 0f;
    private bool isReloading = false;

    [Header("")]
    [SerializeField]
    GameManager gameManager;

    void Start()
    {
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // �߻�(��Ŭ��)
        if (!isReloading && Input.GetButton("Fire1") && fireCooldown <= 0f && currentAmmo > 0)
            Fire();

        // ����(RŰ)
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            StartCoroutine(Reload());
    }

    void Fire()
    {
        fireCooldown = 1f / fireRate;
        currentAmmo--;

        float realRange = range * RANGE_SCALE;

        // ī�޶�(����) �߽ɿ��� Raycast
        Vector3 fireOrigin = mainCam.transform.position;
        Vector3 fireDir = mainCam.transform.forward;
        Ray ray = new Ray(fireOrigin, fireDir);

        RaycastHit hit;
        Vector3 hitPoint;

        if (gameManager.IsGamePlsying)
        {
            if (Physics.SphereCast(ray, bulletRadius, out hit, realRange))
            {
                hitPoint = hit.point;
                Debug.Log("�Ѿ��� ���� ��ü: " + hit.collider.name);
                if (hit.collider.tag.Equals("Target"))
                {
                    Debug.Log("hit");
                    hit.collider.GetComponent<Target>().Hit();
                }
            }
            else
            {
                hitPoint = fireOrigin + fireDir * realRange;
            }

            Vector3 tracerOrigin = muzzlePoint.position;

            // 1. ����(Tracer) ���
            if (useTracer)
            {
                if (tracerPrefab != null)
                {
                    GameObject tracerObj = Instantiate(tracerPrefab, tracerOrigin, Quaternion.identity);
                    var lr = tracerObj.GetComponent<LineRenderer>();
                    lr.SetPosition(0, tracerOrigin);
                    lr.SetPosition(1, hitPoint);
                    Destroy(tracerObj, 0.05f);
                }
                else
                {
                    Debug.DrawLine(tracerOrigin, hitPoint, Color.yellow, 0.05f, true);
                }
            }

            // 2. ���� �÷���(�ѱ� ȭ��) ���
            if (useMuzzleFlash && muzzleFlashPrefab != null && muzzlePoint != null)
            {
                StartCoroutine(ShowMuzzleFlash());
            }
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("���� ��...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("���� �Ϸ�");
    }

    private System.Collections.IEnumerator ShowMuzzleFlash()
    {
        GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint);
        ParticleSystem ps = flash.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
        yield return new WaitForSeconds(muzzleFlashDuration);
        Destroy(flash);
    }
}

using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("총기 파라미터")]
    public float fireRate = 10f;               // 초당 발사수
    public bool isInfiniteAmmo = false;        // 무한 탄창 모드
    public int maxAmmo = 30;                   // 장탄수
    public float reloadTime = 1.5f;            // 장전 시간

    [Range(1f, 2000f)]
    public float range = 500f;                 // Inspector에서 1~2000 입력, 실제 사거리는 RANGE_SCALE 곱
    private const float RANGE_SCALE = 100f;    // 실제 판정 사거리 배율

    public float bulletRadius = 0.05f;         // 판정 구 반지름(지름 0.1)

    [Header("궤적(Tracer) 옵션")]
    public bool useTracer = true;              // Inspector에서 궤적 ON/OFF
    public GameObject tracerPrefab;            // LineRenderer 프리팹 (없으면 Debug.DrawLine)

    [Header("머즐 플래시(총구 화염) 옵션")]
    public bool useMuzzleFlash = true;         // Inspector에서 ON/OFF
    public GameObject muzzleFlashPrefab;       // 머즐 플래시 프리팹
    public float muzzleFlashDuration = 0.06f;  // 총구화염 지속시간

    [Header("참조")]
    public Camera mainCam;                     // Main Camera 연결
    public Transform muzzlePoint;              // 총구 위치 (Gun 자식 MuzzlePoint 연결)

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

        // 발사(좌클릭)
        if (!isReloading && Input.GetButton("Fire1") && fireCooldown <= 0f && (isInfiniteAmmo || currentAmmo > 0))
            Fire();

        // 장전(R키)
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isInfiniteAmmo)
            StartCoroutine(Reload());
    }

    void Fire()
    {
        fireCooldown = 1f / fireRate;

        if(!isInfiniteAmmo)
            currentAmmo--;

        float realRange = range * RANGE_SCALE;

        // 카메라(에임) 중심에서 Raycast
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
                Debug.Log("총알이 맞은 물체: " + hit.collider.name);
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

            // 1. 궤적(Tracer) 기능
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

            // 2. 머즐 플래시(총구 화염) 기능
            if (useMuzzleFlash && muzzleFlashPrefab != null && muzzlePoint != null)
            {
                StartCoroutine(ShowMuzzleFlash());
            }
        }
    }

    System.Collections.IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("장전 중...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("장전 완료");
    }

    private System.Collections.IEnumerator ShowMuzzleFlash()
    {
        GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation, muzzlePoint);
        ParticleSystem ps = flash.GetComponent<ParticleSystem>();
        if (ps != null) ps.Play();
        yield return new WaitForSeconds(muzzleFlashDuration);
        Destroy(flash);
    }

    public void ResetGun()
    {
        StopAllCoroutines();
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}

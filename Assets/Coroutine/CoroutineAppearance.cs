using System.Collections;
using UnityEngine;

public class CoroutineAppearance : MonoBehaviour
{
    [Header("등장 이동 설정")]
    [SerializeField] private float riseHeight = 2f;      // 바닥 아래에서 올라올 거리
    [SerializeField] private float riseDuration = 1.2f;  // 올라오는 시간

    [Header("등장 사운드")]
    [SerializeField] private AudioClip appearanceSfx;

    [Header("공격 스크립트")]
    [SerializeField] private MonoBehaviour turretAttackScript;

    private AudioSource audioSource;

    private Vector3 targetPosition;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(AppearanceSequence());
    }

    private IEnumerator AppearanceSequence()
    {
        // 공격 중지
        if (turretAttackScript != null)
        {
            Debug.Log("공격 비활성화");
            turretAttackScript.enabled = false;
        }

        // 최종 위치 저장
        targetPosition = transform.position;

        // 시작 위치 (바닥 아래)
        Vector3 startPosition = targetPosition + Vector3.down * riseHeight;

        transform.position = startPosition;

        // 등장 사운드 재생
        if (audioSource != null && appearanceSfx != null)
        {
            Debug.Log("등장 사운드 재생");
            audioSource.PlayOneShot(appearanceSfx, 0.2f);
        }

        // 올라오기
        float timer = 0f;

        while (timer < riseDuration)
        {
            Debug.Log($"포탑 등장 중...");
            timer += Time.deltaTime;

            float t = timer / riseDuration;

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        // 위치 보정
        transform.position = targetPosition;

        // 살짝 대기
        yield return new WaitForSeconds(0.2f);

        // 공격 가능
        if (turretAttackScript != null)
        {
            Debug.Log("공격 활성화");
            turretAttackScript.enabled = true;
        }
    }
}
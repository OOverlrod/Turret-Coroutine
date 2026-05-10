using System.Collections;
using UnityEngine;

public class CoroutineTurretAttack : MonoBehaviour
{
    [Header("공격 연출 대상")]
    [SerializeField] private Transform barrel;

    [Header("반동 설정")]
    [SerializeField] private float recoilDistance = 0.2f;
    [SerializeField] private float recoilTime = 0.08f;
    [SerializeField] private float returnTime = 0.12f;

    [Header("사운드")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSfx;
    [SerializeField] private float fireVolume = 0.4f;

    private Vector3 barrelOriginalLocalPos;
    private bool isAttacking;

    private void Awake()
    {
        if (barrel != null)
            barrelOriginalLocalPos = barrel.localPosition;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackRecoil()
    {
        if (isAttacking) return;

        StartCoroutine(AttackRecoilSequence());
    }

    private IEnumerator AttackRecoilSequence()
    {
        isAttacking = true;

        // 1. 총열 뒤로 밀림
        Vector3 recoilPos = barrelOriginalLocalPos + Vector3.back * recoilDistance;

        float timer = 0f;

        while (timer < recoilTime)
        {
            timer += Time.deltaTime;
            float t = timer / recoilTime;

            barrel.localPosition = Vector3.Lerp(barrelOriginalLocalPos, recoilPos, t);

            yield return null;
        }

        // 2. 발사 사운드
        if (audioSource != null && fireSfx != null)
            audioSource.PlayOneShot(fireSfx, fireVolume);

        // 3. 총열 원위치
        timer = 0f;
        while (timer < returnTime)
        {
            timer += Time.deltaTime;
            float t = timer / returnTime;

            barrel.localPosition = Vector3.Lerp(recoilPos, barrelOriginalLocalPos, t);
            yield return null;
        }

        barrel.localPosition = barrelOriginalLocalPos;

        isAttacking = false;
    }
}
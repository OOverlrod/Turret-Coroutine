using System.Collections;
using UnityEngine;

namespace TurretDemo
{
    public class CoroutineEnemyDeath : MonoBehaviour
    {
        [Header("사망 이동 연출")]
        [SerializeField] private float moveUpDistance = 2f;
        [SerializeField] private float moveUpDuration = 0.8f;

        [Header("죽을 때 끌 컴포넌트")]
        [SerializeField] private MonoBehaviour enemyMoveScript;
        [SerializeField] private Collider enemyCollider;

        private bool isDead;

        public void PlayDeathSequence(EnemySpawner enemySpawner)
        {
            if (isDead) return;

            StartCoroutine(DeathSequence(enemySpawner));
        }

        private IEnumerator DeathSequence(EnemySpawner enemySpawner)
        {
            isDead = true;

            // 이동 스크립트 비활성화
            if (enemyMoveScript != null)
                enemyMoveScript.enabled = false;

            // 콜라이더 비활성화
            if (enemyCollider != null)
                enemyCollider.enabled = false;


            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + Vector3.up * moveUpDistance;

            // 점점 상승하는 효과
            float timer = 0f;

            while (timer < moveUpDuration)
            {
                timer += Time.deltaTime;

                float t = timer / moveUpDuration;

                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                yield return null;
            }

            transform.position = targetPosition;

            // 완전히 상승하는 효과 후 오브젝트 비활성화
            enemySpawner.ReturnToPool(gameObject);
        }

        private void OnEnable()
        {
            isDead = false;

            if (enemyMoveScript != null)
                enemyMoveScript.enabled = true;

            if (enemyCollider != null)
                enemyCollider.enabled = true;
        }
    }
}
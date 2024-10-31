using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granede_Ctrl : MonoBehaviour
{
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float explosionRadius = 5.0f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private int enemyLayer = 9;
    [SerializeField] private int destroyableLayer = 11;
    [SerializeField] private ParticleSystem Expeff;
    private Vector3 targetPosition;

    public void ThrowGrenade(GameObject grenade, Vector3 target)
    {

        Rigidbody rb = grenade.GetComponent<Rigidbody>();

        // 목표 지점까지 필요한 초기 속도 계산
        Vector3 velocity = CalculateThrowVelocity(target, throwPoint.position, 45f);
        rb.velocity = velocity;

        // 목표 위치 저장 및 거리 체크 시작
        targetPosition = target;
        StartCoroutine(CheckDistanceToTarget(grenade));
    }

    private IEnumerator CheckDistanceToTarget(GameObject grenade)
    {
        // 그레네이드가 목표 지점에 도달할 때까지 반복
        while (Vector3.Distance(grenade.transform.position, targetPosition) > 0.5f)
        {
            yield return null; // 매 프레임 대기
        }

        // 목표 지점 도달 시 폭발 발생
        Explode();
    }

    private void Explode()
    {
        // 폭발 효과 재생
        Expeff.Play();

        // 주변 오브젝트 탐색
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;

            if (obj.layer == enemyLayer)
            {
                var damageable = obj.GetComponent<OnDamege>();
                if (damageable != null)
                {
                    Vector3 hitPoint = obj.transform.position; // 대충 오브젝트 위치 사용
                    Vector3 hitNormal = (obj.transform.position - transform.position).normalized; // 폭발 중심부에서 유닛 방향
                    damageable.OnDamege(4, hitPoint, hitNormal);
                }
            }
            else if (obj.layer == destroyableLayer)
            {
                // 레이어 제거 후 폭발 효과로 날아가게 하기
                obj.layer = 0; // 기본 레이어로 설정
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }
        }

        // 폭발 후 그레네이드 제거
        Destroy(gameObject);
    }

    private Vector3 CalculateThrowVelocity(Vector3 target, Vector3 origin, float angle)
    {
        float gravity = Physics.gravity.y;
        float radianAngle = angle * Mathf.Deg2Rad;

        Vector3 direction = target - origin;
        float horizontalDistance = new Vector3(direction.x, 0, direction.z).magnitude;
        float verticalDistance = direction.y;

        float initialVelocity = Mathf.Sqrt(Mathf.Abs(gravity * horizontalDistance * horizontalDistance) /
                                           (2 * (horizontalDistance * Mathf.Tan(radianAngle) - verticalDistance)));

        Vector3 velocity = new Vector3(direction.x, 0, direction.z).normalized * initialVelocity * Mathf.Cos(radianAngle);
        velocity.y = initialVelocity * Mathf.Sin(radianAngle);

        return velocity;
    }
}




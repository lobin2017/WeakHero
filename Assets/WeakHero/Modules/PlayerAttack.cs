using UnityEngine;
using UnityEngine.InputSystem; 

public class PlayerAttack : MonoBehaviour
{
    [Header("공격 설정")]
    [SerializeField] private float attackDamage = 15f;
    [SerializeField] private float attackRadius = 2.5f; 
    [Range(0, 360)]
    [SerializeField] private float attackAngle = 120f;  

    [Header("레이어 설정")]
    [SerializeField] private LayerMask enemyLayer; 

    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        Debug.Log("플레이어 공격 발동!");

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Vector3 directionToEnemy = (enemyCollider.transform.position - transform.position).normalized;

            directionToEnemy.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;

            float dot = Vector3.Dot(forward, directionToEnemy);
            float cosAngle = Mathf.Cos((attackAngle * 0.5f) * Mathf.Deg2Rad);

            if (dot >= cosAngle)
            {
                if (enemyCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(attackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle * 0.5f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, attackAngle * 0.5f, 0) * transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * attackRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * attackRadius);
    }
}
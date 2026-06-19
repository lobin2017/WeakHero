using UnityEngine;

namespace Monster
{
    public enum MonsterState
    {
        Idle,
        Chase,
        Attack
    }
    public class MonsterSight : MonoBehaviour
    {

        [SerializeField] private Transform player;

        [Header("거리")]
        [SerializeField] private float sightRange = 8f;
        [SerializeField] private float chaseDistance = 7f;
        [SerializeField] private float attackDistance = 2f;

        [Header("시야")]
        [SerializeField] private float sightAngle = 45f;
        [SerializeField] private float rotationSpeed = 4f;

        private MonsterState currentState = MonsterState.Idle;

        private bool wasInSight;
        private bool isInSight;

        MonsterState previousState;

        public void UpdateSight()
        {
            Vector3 toPlayer = player.position - transform.position;

            float sqrDistance = toPlayer.sqrMagnitude;

            bool inSightRange = sqrDistance <= sightRange * sightRange;

            Vector3 dirToPlayer = toPlayer.normalized;
            float dot = Vector3.Dot(transform.forward, dirToPlayer);
            float sightThreshold = Mathf.Cos(sightAngle * Mathf.Deg2Rad);

            bool inSightAngle = dot > sightThreshold;

            isInSight = inSightRange && inSightAngle;

            if (!wasInSight && isInSight)
            {
                Debug.Log("플레이어 식별");
            }

            if (wasInSight && !isInSight)
            {
                Debug.Log("플레이어 놓침");
            }

            wasInSight = isInSight;

            if (isInSight)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(dirToPlayer, Vector3.up);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * 360f * Time.deltaTime
                );
            }

        }
        public MonsterState CurrentState => currentState;
        
        public void UpdateState()
        {
            Vector3 toPlayer = player.position - transform.position;

            float sqrDistance = toPlayer.sqrMagnitude;

            if (player == null)
            {
                currentState = MonsterState.Idle;
                return;
            }
            float attackSqr = attackDistance * attackDistance;
            float chaseSqr = chaseDistance * chaseDistance;

            if (sqrDistance <= attackSqr && isInSight)
            {
                currentState = MonsterState.Attack;
            }
            else if (sqrDistance <= chaseSqr && isInSight)
            {
                currentState = MonsterState.Chase;
            }
            else
            {
                currentState = MonsterState.Idle;
            }
            if (previousState != currentState)
            {
                Debug.Log($"{previousState} -> {currentState}");
                previousState = currentState;
            }

        }

        void Update()
        {
            UpdateSight();
            UpdateState();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.violet;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 8f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, sightRange);

            if (player != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(transform.position, player.position);
            }
        }
    }
}
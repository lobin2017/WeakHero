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
        [SerializeField] public Transform player;

        [Header("거리")]
        [SerializeField] private float chaseDistance = 12f;
        [SerializeField] private float attackDistance = 2.8f;

        private MonsterState currentState = MonsterState.Idle;
        private MonsterState previousState;

        private void Update()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (player == null)
            {
                currentState = MonsterState.Idle;
                return;
            }

            Vector3 toPlayer = player.position - transform.position;
            float sqrDistance = toPlayer.sqrMagnitude;

            Vector3 directionToPlayer = new Vector3(toPlayer.x, 0, toPlayer.z).normalized;

            bool inAttackRange = sqrDistance <= attackDistance * attackDistance;
            bool inChaseRange = sqrDistance <= chaseDistance * chaseDistance;

            if (inAttackRange)
                currentState = MonsterState.Attack;
            else if (inChaseRange)
                currentState = MonsterState.Chase;
            else
                currentState = MonsterState.Idle;

            if (previousState != currentState)
            {
                // Debug.Log($"{gameObject.name}: {previousState} -> {currentState}");
                previousState = currentState;
            }
        }

        public MonsterState CurrentState => currentState;
    }
}
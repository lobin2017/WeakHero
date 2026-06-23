using Monster;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] private float moveSpeed = 3.5f;

    private MonsterSight monsterSight;
    private Animator animator;

    private void Awake()
    {
        monsterSight = GetComponent<MonsterSight>();
        animator = GetComponent<Animator>();
    }

    public void OnChase()
    {
        if (monsterSight.CurrentState != MonsterState.Chase || player == null)
        {
            if (animator != null) animator.SetBool("isMoving", false);
            return;
        }

        Vector3 direction = player.position - transform.position;
        Vector3 moveDir = new Vector3(direction.x, 0, direction.z).normalized;

        if (moveDir.magnitude > 0.1f)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(90f, targetAngle, 0f);
        }

        if (animator != null)
            animator.SetBool("isMoving", true);
    }

    void Update()
    {
        OnChase();
    }
}
using UnityEngine;

namespace Monster
{
    public class MonsterMovement : MonoBehaviour
    {
        [SerializeField] public Transform player;
        [SerializeField] private float moveSpeed = 3.5f;

        private MonsterSight monsterSight;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            monsterSight = GetComponent<MonsterSight>();
            animator = GetComponent<Animator>();

            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }

        public void OnChase()
        {
            if (monsterSight.CurrentState != MonsterState.Chase || player == null)
            {
                if (animator != null)
                    animator.SetBool("isMoving", false);

                return;
            }

            Vector3 direction = player.position - transform.position;

            Vector3 moveDir = new Vector3(
                direction.x,
                0,
                direction.z
            ).normalized;

            transform.Translate(
                moveDir * moveSpeed * Time.deltaTime,
                Space.World
            );

            if (spriteRenderer != null)
            {
                if (moveDir.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else if (moveDir.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
            }

            if (animator != null)
            {
                animator.SetBool("isMoving", true);
            }
        }

        private void Update()
        {
            OnChase();
        }
    }
}
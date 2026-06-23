using Monster;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private MonsterManager monsterManager;

    [SerializeField] private float weaponDamage = 5f;
    [SerializeField] private float attackDistance = 3f;
    [SerializeField] private float weaponDelay = 1f;
    [SerializeField] private float attackAngle = 70f;

    private float lastWeaponAttackTime;
    private Vector3 aimDirection;
    private PlayerInputActions inputActions;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
    }
    private void OnDisable()
    {
        inputActions.Player.Disable();
    }
    private void HandleAiming()
    {
        Vector2 mouseScreenPos = inputActions.Player.Aim.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimDirection = hit.point - transform.position;
            aimDirection.y = 0;

            aimDirection.Normalize();
        }
        if (aimDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (aimDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void HandleAttackInput()
    {
        if (inputActions.Player.Attack.IsPressed())
        {
            if (Time.time - lastWeaponAttackTime >= weaponDelay)
            {
                PerformAttack();
                lastWeaponAttackTime = Time.time;
            }
        }
    }

    public void PerformAttack()
    {
        if (monsterManager == null || monsterManager.activeMonsters.Count == 0)
        {
            
            return;
        }
        for (int i = 0; i < monsterManager.activeMonsters.Count; i++)
        {
            MonsterHealth monster = monsterManager.activeMonsters[i];

            if (monster == null)
                continue;

            Vector3 directionToMonster = monster.transform.position - transform.position;
            float distanceSqr = directionToMonster.sqrMagnitude;
            float attackRangeSqr = attackDistance * attackDistance;

            if (distanceSqr > attackRangeSqr)
            {
                continue;
            }

            directionToMonster.Normalize();
            float dot = Vector3.Dot(aimDirection, directionToMonster);
            float cosAngle = Mathf.Cos(attackAngle * Mathf.Deg2Rad);

            if (dot > cosAngle)
            {
                monster.TakeDamage(weaponDamage);
                Debug.Log($"{monster.name} 에게 {weaponDamage} 데미지");
            }
        }
    }
    void Update()
    {
        HandleAiming();
        HandleAttackInput();
    }
    
}
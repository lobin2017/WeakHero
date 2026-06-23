using UnityEngine;

namespace Monster
{
    public class MonsterInitializer : MonoBehaviour
    {
        public Transform playerTransform;

        private void Start()
        {
            SetupAllComponents();
            Destroy(this, 0.1f);
        }

        private void SetupAllComponents()
        {
            var sight = GetComponent<MonsterSight>();
            var movement = GetComponent<MonsterMovement>();
            var attack = GetComponent<MonsterAttack>();

            if (sight != null) sight.player = playerTransform;
            if (movement != null) movement.player = playerTransform;
            if (attack != null) attack.Initialize(playerTransform);
        }
    }
}
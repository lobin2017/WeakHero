using Monster;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [Header("Monster Spawn Settings")]
    [SerializeField] private List<GameObject> monsterPrefabs;

    [Header("Random Spawn Area")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(20f, 20f);
    [SerializeField] private Vector3 spawnCenter = Vector3.zero;

    [Header("Initial Spawn")]
    [SerializeField] private int initialSpawnCount = 6;

    public List<MonsterHealth> activeMonsters = new List<MonsterHealth>();

    private Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
    }

    private void Start()
    {
        SpawnInitialMonsters();
    }

    private void SpawnInitialMonsters()
    {
        for (int i = 0; i < initialSpawnCount; i++)
        {
            SpawnRandomMonster();
        }
    }

    public void SpawnRandomMonster()
    {
        if (monsterPrefabs.Count == 0 || playerTransform == null) return;

        Vector3 randomPos = GetRandomSpawnPosition();

        Quaternion spawnRotation = Quaternion.Euler(90f, Random.Range(0, 360f), 0);

        GameObject monsterObj = Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Count)],
                                           randomPos, spawnRotation);

        monsterObj.transform.position += new Vector3(0, 0.6f, 0);

        SetupMonster(monsterObj);

        MonsterHealth health = monsterObj.GetComponent<MonsterHealth>();
        if (health != null)
        {
            activeMonsters.Add(health);
            health.OnDeath += () => RemoveMonster(health);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);

        return spawnCenter + new Vector3(randomX, 0, randomZ);
    }

    private void SetupMonster(GameObject monster)
    {
        if (monster == null) return;

        var sight = monster.GetComponent<MonsterSight>();
        var movement = monster.GetComponent<MonsterMovement>();
        var attack = monster.GetComponent<MonsterAttack>();

        if (sight != null) sight.player = playerTransform;
        if (movement != null) movement.player = playerTransform;
        if (attack != null) attack.Initialize(playerTransform);

        MonsterInitializer initializer = monster.AddComponent<MonsterInitializer>();
        initializer.playerTransform = playerTransform;
    }

    public void RemoveMonster(MonsterHealth monster)
    {
        if (monster != null && activeMonsters.Contains(monster))
            activeMonsters.Remove(monster);
    }

    void Update()
    {
        for (int i = activeMonsters.Count - 1; i >= 0; i--)
        {
            if (activeMonsters[i] == null)
                activeMonsters.RemoveAt(i);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnCenter, new Vector3(spawnAreaSize.x, 1f, spawnAreaSize.y));
    }
}
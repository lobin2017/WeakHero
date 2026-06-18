using Monster;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [Header("Monster Settings")]
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    public List<MonsterHealth> activeMonsters = new List<MonsterHealth>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public MonsterHealth SpawnMonster(int prefabIndex, Vector3 position)
    {
        if (prefabIndex < 0 || prefabIndex >= monsterPrefabs.Count)
        {
            return null;
        }

        GameObject monsterObj = Instantiate(monsterPrefabs[prefabIndex], position, Quaternion.identity);
        MonsterHealth health = monsterObj.GetComponent<MonsterHealth>();

        if (health != null)
        {
            activeMonsters.Add(health);

            health.OnDeath += () => RemoveMonster(health);
        }
        return health;
    }

    public void RemoveMonster(MonsterHealth monster)
    {
        if (monster != null && activeMonsters.Contains(monster))
        {
            activeMonsters.Remove(monster);
        }
    }

    void Update()
    {
        if (activeMonsters.Count <= 0 && spawnPoints.Length > 0)
        {
            int randomPrefab = Random.Range(0, monsterPrefabs.Count);
            int randomPoint = Random.Range(0, spawnPoints.Length);

            SpawnMonster(randomPrefab, spawnPoints[randomPoint].position);
        }

        for (int i = activeMonsters.Count - 1; i >= 0; i--)
        {
            if (activeMonsters[i] == null)
            {
                activeMonsters.RemoveAt(i);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Stats", fileName = "SO_NewEnemyStats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private int armor = 0;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float spawnChance = 1f;

    private float runtimeMaxHealth;
    private int runTimeArmor;
    private float runTimeMoveSpeed;
    private float runTimeSpawnChance;

    public float MaxHealth { get { return runtimeMaxHealth; } set { runtimeMaxHealth = value; } }
    public int Armor { get { return runTimeArmor; } set { runTimeArmor = value; } }
    public float MoveSpeed { get { return runTimeMoveSpeed; } set { runTimeMoveSpeed = value; } }
    public float SpawnChance { get { return runTimeSpawnChance; } set { runTimeSpawnChance = value; } }

    private void OnEnable()
    {
        runtimeMaxHealth = maxHealth;
        runTimeArmor = armor;
        runTimeMoveSpeed = moveSpeed;
        runTimeSpawnChance = spawnChance;
    }

    public void ResetStats()
    {
        runtimeMaxHealth = maxHealth;
        runTimeArmor = armor;
        runTimeMoveSpeed = moveSpeed;
        runTimeSpawnChance = spawnChance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stats/Player Stats", fileName = "SO_PlayerStats")]
public class PlayerStats : Stats
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float armor = 0;
    [SerializeField] private float moveSpeed = 200f;
    [SerializeField] private float critChance = .15f;
    [SerializeField] private float critMulitplier = 2;

    private float runTimeHealth;
    private float runTimeArmor;
    private float runTimeMoveSpeed;
    private float runTimeCritChance;
    private float runTimeCritMulitplier;

    public float Health { get { return runTimeHealth; } set { runTimeHealth = value; } }
    public float Armor { get { return runTimeArmor; } set { runTimeArmor = value; } }
    public float MoveSpeed { get { return runTimeMoveSpeed; } set { runTimeMoveSpeed = value; } }
    public float CritChance { get { return runTimeCritChance; } set { runTimeCritChance = value; } }
    public float CritMulitplier { get { return runTimeCritMulitplier; } set { runTimeCritMulitplier = value; } }


    private void OnEnable()
    {
        runTimeHealth = health;
        runTimeArmor = armor;
        runTimeMoveSpeed = moveSpeed;
        runTimeCritChance = critChance;
        runTimeCritMulitplier = critMulitplier;
    }
}

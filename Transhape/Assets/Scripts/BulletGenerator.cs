using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    [Header("Prefab:")]
    
    [SerializeField] private GameObject squareAttackPrefab;
    [SerializeField] private GameObject circleAttackPrefab;

    void Update()
    {
    }
    public void AttackSquare()
    {
        Vector3 spawnRightPos = new Vector3 (transform.position.x + 1, transform.position.y, 0);
        Vector3 spawnLeftPos = new Vector3(transform.position.x - 1, transform.position.y, 0);
        Instantiate(squareAttackPrefab, spawnRightPos, Quaternion.Euler(0, 0, -90));
        Instantiate(squareAttackPrefab, spawnLeftPos, Quaternion.Euler(0, 0, 90));
    }
    public void AttackCircle()
    {
        Instantiate(circleAttackPrefab, transform.position, Quaternion.identity);
    }
}

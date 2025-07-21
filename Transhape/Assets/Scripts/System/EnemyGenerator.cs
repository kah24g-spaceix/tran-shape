using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemySquarePrefab;
    [SerializeField] private GameObject enemyCirclePrefab;
    [SerializeField] private int maxSpwanEnemy;
    [SerializeField] private int summonedEnemy;
    public int deadEnemy;
    [SerializeField] private int round;
    Vector2 spawnPos = new Vector2(15, -3.7f);
    private void Awake()
    {
        maxSpwanEnemy = 10;
        summonedEnemy = 0;
        deadEnemy = 0;
        round = 0;
        StartCoroutine(SpawnEnemy());
    }
    void Update()
    {
        if (deadEnemy == maxSpwanEnemy)
        {
            round++;
            summonedEnemy = 0;
            maxSpwanEnemy *= round;
            StartCoroutine(SpawnEnemy());
        }
    }
    private IEnumerator SpawnEnemy()
    {
        while (summonedEnemy < maxSpwanEnemy)
        {
            int direction = Random.Range(0, 2);
            int enemy = Random.Range(0, 2);
            switch (enemy)
            {
                case 0:
                    if (direction == 0)
                        Instantiate(enemySquarePrefab, new Vector3(player.transform.position.x + spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                    else if (direction == 1)
                        Instantiate(enemySquarePrefab, new Vector3(player.transform.position.x - spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                    break;
                case 1:
                    if (direction == 0)
                        Instantiate(enemyCirclePrefab, new Vector3(player.transform.position.x + spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                    else if (direction == 1)
                        Instantiate(enemyCirclePrefab, new Vector3(player.transform.position.x - spawnPos.x, spawnPos.y, 0), Quaternion.identity);
                    break;
            }
            summonedEnemy++;
            yield return new WaitForSeconds((Random.Range(4.0f, 7.0f)));
        }
    }
}

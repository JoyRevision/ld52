using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{

    List<Transform> spawnPoints;

    float timer = 0f;
    float lastSpawnSec = 0f;
    float timeToSpawnSec = 2f;

    public GameObject enemyToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Transform>();
        spawnPoints.Add(transform.Find("TopLeft"));
        spawnPoints.Add(transform.Find("TopRight"));
        spawnPoints.Add(transform.Find("BottomLeft"));
        spawnPoints.Add(transform.Find("BottomRight"));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        lastSpawnSec += Time.deltaTime;

        if (lastSpawnSec >= timeToSpawnSec)
        {
            var randIdx = Random.Range(0, spawnPoints.Count);
            Vector3 spawnPos = spawnPoints[randIdx].transform.position;

            Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

            lastSpawnSec = 0f;

            // Difficulty. After 60sec it's hard, 30s medium, otherwise pretty easy
            if (timer > 60f)
            {
                timeToSpawnSec = Random.Range(0.25f, 1f);
            }
            else if (timer > 30f)
            {
                timeToSpawnSec = Random.Range(0.5f, 2f);
            }
            else
            {
                timeToSpawnSec = Random.Range(0.5f, 3.5f);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // Ignore collisions with enemies and projectiles
        var tag = other.gameObject.tag;
        if (tag == "Enemy" || tag == "Projectile")
        {
            Physics.IgnoreCollision(other.collider, GetComponent<BoxCollider>());
        }
    }
}

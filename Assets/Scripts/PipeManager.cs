using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [SerializeField] private GameObject pipes;
    [SerializeField] private float spawnRate = 2f;
    [SerializeField] private float spawnX = 9f;
    [SerializeField] private float minY = -2.1f, maxY = 2.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(SpawnPipes), 0f, spawnRate);
    }

    private void SpawnPipes()
    {
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnX, randomY, 0);
        Instantiate(pipes, spawnPosition, Quaternion.identity);
    }

    void Update()
    {
        if (!UIManager.Instance.GameStarted) return;
        GameObject[] allPipes = GameObject.FindGameObjectsWithTag("Pipes");
        foreach (GameObject pipe in allPipes)
        {
            if (pipe.transform.position.x < -10f)
            {
                Destroy(pipe);
            }
        }
    }


    public void resetPipes() {
        CancelInvoke(nameof(SpawnPipes));
        GameObject[] allPipes = GameObject.FindGameObjectsWithTag("Pipes");
        foreach (GameObject pipe in allPipes) Destroy(pipe);
        InvokeRepeating(nameof(SpawnPipes), 0f, spawnRate);
    }
}

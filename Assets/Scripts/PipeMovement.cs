using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    [SerializeField] float pipeSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.Instance.GameStarted) return;
        if (gameObject.scene.rootCount == 0) // Means this is the prefab, not an instance
        {
            Debug.LogError("You're modifying the original prefab!");
            return;
        }
        if(transform != null){
            transform.Translate(Vector2.left * pipeSpeed * Time.deltaTime);
        }
        else{
            Debug.Log("transform is null!");
        }
    }
}

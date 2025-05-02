using UnityEngine;
using UnityEngine.UI;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.02f;

    private float width;

    private void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("Sprite width: " + width);
    }

    private void Update()
    {
        transform.position += Vector3.left * scrollSpeed * Time.unscaledDeltaTime;

        if (transform.position.x < -width)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}
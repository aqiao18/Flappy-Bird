using UnityEngine;
using UnityEngine.UI;

public class BackgroundLooper : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.02f;
    public static BackgroundLooper Instance { get; private set;}
    public static bool pauseScrolling = false;

    private float width;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log("Sprite width: " + width);
    }

    private void Update()
    {
        print(pauseScrolling);
        if (UIManager.Instance == null || pauseScrolling) return;

        transform.position += Vector3.left * scrollSpeed * Time.unscaledDeltaTime;

        if (transform.position.x < -width)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}
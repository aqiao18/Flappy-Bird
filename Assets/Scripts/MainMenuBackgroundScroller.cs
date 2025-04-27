using UnityEngine;
using UnityEngine.UI;

public class BackgroundMover : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.02f;
    private RawImage rawImage;
    private Vector2 uvOffset = Vector2.zero;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        uvOffset.x += scrollSpeed * Time.unscaledDeltaTime;
        rawImage.uvRect = new Rect(uvOffset, rawImage.uvRect.size);
    }
}
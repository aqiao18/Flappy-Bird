using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool scored = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!scored && other.CompareTag("Player"))
        {
            SoundManager.Instance.PlayScoreSound();
            ScoreManager.Instance.AddScore(1);
            scored = true;
        }
    }
}
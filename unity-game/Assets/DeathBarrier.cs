using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CatBox"))
        {
            GameLogicCode.Instance?.BoxMissed();

            Destroy(other.gameObject);
        }
    }
}

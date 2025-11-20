using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 randomDirection;

    private void Start()
    {
        PickRandomDirection();
    }

    private void Update()
    {
        transform.Translate(randomDirection * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // bounce in a new direction if the cat hits a wall or another cat
        PickRandomDirection();
    }

    void PickRandomDirection()
    {
        randomDirection = Random.insideUnitCircle.normalized;
    }
}

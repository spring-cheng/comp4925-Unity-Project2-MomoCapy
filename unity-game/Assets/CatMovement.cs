using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] float moveSpeed = 5f;

    [Header("Collision Effects")]
    public GameObject poofPrefab;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;

        transform.Translate(movement, Space.World);

        bool isWalking = (horizontal != 0 || vertical != 0);
        animator.SetBool("isWalking", isWalking);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CatBox"))
        {
            // spawn poof effect
            if (poofPrefab != null)
            {
                GameObject poof = Instantiate(poofPrefab, other.transform.position, Quaternion.identity);
                Destroy(poof, 0.2f); // destroy after 0.2 second
            }

            // notify GameManager
            if (GameLogicCode.Instance != null)
            {
                GameLogicCode.Instance.CatSaved();
            }

            // destroy the box
            Destroy(other.gameObject);
        }
    }
}

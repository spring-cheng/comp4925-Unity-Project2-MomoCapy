using UnityEngine;

public class CatMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;

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
}

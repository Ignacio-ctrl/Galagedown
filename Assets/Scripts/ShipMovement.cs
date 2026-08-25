using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float Speed;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject Panel;
    [SerializeField] private Collider2D mapBoundsCollider;

    Rigidbody2D rb;
    Animator anim;

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private Vector2 spriteExtents;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

       
    }

    void Update()
    {
        bool ShotInput = Input.GetButtonDown("Fire1");
        if (ShotInput)
        {
            Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }
    }

    private void FixedUpdate()
    {
        //Movement//
        //Move w Keys//
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 direccion = new Vector2(horizontalInput, verticalInput);

        rb.MovePosition(rb.position + direccion * Speed * Time.fixedDeltaTime);

        //Aim w Mouse//
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 mDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = mDirection;

        //animation//
        if (verticalInput != 0 || horizontalInput != 0)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }
    }



    public void pause()
    {
        Time.timeScale = 0f;
        Panel.SetActive(true);
    }
}
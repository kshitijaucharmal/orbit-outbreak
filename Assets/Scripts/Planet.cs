using UnityEngine;

public class Planet : MonoBehaviour {
    
    [HideInInspector] public bool stuck = false;
    
    [HideInInspector] public float detectionRange;
    // The gravitational constant in N m^2 / kg^2
    [SerializeField] private float gravityMultiplier = 25f;
    [SerializeField] private float rotationMultiplier = 100;

    private float gravityConstant = 6.674f;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public float mass;
    
    private float rotationSpeed;

    [HideInInspector] public Vector2 releaseDir;
    private PlanetSpawner ps;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        ps = FindObjectOfType<PlanetSpawner>();
    }

    public void SetSize(int size){
        rb = GetComponent<Rigidbody2D>();

        // Set Scale
        transform.localScale = Vector2.one * size;

        // Set mass according to size
        rb.mass = size;
        mass = rb.mass;

        // Set Detection range
        detectionRange = transform.localScale.x * 2f; // or y
        
        // Set Rotation speed
        rotationSpeed = transform.localScale.x * rotationMultiplier;
        
        // Rotate
        var direction = Random.Range(0, 1) < 0.5 ? -1 : 1;
        rb.AddTorque(rotationSpeed * direction * Time.fixedDeltaTime, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        if(stuck || rb == null) return;

        // Calculate the force of gravity between this planet and all other planets in the scene
        foreach (Planet otherPlanet in ps.planets) {
            // Skip if not in range
            if(Vector2.Distance(transform.position, otherPlanet.transform.position) > (detectionRange + otherPlanet.detectionRange)) continue;
            if (otherPlanet != this && otherPlanet.enabled == true){
                // Calculate the vector from this planet to the other planet
                Vector2 direction = otherPlanet.transform.position - transform.position;

                // Calculate the distance between the two planets
                float distance = direction.magnitude;

                // Calculate the force of gravity between the two planets
                float force = gravityConstant * gravityMultiplier * mass * otherPlanet.mass / (distance * distance);

                // Apply the force to this planet
                rb.AddForce(direction.normalized * force);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(stuck || transform.CompareTag("P1Planet") || transform.CompareTag("P2Planet")) return;
        
        // var tag = gameObject.tag;

        // //if(stuck || transform.CompareTag("P1Planet") || transform.CompareTag("P2Planet")) return;

        // if(tag == "P1Player"){
        //     GameStatus gs = FindObjectOfType<GameStatus>();
        //     // P1 Dead
        //     if(collision.transform.CompareTag("P2Planet")){
        //         gs.UpdateStatus("P2 Wins!!");
        //         Destroy(gameObject);
        //     }

        //     // Game Over (Tie)
        //     if(collision.transform.CompareTag("P2Player")){
        //         gs.UpdateStatus("Both Dead.");
        //         Destroy(collision.gameObject);
        //         Destroy(gameObject);
        //     }
        // }
        // if(tag == "P2Player"){
        //     GameStatus gs = FindObjectOfType<GameStatus>();
        //     // P2 Dead
        //     if(collision.transform.CompareTag("P1Planet")){
        //         gs.UpdateStatus("P1 Wins!!");
        //         Destroy(gameObject);
        //     }
        //     // Game Over (Tie)
        //     if(collision.transform.CompareTag("P1Player")){
        //         gs.UpdateStatus("Both Dead.");
        //         Destroy(collision.gameObject);
        //         Destroy(gameObject);
        //     }
        // }
        // Stick to any player
        if(collision.transform.CompareTag("P1Player") || collision.transform.CompareTag("P2Player")){
            releaseDir = (collision.collider.transform.position - transform.position).normalized;
            collision.transform.GetComponent<PlayerMovement>().StickToPlayer(this);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
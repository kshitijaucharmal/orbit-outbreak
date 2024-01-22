using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public enum Player{
        PLAYER1,
        PLAYER2
    }

    public Player player;

    private Rigidbody2D rb;

    [SerializeField] private Transform otherPlayer;

    [SerializeField] private float speed = 100f;
    [SerializeField] private float rotateSpeed = 1000f;
    [SerializeField] private float velocityLimit = 4f;
    [SerializeField] private Transform planetStore;
    [SerializeField] private float shootForce = 200;

    private Vector2 moveDir;

    private Planet thisPlanet;
    private string playerName;
    private string rivalName;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        thisPlanet = GetComponent<Planet>();
        playerName = player == Player.PLAYER1 ? "P1" : "P2";
        rivalName = player == Player.PLAYER1 ? "P2" : "P1";
    }

    public void ReleaseLastPlanet(Transform rival){
        var lastPlanet = planetStore.GetChild(planetStore.childCount - 1).GetComponent<Planet>();
        Rigidbody2D planetRb = lastPlanet.transform.AddComponent<Rigidbody2D>();
        lastPlanet.stuck = false;
        lastPlanet.transform.parent = null;
        rb.mass -= lastPlanet.mass * 0.1f;
        thisPlanet.detectionRange -= lastPlanet.detectionRange * 0.5f;

        planetRb.mass = lastPlanet.mass;
        var dir = -(transform.position - rival.position).normalized;
        planetRb.AddForce(dir * shootForce, ForceMode2D.Impulse);
    }

    public void StickToPlayer(Planet planet){
        planet.stuck = true;
        rb.mass += planet.mass * 0.1f;
        thisPlanet.detectionRange += planet.detectionRange * 0.5f;
        
        Rigidbody2D planetRb = planet.GetComponent<Rigidbody2D>();
        planetRb.velocity = Vector2.zero;
        rb.angularDrag += planetRb.angularDrag;
        planetRb.freezeRotation = true;
        planet.transform.parent = planetStore;

        planet.gameObject.tag = playerName + "Planet";

        Destroy(planetRb);
        // Destroy(planet.GetComponent<Planet>());
        // planet.GetComponent<Planet>().enabled = false;
    }

    // Update is called once per frame
    void Update() {
        moveDir.x = Input.GetAxisRaw(playerName + "Horizontal");
        moveDir.y = Input.GetAxisRaw(playerName + "Vertical");
        moveDir.Normalize();

        var rotateDir = Input.GetAxisRaw(playerName + "Rotation");
        transform.Rotate(Vector3.forward * rotateDir * (1 / rb.mass * 10) * rotateSpeed * Time.deltaTime);

        if(Input.GetButtonDown(playerName + "Shoot")){
            ReleaseLastPlanet(otherPlayer);
        }
    }

    void FixedUpdate(){
        if (moveDir.magnitude >= 0.1){
            rb.AddForce(moveDir * speed * Time.deltaTime, ForceMode2D.Force);
        }
        rb.velocity = rb.velocity.sqrMagnitude < velocityLimit * velocityLimit ? rb.velocity : rb.velocity.normalized * velocityLimit;
    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.transform.CompareTag(rivalName + "Planet")){
            GameStatus gs = FindAnyObjectByType<GameStatus>();
            gs.UpdateStatus(rivalName + " Won.");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}

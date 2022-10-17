using System . Collections ;
using System . Collections . Generic ;
using UnityEngine ;
using UnityEngine . InputSystem ;
using TMPro;

public class PlayerController : MonoBehaviour {

    public Vector2 moveValue ;
    public float speed ;
    private int count;
    private int numPickups = 3;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI playerpos;
    public TextMeshProUGUI playervel;
    private Vector3 lastpos;
    public float velocity;

    void Start() {
        count = 0;
        winText.text = "";
        SetCountText();
        playerpos.text = "";
        playervel.text = "";
        velocity = 0;
        lastpos = transform.position;
    }

    void OnMove ( InputValue value ) {
        moveValue = value .Get < Vector2 >();
    }

    void FixedUpdate () {
        Vector3 movement = new Vector3 ( moveValue .x, 0.0f, moveValue .y);

        GetComponent < Rigidbody >() . AddForce ( movement * speed * Time.fixedDeltaTime );

        velocity = ((transform.position - lastpos).magnitude / Time.deltaTime);
        lastpos = transform.position;

        playerpos.text = "Position: "+lastpos;
        playervel.text = "Velocity: "+velocity;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "PickUp") {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    private void SetCountText() {
        scoreText.text = "Score: " + count.ToString();
        if (count >= numPickups) {
            winText.text = "You Win!";
        }
    }
}
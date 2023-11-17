using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject playButton;

    public float speed;
    private Rigidbody rig;
    private float startTime;
    private float timeTaken;
    public float turnSpeed = 5f;

    private int collectablesPicked = 0;
    public int maxCollectables = 10;
    private bool isPlaying;
    public TextMeshProUGUI curTimeText;
    private bool isPlayed = false;
    public TextMeshProUGUI buttonText;
    public LeaderBoard leaderBoard;

    private Vector3 playerInput;
    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
    }
    void Update()
    {
        if (!isPlaying)
        {
            transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
            transform.Rotate(Vector3.forward, turnSpeed / 2 * Time.deltaTime);
            return;
        }
          
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;
        playerInput = new Vector3(x, rig.velocity.y, z);

        curTimeText.text = (Time.time - startTime).ToString("F2");

    }

    private void FixedUpdate()
    {
        rig.AddForce(playerInput * speed);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectable"))
        {
            collectablesPicked++;
            Destroy(other.gameObject);
            if (collectablesPicked == maxCollectables)
                End();
        }
    }
    public void Begin()
    {
        rig.useGravity = true;
        if (isPlayed)
            SceneManager.LoadScene("Game1");
        startTime = Time.time;
        isPlaying = true;
        playButton.SetActive(false);
    }
    void End()
    {
        timeTaken = Time.time - startTime;
        isPlaying = false;
        leaderBoard.SetLeaderboardEntry(-Mathf.RoundToInt(timeTaken * 1000.0f));
        playButton.SetActive(true);
        buttonText.text = "Replay";
        isPlayed = true;
        Invoke("UpdateLeaderBoard", 1.5f);
    }

    private void UpdateLeaderBoard()
    {
        leaderBoard.DisplayLeaderboard();
    }
}

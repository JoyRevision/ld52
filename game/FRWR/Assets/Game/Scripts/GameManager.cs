using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;
    int carrotsKilled = 0;

    public GameObject healthBar;
    Slider healthSlider;

    public GameObject deathScreen;
    public GameObject score;
    private TextMeshProUGUI scoreText;

    bool playerDied = false;
    bool finished = false;

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.2f);

        // TODO: Set score text
        deathScreen.SetActive(true);
        scoreText.text = $"Carrots Harvested: {carrotsKilled}";

        finished = true;
    }

    public void CarrotDied()
    {
        carrotsKilled += 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        healthSlider = healthBar.GetComponent<Slider>();

        scoreText = score.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float health = player.GetHealth();
        healthSlider.value = health;

        if (health <= 0)
        {
            if (!playerDied)
            {
                StartCoroutine(EndGame());
            }
            playerDied = true;
        }

        if (finished && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

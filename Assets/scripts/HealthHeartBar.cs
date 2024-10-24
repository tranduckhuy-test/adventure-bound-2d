using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    PlayerHealth playerHealth;
    List<HealthHeart> hearts = new List<HealthHeart>();

    [SerializeField] float playerMaxHealth = 12f;

    private void OnEnable()
    {
        GameManager.OnPlayerSpawned += OnPlayerSpawned;
        PlayerHealth.OnhealthUpdate += DrawHearts;
    }

    private void OnDisable()
    {
        GameManager.OnPlayerSpawned -= OnPlayerSpawned;
        PlayerHealth.OnhealthUpdate -= DrawHearts;
    }

    private void OnPlayerSpawned(GameObject player)
    {
        // playerHealth is on the hurtbox
        playerHealth = player.GetComponentInChildren<PlayerHealth>();
        DrawHearts();
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, transform);

        // Create heart object (game object and image handling)
        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HealthHeart.HeartStatus.Empty);
        // Add the created heart to the list for management
        hearts.Add(heartComponent);
    }

    public void DrawHearts()
    {
        // If health = 12, we need to draw 12/4 = 3 hearts
        ClearHearts();

        // Determine how many empty hearts to draw based on max health
        float maxHealthRemainder = playerMaxHealth % 4;
        int heartsToMake = (int)(playerMaxHealth / 4 + maxHealthRemainder);

        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

        // Update the sprite of each heart
        for (int i = 0; i < heartsToMake; i++)
        {
            // This calculation subtracts the current health by the equivalent health points of one heart, and then adds the clamp threshold
            int heartStatusRemainder = (int)Mathf.Clamp(playerHealth.Health - (i * 4), 0, 4);
            hearts[i].SetHeartImage((HealthHeart.HeartStatus)heartStatusRemainder);
        }

        // Check health and call PlayerDied if health <= 0
        if (playerHealth.Health <= 0)
        {
            GameManager.instance.PlayerDied();
        }
    }

    public void ClearHearts() // Clear all child objects under the health bar
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }

        hearts = new List<HealthHeart>();
    }
}

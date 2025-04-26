using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentPlayerHealth;

    [SerializeField] private Image healthFillImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentPlayerHealth -= amount;
        currentPlayerHealth = Mathf.Max(currentPlayerHealth, 0);
        UpdateHealthBar();
        if (currentPlayerHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentPlayerHealth += amount;
        currentPlayerHealth = Mathf.Min(currentPlayerHealth, maxHealth);
    }
    private void UpdateHealthBar()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentPlayerHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("u ded.");
    }
}

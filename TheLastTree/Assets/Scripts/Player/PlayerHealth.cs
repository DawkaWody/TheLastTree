using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentPlayerHealth;

    [SerializeField] private Image healthFillImage;

    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private float flashDuration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPlayerHealth = maxHealth;
        playerSprite.material = new Material(Shader.Find("Custom/2DFlash"));
    }

    public void TakeDamage(int amount)
    {
        currentPlayerHealth -= amount;
        currentPlayerHealth = Mathf.Max(currentPlayerHealth, 0);
        StartCoroutine(Flash());
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
    private IEnumerator Flash()
    {
        Material mat = playerSprite.material;
        mat.SetFloat("_FlashAmount", 1f);
        yield return new WaitForSeconds(flashDuration);
        mat.SetFloat("_FlashAmount", 0f);
    }
}

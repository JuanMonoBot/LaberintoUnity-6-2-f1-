using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI de vida")]
    [Tooltip("Asignamos los sprites de vidas")]
    public Image[] lifeImages; 

    public int CurrentHearts { get; private set; }

    private UI_Menu uiMenu;

    private void Start()
    {
        uiMenu = Object.FindFirstObjectByType<UI_Menu>();

        // Vida máxima = número de bloques
        CurrentHearts = lifeImages.Length;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount = 1)
    {
        CurrentHearts -= amount;
        if (CurrentHearts < 0)
            CurrentHearts = 0;

        UpdateHealthUI();

        // Cuando no quede ningún bloque, reiniciamos nivel
        if (CurrentHearts <= 0)
        {
            if (uiMenu != null)
                uiMenu.ReiniciarPlayer();

            // Dejamos la vida llena para la siguiente ronda
            ResetHealth();
        }
    }

    public void ResetHealth()
    {
        CurrentHearts = lifeImages.Length;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (lifeImages == null) return;

        for (int i = 0; i < lifeImages.Length; i++)
        {
            bool shouldBeVisible = i < CurrentHearts;

            if (lifeImages[i] != null)
            {
                // Apagar el componente Image
                lifeImages[i].enabled = shouldBeVisible;

                // Opción B (alternativa): apagar todo el GameObject
                // lifeImages[i].gameObject.SetActive(shouldBeVisible);
            }
        }
    }
}

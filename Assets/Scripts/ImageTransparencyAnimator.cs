using UnityEngine;
using UnityEngine.UI;

public class ImageTransparencyAnimator : MonoBehaviour
{
    public float animationSpeed = 1f; // Speed of the transparency animation
    public float targetAlpha = 0.5f; // Target alpha value when fully transparent

    private Image imageComponent;
    private bool isAnimating = false;
    private float currentAlpha;
    private float targetAlphaDirection = 1f;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
        currentAlpha = imageComponent.color.a;
    }

    private void Update()
    {
        if (isAnimating)
        {
            currentAlpha += Time.deltaTime * animationSpeed * targetAlphaDirection;
            currentAlpha = Mathf.Clamp01(currentAlpha);

            Color newColor = imageComponent.color;
            newColor.a = currentAlpha;
            imageComponent.color = newColor;

            // Check if reached target alpha, then reverse direction
            if (currentAlpha <= 0f || currentAlpha >= targetAlpha)
            {
                targetAlphaDirection *= -1f;
            }
        }
    }

    // Call this method to start the animation
    public void StartAnimation()
    {
        isAnimating = true;
    }

    // Call this method to stop the animation
    public void StopAnimation()
    {
        isAnimating = false;
    }
}
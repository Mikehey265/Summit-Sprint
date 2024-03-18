using UnityEngine;

public class ImageAnimation : MonoBehaviour
{
    public GameObject picture; // Assign your picture GameObject here in the inspector
    public float newYPosition = 5f; // The new Y position to animate to
    public float animationTime = 2f; // Duration of the animation in seconds

    void Start()
    {
        // Start the animation
        AnimatePicture();
    }

    private void AnimatePicture()
    {
        if (picture != null)
        {
            // LeanTween animation to change the Y position with easeOutBounce
            LeanTween.moveY(picture, newYPosition, animationTime)
                     .setEase(LeanTweenType.easeOutBounce);
        }
        else
        {
            Debug.LogError("Picture GameObject is not assigned!");
        }
    }
}

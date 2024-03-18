using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public float PosX;
    public float OutPosX;
    public float animationTime = 2.0f;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void AnimateIn()
    {
        Debug.Log("Animating In: " + gameObject.name);

        gameObject.SetActive(true);

        float targetPosX = rectTransform.localPosition.x + PosX;
        LeanTween.moveX(rectTransform, targetPosX, animationTime)
                 .setEase(LeanTweenType.easeInOutBack);
    }

    public void AnimateOut()
    {
        Debug.Log("Animating Out: " + gameObject.name);

        LeanTween.moveX(rectTransform, OutPosX, animationTime)
                 .setEase(LeanTweenType.easeInOutBack);

        Invoke("DeactivateGameObject", 2.2f);
    }

    private void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }
}

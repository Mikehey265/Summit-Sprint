using StaminaSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public ButtonAnimation playButton;
    public ButtonAnimation settingsButton;
    public ButtonAnimation settings2Button;
    public ButtonAnimation levelsButton;
    public GameObject middleButton;
    public GameObject leftSideButton;
    public GameObject rightSideButton;
    public float delayTime = 0.5f;

    private bool buttonClickable = true;

    private void Start()
    {
        playButton.AnimateIn();
        settingsButton.AnimateIn();
    }

    private void EnableButtonClick()
    {
        buttonClickable = true;
    }

    private void DisableButtonClick()
    {
        buttonClickable = false;
    }

    private void AnimateButtonOut(ButtonAnimation buttonAnimation)
    {
        if (buttonAnimation != null)
        {
            buttonAnimation.AnimateOut();
        }
    }

    private void AnimateButtonIn(ButtonAnimation buttonAnimation)
    {
        if (buttonAnimation != null)
        {
            buttonAnimation.AnimateIn();
        }
    }

    public void Play()
    {
        if (!buttonClickable) return;

        DisableButtonClick();
        AnimateButtonOut(playButton);
        Invoke(nameof(AnimateInLevelsButton), delayTime);
    }

    public void Settings()
    {
        if (!buttonClickable) return;

        DisableButtonClick();
        AnimateButtonOut(settingsButton);
        Invoke(nameof(AnimateInSettings2Button), delayTime);
    }

    public void Level_1()
    {
        SceneManager.LoadScene("Level_1 Long");
        AudioManager.Instance.PlayMusic("GameMusic");
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("LevelSelector");
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void Tut()
    {
        SceneManager.LoadScene("Level_0");
    }

    public void Middle()
    {
        if (!buttonClickable) return;

        if (playButton.gameObject.activeSelf && settings2Button.gameObject.activeSelf)
        {
            AnimateButtonOut(settings2Button);
            AnimateButtonIn(settingsButton);
            leftSideButton.SetActive(false);
        }

        if (levelsButton.gameObject.activeSelf && settingsButton.gameObject.activeSelf)
        {
            AnimateButtonOut(levelsButton);
            AnimateButtonIn(playButton);
            rightSideButton.SetActive(false);
        }

        if (levelsButton.gameObject.activeSelf && settings2Button.gameObject.activeSelf)
        {
            AnimateButtonOut(levelsButton);
            AnimateButtonOut(settings2Button);
            AnimateButtonIn(playButton);
            AnimateButtonIn(settingsButton);
            leftSideButton.SetActive(false);
            rightSideButton.SetActive(false);
        }

        if (playButton.gameObject.activeSelf && settingsButton.gameObject.activeSelf)
        {
            middleButton.gameObject.SetActive(false);
        }
    }


    public void LeftSide()
    {
        if (!buttonClickable) return;

        if (settings2Button.gameObject.activeSelf)
        {
            AnimateButtonOut(settings2Button);
            AnimateButtonIn(settingsButton); // Animate settingsButton in when LeftSide is clicked
            leftSideButton.SetActive(false);
            middleButton.SetActive(false);
        }

        if (!levelsButton.gameObject.activeSelf)
        {
            middleButton.SetActive(false);
            leftSideButton.SetActive(false);
        }
    }

    public void RightSide()
    {
        if (!buttonClickable) return;

        if (levelsButton.gameObject.activeSelf)
        {
            AnimateButtonOut(levelsButton);
            AnimateButtonIn(playButton); // Animate playButton in when rightSide is clicked
            rightSideButton.SetActive(false);
        }

        if (!settings2Button.gameObject.activeSelf)
        {
            middleButton.SetActive(false);
            rightSideButton.SetActive(false);
        }
    }

    private void AnimateInLevelsButton()
    {
        if (levelsButton == null)
        {
            Debug.LogError("levelsButton is null!");
        }
        else
        {
            AnimateButtonIn(levelsButton);
            Invoke(nameof(ActivateMiddleButton), 1.5f);
            Invoke(nameof(ActivateRightButton), 1.7f);
            Invoke(nameof(EnableButtonClick), 1.5f); // Re-enable button clicks after animation
        }
    }

    private void AnimateInSettings2Button()
    {
        if (settings2Button == null)
        {
            Debug.LogError("settings2Button is null!");
        }
        else
        {
            AnimateButtonIn(settings2Button);
            Invoke(nameof(ActivateMiddleButton), 1.5f);
            Invoke(nameof(ActivateLeftButton), 1.7f);
            Invoke(nameof(EnableButtonClick), 1.5f); // Re-enable button clicks after animation
        }
    }

    private void ActivateMiddleButton()
    {
        if (!middleButton.activeSelf)
        {
            middleButton.SetActive(true);
        }
    }

    private void ActivateLeftButton()
    {
        if (!leftSideButton.activeSelf)
        {
            leftSideButton.SetActive(true);
        }
    }

    private void ActivateRightButton()
    {
        if (!rightSideButton.activeSelf)
        {
            rightSideButton.SetActive(true);
        }
    }
    
    public void PlayUISound(string name)
    {
        AudioManager.Instance.PlayFX(name);
    }
}
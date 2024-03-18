using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFunction : MonoBehaviour
{
	public GameObject levelOneSign;
	public GameObject levelTwoSign;
	public GameObject levelThreeSign;

	public void LevelOne()
	{
		levelOneSign.SetActive(true);
		levelTwoSign.SetActive(false);
		levelThreeSign.SetActive(false);
	}

	public void LevelTwo()
	{
		levelOneSign.SetActive(false);
		levelTwoSign.SetActive(true);
		levelThreeSign.SetActive(false);
	}

	public void LevelThree()
	{
		levelOneSign.SetActive(false);
		levelTwoSign.SetActive(false);
		levelThreeSign.SetActive(true);
	}
}

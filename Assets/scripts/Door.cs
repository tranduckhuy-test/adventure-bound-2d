using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	float secToWait = 1;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponent<PlayerController>().enabled = false;
			collision.GetComponent<Animator>().SetBool("isWalking", false);
			SFXManager.instance.StairsDown();
			StartCoroutine("GoToNextLevel");
		}
	}

    //Wait a few seconds before loading the next level
    IEnumerator GoToNextLevel()
	{
		yield return new WaitForSeconds(secToWait);
		GameManager.instance.LoadNextScene();
	}
}

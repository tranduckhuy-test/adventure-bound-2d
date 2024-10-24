using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] GameObject stopMenu;
	[SerializeField] GameObject door;
	[SerializeField] GameObject roleMenu;
	[SerializeField] GameObject menuButton;
	[SerializeField] GameObject deadMenu;


	bool isGamePause = false;
	public bool IsGamePause
	{
		get { return isGamePause; }
	}

	private void Update()
	{
		if (GameManager.instance.IsGameStart && Input.GetKeyDown(KeyCode.Escape))
		{
			if (isGamePause)
            {
                Continue();
            }
            else
            {
                OpenStopMenu();
            }
		}
	}

	public void OpenStopMenu()
	{
		stopMenu.SetActive(true);
		deadMenu.SetActive(false);
		Time.timeScale = 0;
		isGamePause = true;

	}

	public void OpenRoleMenu()
	{
		menuButton.SetActive(false);
		roleMenu.SetActive(true);

	}

	public void CloseStopMenu()
	{
		stopMenu.SetActive(false);
	}

	public void UnhideDoor()
	{
		door.SetActive(true);
	}

	public void Continue()
	{
		SFXManager.instance.ClickSound();
		isGamePause = false;
		Time.timeScale = 1;
		CloseStopMenu();
	}

	public void SaveButton()
	{
		GameManager.instance.Save();
	}

	public void LoadButton()
	{
        GameManager.instance.Load();
	}

	public void RespawnButton()
    {
        Time.timeScale = 1;
		GameManager.instance.IsRespawn = true;
        GameManager.instance.Load();
    }

	public void BackToMenuButton()
	{
		GameManager.instance.BackToMenu();
	}

	public void ClickStartGameButton()
	{
		GameManager.instance.ClickStartGame();
	}

	public void QuitGameButton()
	{
		GameManager.instance.QuitGame();
	}

	public void SetCurrentRoleButton(string role)
	{
		LevelManager.instance.current_role = role;
		GameManager.instance.ClickStartGame();
	}

    public void ShowDeadMenu()
    {
        deadMenu.SetActive(true);
        stopMenu.SetActive(false);
        Time.timeScale = 0;
    }
}

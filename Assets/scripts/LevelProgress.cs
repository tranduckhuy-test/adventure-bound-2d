using UnityEngine;
using UnityEngine.UI;

public class LevelProgress : MonoBehaviour
{
    private Slider slider;

    [SerializeField] private GameObject killEnemyObj;
    [SerializeField] private GameObject findStairsObj;

    public float fillSpeed = 0.5f;
    private float targetProgress = 0;
    private int totalMonsterCount = 0;
    private int defeatedMonsterCount = 0;
    private bool isLevelComplete = false;

    public Transform monsterParent;

    private void OnEnable()
    {
        Monster.OnLevelProgressUpdate += IncrementProgress;
    }

    private void OnDisable()
    {
        Monster.OnLevelProgressUpdate -= IncrementProgress;
    }

    private void Awake()
    {
        slider = GetComponent<Slider>();
        CountMonsters();
    }

    void Update()
    {
        if (slider.value < targetProgress)
        {
            slider.value += fillSpeed * Time.deltaTime;
        }

        if (!isLevelComplete && slider.value >= 1f && Monster.isBossDefeated)
        {
            findStairsObj.SetActive(true);
            killEnemyObj.SetActive(false);
            isLevelComplete = true;
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.LevelFinished();
        }
    }

    public void IncrementProgress()
    {
        defeatedMonsterCount++;

        targetProgress = (float)defeatedMonsterCount / totalMonsterCount;
        Debug.Log("Target Progress: " + targetProgress);
    }

    private void CountMonsters()
    {
        if (monsterParent == null)
        {
            return;
        }

        totalMonsterCount = monsterParent.childCount;
    }
}

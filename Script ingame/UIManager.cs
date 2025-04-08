using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerSaved playerData;

    [Header("Door")]
    public GameObject pressDoorText;
    //=========Old Version Door=============
    //public GameObject interactDoor;
    //public Button scene1Button;
    //public Button scene2Button;
    //public string scene1Name;
    //public string scene2Name;
    //======��е�Ẻ����=========
    public GameObject choicePanel; // Panel ����纻���������͡
    public List<SceneOption> sceneOptions = new List<SceneOption>();
    public SceneOption requiredOption; // ������͡����ͧ�����ҧ���� 1 ����

    [System.Serializable] // ������ʴ�� Inspector ��
    public class SceneOption
    {
        public string sceneName;
        public string buttonText;
        public Color buttonColor = Color.white; // �ջ���
        public Sprite buttonSprite; // �ٻ�Ҿ�����ѧ����
    }
    public int minChoices = 1; // �ӹǹ������͡��鹵��
    public int maxChoices = 3; // �ӹǹ������͡�٧�ش


    [Header("Item")]
    public GameObject pressItemText;
    public TMP_Text itemDurability;

    [Header("Player Status")]
    public TMP_Text hpText;
    public Slider hpBar;

    [Header("GUI")]
    public GameObject loseButton;
    public GameObject winButton;
    public GameObject pauseMenuUI;
    public static bool GameIsPaused = false;

    private PlayerController playerStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        //========Door=========
        pressDoorText.SetActive(false);
        //interactDoor.SetActive(false);
        //scene1Button.onClick.AddListener(() => WarpToScene(scene1Name)); // ���� Listener ������ Scene 1
        //scene2Button.onClick.AddListener(() => WarpToScene(scene2Name));
        //fadeTransition.gameObject.SetActive(false);
        HideChoices();
        //========ITEM=========
        pressItemText.SetActive(false);
        itemDurability.gameObject.SetActive(false);
        //========Status=========
        loseButton.SetActive(false);
        winButton.SetActive(false);
        //========Pause=========
        pauseMenuUI.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void ShowChoices()
    {
        choicePanel.SetActive(true);
        GenerateChoices();
    }

    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }

    void GenerateChoices()
    {
        // ��ҧ����������͡���
        foreach (Transform child in choicePanel.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        if (playerData.roomClear == 5)
        {
            //���ҧ�������ͧ MiniBoss
            requiredOption.sceneName = "MiniBoss";
            requiredOption.buttonText = "MiniBoss";
            requiredOption.buttonColor = Color.magenta;

            sceneOptions.RemoveAt(0);

        }
        else if (playerData.roomClear == 10)
        {
            requiredOption.sceneName = "Boss";
            requiredOption.buttonText = "Boss";
            requiredOption.buttonColor = Color.blue;

            sceneOptions.RemoveAt(0);
        }

        // �����ӹǹ������͡
        int choiceCount = Random.Range(minChoices, maxChoices + 1);

        // ���ҧ List �ͧ������͡�������� (������ requiredOption)
        List<SceneOption> availableOptions = new List<SceneOption>(sceneOptions);
        availableOptions.Remove(requiredOption);

        // ���ҧ���� requiredOption ��͹����
        CreateButton(requiredOption);
        choiceCount--;

        // ���ҧ List �����纪��� Scene ��Т�ͤ������������
        List<string> usedSceneNames = new List<string>();
        List<string> usedButtonTexts = new List<string>();

        usedSceneNames.Add(requiredOption.sceneName);
        usedButtonTexts.Add(requiredOption.buttonText);

        for (int i = 0; i < choiceCount; i++)
        {
            if (availableOptions.Count == 0) break; // ��ش�������յ�����͡�����

            int randomIndex = Random.Range(0, availableOptions.Count);
            SceneOption selectedOption = availableOptions[randomIndex];

            // ��Ǩ�ͺ��������ӡѹ
            if (!usedSceneNames.Contains(selectedOption.sceneName) && !usedButtonTexts.Contains(selectedOption.buttonText))
            {
                CreateButton(selectedOption);
                usedSceneNames.Add(selectedOption.sceneName);
                usedButtonTexts.Add(selectedOption.buttonText);
                availableOptions.RemoveAt(randomIndex);
            }
            else
            {
                // ��ҫ�� �����������
                i--;
            }
        }
        // ���� Component Layout Group ���ͨѴ���§����
        HorizontalLayoutGroup layoutGroup = choicePanel.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter; // �Ѵ���˹觻����������ç��ҧ
        layoutGroup.spacing = 10; // ��˹�������ҧ�����ҧ����
    }

    void CreateButton(SceneOption option)
    {
        // ���ҧ����
        GameObject button = Instantiate(Resources.Load("ChoiceButton")) as GameObject;
        button.transform.SetParent(choicePanel.transform, false);

        // ��˹�������ٻ�Ҿ�����ѧ����
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = option.buttonColor;
        buttonImage.sprite = option.buttonSprite;

        button.GetComponentInChildren<TMP_Text>().text = option.buttonText; // ��˹���ͤ���������
        button.GetComponent<Button>().onClick.AddListener(() => WarpToScene(option.sceneName));    // ��˹���÷ӧҹ����͡�����
    }

    void WarpToScene(string sceneName)
    {
        // �ѹ�֡�����ż����� 
        //fadeTransition.gameObject.SetActive(true);
        SavePlayerData();
        SceneTransition.Instance.FadeToScene(sceneName);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // ��Ŵ Scene ������͡
    }

    public void Playagain()
    {
        playerData.ResetData();
        SceneManager.LoadScene("Scene1");
        Resume();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Resume();
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
    public void SavePlayerData()
    {
        playerStatus = FindObjectOfType<PlayerController>(); 
        if (playerStatus != null)
        {
            playerData.health = playerStatus.currentHealth;
        }
    }

    public void LoadUIData()
    {
        // ��Ŵ������ UI �ҡ uiData ����
        playerStatus = FindObjectOfType<PlayerController>();
        playerStatus.currentHealth = playerData.health;
        Debug.Log("Health: " + playerData.health);
        Debug.Log("Room Clear: " + playerData.roomClear);

    }

   
    public void UpdateUI()
    {
        UpdateHealth(playerData.health);
        UpdateHealthBar(playerData.health ,playerStatus.health);
    }

    public void UpdateHealth(int health)
    {
        hpText.text = "Health: " + health;
    }

    public void UpdateHealthBar(int currentHealth ,int maxHealth)
    {
        if (hpBar != null)
        {
            hpBar.value = (float)currentHealth / maxHealth;
        }
    }

    public void UpdateDurability(int durability)
    {
        itemDurability.text = "Dura : " + durability;
    }

}

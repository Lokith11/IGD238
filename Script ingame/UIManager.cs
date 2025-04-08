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
    //======ประตูแบบใหม่=========
    public GameObject choicePanel; // Panel ที่เก็บปุ่มตัวเลือก
    public List<SceneOption> sceneOptions = new List<SceneOption>();
    public SceneOption requiredOption; // ตัวเลือกที่ต้องมีอย่างน้อย 1 ปุ่ม

    [System.Serializable] // ทำให้แสดงใน Inspector ได้
    public class SceneOption
    {
        public string sceneName;
        public string buttonText;
        public Color buttonColor = Color.white; // สีปุ่ม
        public Sprite buttonSprite; // รูปภาพพื้นหลังปุ่ม
    }
    public int minChoices = 1; // จำนวนตัวเลือกขั้นต่ำ
    public int maxChoices = 3; // จำนวนตัวเลือกสูงสุด


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
        //scene1Button.onClick.AddListener(() => WarpToScene(scene1Name)); // เพิ่ม Listener ให้ปุ่ม Scene 1
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
        // ล้างปุ่มตัวเลือกเก่า
        foreach (Transform child in choicePanel.transform)
        {
            if (child.GetComponent<Button>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        if (playerData.roomClear == 5)
        {
            //สร้างปุ่มไปห้อง MiniBoss
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

        // สุ่มจำนวนตัวเลือก
        int choiceCount = Random.Range(minChoices, maxChoices + 1);

        // สร้าง List ของตัวเลือกที่เหลือ (ไม่รวม requiredOption)
        List<SceneOption> availableOptions = new List<SceneOption>(sceneOptions);
        availableOptions.Remove(requiredOption);

        // สร้างปุ่ม requiredOption ก่อนเสมอ
        CreateButton(requiredOption);
        choiceCount--;

        // สร้าง List เพื่อเก็บชื่อ Scene และข้อความที่ใช้แล้ว
        List<string> usedSceneNames = new List<string>();
        List<string> usedButtonTexts = new List<string>();

        usedSceneNames.Add(requiredOption.sceneName);
        usedButtonTexts.Add(requiredOption.buttonText);

        for (int i = 0; i < choiceCount; i++)
        {
            if (availableOptions.Count == 0) break; // หยุดถ้าไม่มีตัวเลือกเหลือ

            int randomIndex = Random.Range(0, availableOptions.Count);
            SceneOption selectedOption = availableOptions[randomIndex];

            // ตรวจสอบความไม่ซ้ำกัน
            if (!usedSceneNames.Contains(selectedOption.sceneName) && !usedButtonTexts.Contains(selectedOption.buttonText))
            {
                CreateButton(selectedOption);
                usedSceneNames.Add(selectedOption.sceneName);
                usedButtonTexts.Add(selectedOption.buttonText);
                availableOptions.RemoveAt(randomIndex);
            }
            else
            {
                // ถ้าซ้ำ ให้สุ่มใหม่
                i--;
            }
        }
        // เพิ่ม Component Layout Group เพื่อจัดเรียงปุ่ม
        HorizontalLayoutGroup layoutGroup = choicePanel.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter; // จัดตำแหน่งปุ่มให้อยู่ตรงกลาง
        layoutGroup.spacing = 10; // กำหนดระยะห่างระหว่างปุ่ม
    }

    void CreateButton(SceneOption option)
    {
        // สร้างปุ่ม
        GameObject button = Instantiate(Resources.Load("ChoiceButton")) as GameObject;
        button.transform.SetParent(choicePanel.transform, false);

        // กำหนดสีและรูปภาพพื้นหลังปุ่ม
        Image buttonImage = button.GetComponent<Image>();
        buttonImage.color = option.buttonColor;
        buttonImage.sprite = option.buttonSprite;

        button.GetComponentInChildren<TMP_Text>().text = option.buttonText; // กำหนดข้อความบนปุ่ม
        button.GetComponent<Button>().onClick.AddListener(() => WarpToScene(option.sceneName));    // กำหนดการทำงานเมื่อกดปุ่ม
    }

    void WarpToScene(string sceneName)
    {
        // บันทึกข้อมูลผู้เล่น 
        //fadeTransition.gameObject.SetActive(true);
        SavePlayerData();
        SceneTransition.Instance.FadeToScene(sceneName);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName); // โหลด Scene ที่เลือก
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
        // โหลดข้อมูล UI จาก uiData มาใช้
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

using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuItemController : MonoBehaviour {

    public static MenuItemController Instance;

    private const int START_LEVEL_ID = 1;
    private const float BUTTON_PADDING = 15f;

    private float verticalTriggerInput = 0;

    public int SelectedIndex { get; private set; }

    public AudioClip MoveSelectionClip, SelectClip, MusikClip;

    public TitleScreenButton.EnumButtonAction [] MenuItems;

    public GameObject ButtonPrefap;
    public List<GameObject> Buttons { get; private set; }

    public Sprite StartTextSprite, LoadTextSprite, OptionsTextSprite, QuitTextSprite;

    public Canvas MainCanvas;
    public Canvas OptionsCanvas;
    public GameObject buttonOriginPos;

    public bool IsMainMenu;

    // Use this for initialization
    void Start () {
        Instance = this;

        SoundManager.Instance.PlayMusik(MusikClip);

        Buttons = new List<GameObject>();
        SelectedIndex = 0;

        // Hier loppen wir durch alle im Inspector eingetragenen MenuItems und Initialisieren diese als "TitleScreenButton"
        for (int i = 0; i < MenuItems.Length; i++)
        {
            float yPos = buttonOriginPos.GetComponent<RectTransform>().position.y + ((MenuItems.Length + 1) * ButtonPrefap.GetComponent<RectTransform>().sizeDelta.y)+((MenuItems.Length +1 ) * BUTTON_PADDING);

            // Wir kalkulieren die Y-Position des neuen Buttons welche von der Anzahl der bisherigen durchläufe "i" abhängt
            Vector3 pos = new Vector3(buttonOriginPos.GetComponent<RectTransform>().position.x,
                yPos - (i * ButtonPrefap.GetComponent<RectTransform>().sizeDelta.y) - (i*BUTTON_PADDING) ,
                buttonOriginPos.GetComponent<RectTransform>().position.z);

			// Hier erzeugen wir eine Instance des "ButtonPrefap"s 
            GameObject currentButton_Go = Instantiate(ButtonPrefap, pos , transform.rotation) as GameObject;
	
			// Und setzen das Menu-Canvas als Parent
            currentButton_Go.transform.SetParent(MainCanvas.transform);
        	// Und anschließend geben wir dem Button die Function "MenuAction" als OnClick EventTrigger mit
            currentButton_Go.GetComponent<Button>().onClick.AddListener( MenuAction);
            
            // Wir holen uns eine Referenz zu dem "TitleScreenButton"-Skript und initialisieren den Button anhand der Einträge aus der Liste
            // "MenuItems"
            TitleScreenButton currentButton_Script = currentButton_Go.GetComponent<TitleScreenButton>();
            currentButton_Script.ButtonAction = MenuItems[i];
            string buttonText = string.Empty;
            switch (currentButton_Script.ButtonAction)
            {
                case TitleScreenButton.EnumButtonAction.Start:
                    buttonText = "Start";
                    break;
                case TitleScreenButton.EnumButtonAction.Load:
                    buttonText = "Load";
                    break;
                case TitleScreenButton.EnumButtonAction.Options:
                    buttonText = "Options";
                    break;
                case TitleScreenButton.EnumButtonAction.Quit:
                    buttonText = "Quit";
                    break;
                case TitleScreenButton.EnumButtonAction.Unpause:
                    buttonText = "Resume";
                    break;
            }
            currentButton_Script.TextComponent.text = buttonText;
            currentButton_Script.ButtonState = i == 0 ? TitleScreenButton.EnumButtonState.Selected : TitleScreenButton.EnumButtonState.Idle;
            currentButton_Script.Hover = OnHover;
            currentButton_Script.MyID = i;
            // Anschließend fügen wir diesen Button unserer Liste "Buttons" hinzu
            Buttons.Add(currentButton_Go);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (InputManager.Instance.ControllerState == InputManager.EnumControllerState.Menu)
        {
            if (InputManager.Instance.Trigger_Action_Start && IsMainMenu == false)
            {
                MainCanvas.gameObject.SetActive(false);
                InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
            }
            if (InputManager.Instance.Trigger_Action_One)
            {
                MenuAction();
            }
            if (InputManager.Instance.Vertical != 0f)
            {
                MoveMenuSelection(InputManager.Instance.VerticalTrigger);
            }
        }
        else if (InputManager.Instance.ControllerState == InputManager.EnumControllerState.ControlCharacter)
        {
            if (InputManager.Instance.Trigger_Action_Start)
            {
                SelectedIndex = 0;
                MainCanvas.gameObject.SetActive(true);
                InputManager.Instance.ControllerState = InputManager.EnumControllerState.Menu;
            }
        }
	}

    void MenuAction()
    {
        TitleScreenButton selectedButton_Script = Buttons[SelectedIndex].GetComponent<TitleScreenButton>();

        switch (selectedButton_Script.ButtonAction)
        {
            case TitleScreenButton.EnumButtonAction.Start:
                SoundManager.Instance.PlaySoundEffect(SelectClip);
                SceneManager.LoadScene(START_LEVEL_ID);
                break;
            case TitleScreenButton.EnumButtonAction.Load:
                SoundManager.Instance.PlaySoundEffect(SelectClip);
                Debug.LogError("class MenuItemController: void MenuAction() = case: TitleScreenButton.EnumButtonAction.Load ist noch nicht implementiert");
                break;
            case TitleScreenButton.EnumButtonAction.Options:
                SoundManager.Instance.PlaySoundEffect(SelectClip);
                MainCanvas.gameObject.SetActive(false);
                OptionsCanvas.gameObject.SetActive(true);
                break;
            case TitleScreenButton.EnumButtonAction.Quit:
                Application.Quit();
                break;
            case TitleScreenButton.EnumButtonAction.Unpause:
                MainCanvas.gameObject.SetActive(false);
                InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
                break;
        }
    }

    void MoveMenuSelection(float input)
    {
        // Wenn diese Funktion aufgerufen wurde und der Input 0 ist verlassen wir diese Funktion 
        if (input == 0)
            return;

        SoundManager.Instance.PlaySoundEffect(MoveSelectionClip);

        int indexToGo, indexAddition;

        // Hier prüfen wir ob der Vertical Input größer oder kleiner als null war und setzen dem entsprechend unserer Variable 
        // "indexAddition" zu 1 oder -1
        indexAddition = input > 0 ? -1 : 1;

        // Hier setzen wir den Button-Status des zuvor selektierten Button auf Idle
        Buttons[SelectedIndex].GetComponent<TitleScreenButton>().ButtonState = TitleScreenButton.EnumButtonState.Idle;

        // Nun kalkulieren wir den Index des neu selektieren Buttons
        indexToGo = SelectedIndex + indexAddition;

        if (indexToGo < 0)
            SelectedIndex = Buttons.Count - 1;
        else if (indexToGo > Buttons.Count - 1)
            SelectedIndex = 0;
        else
            SelectedIndex = indexToGo;

        // Hier setzen wir den Status des Buttons aus unserer Liste "Buttons" an der kalkulierten Position "SelectedIndex" auf selected
        Buttons[SelectedIndex].GetComponent<TitleScreenButton>().ButtonState = TitleScreenButton.EnumButtonState.Selected;
    }

    public void OnHover(int id)
    {
        SoundManager.Instance.PlaySoundEffect(MoveSelectionClip);

        // Hier setzen wir den Button-Status des zuvor selektierten Button auf Idle
        Buttons[SelectedIndex].GetComponent<TitleScreenButton>().ButtonState = TitleScreenButton.EnumButtonState.Idle;

        SelectedIndex = id;

        // Hier setzen wir den Status des Buttons aus unserer Liste "Buttons" an der kalkulierten Position "SelectedIndex" auf selected
        Buttons[SelectedIndex].GetComponent<TitleScreenButton>().ButtonState = TitleScreenButton.EnumButtonState.Selected;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChartEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        catManager = FindObjectOfType<ChartManager>();
        SetDotCountForLine(16);
        UpdateSelectedDot(0);
        UpdateOwner(0);
        for (int i = 0; i < helpMenu.transform.childCount; i++)
        {
            helpMenuPages.Add(helpMenu.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (!subtitleField.isFocused && !helpMenu.activeSelf) //Sorry for this
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) PlaceButton(-1);
            if (Input.GetKeyDown(KeyCode.Alpha1)) PlaceButton(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) PlaceButton(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) PlaceButton(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) PlaceButton(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) PlaceButton(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) PlaceButton(5);

            if (Input.GetKeyDown(KeyCode.A)) UpdateSelectedDot(-1);
            if (Input.GetKeyDown(KeyCode.D)) UpdateSelectedDot(1);
            if (Input.GetKeyDown(KeyCode.W)) UpdateSelectedDot(-16);
            if (Input.GetKeyDown(KeyCode.S)) UpdateSelectedDot(16);

            if (Input.GetKeyDown(KeyCode.LeftArrow)) UpdateOwner(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) UpdateOwner(1);

            if (Input.GetKeyDown(KeyCode.RightBracket)) SetDotCountForLine((byte)Mathf.FloorToInt(selectedDot + 1));
        }
        else if (helpMenu.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) UpdatePage(-1);
            if (Input.GetKeyDown(KeyCode.RightArrow)) UpdatePage(1);
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            helpMenu.SetActive(!helpMenu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            EditorLine editorLine = new EditorLine();
            editorLine.length = lineLength;
            editorLine.buttons = new List<Button>();
            editorLine.buttons = buttonsInLine;
            editorLine.subtitle = subtitleField.text;
            editorLines.Add(editorLine);
            lineCount.text = $"Lines: {editorLines.Count}";
            print(buttonsInLine.Count);
            while (buttonsInLine.Count > 0)
            {
                buttons.Add(buttonsInLine[0]);
                buttonsInLine.RemoveAt(0);
                Destroy(buttonOnDots[0]);
                buttonOnDots.RemoveAt(0);
            }
            SetDotCountForLine(16);
            selectedDot = 0;
            UpdateSelectedDot(0);
            subtitleField.text = string.Empty;
            print($"buttons: {buttons.Count}, buttons in line: {buttonsInLine.Count}");
        }
    }

    void UpdateOwner(int direction)
    {
        if (direction < 0) ownerNum--; else ownerNum++;
        if (ownerNum > 2) ownerNum = 2;
        if (ownerNum < 0) ownerNum = 0;
        ownerIndicatorGeneral.sprite = owner[ownerNum];
    }

    void UpdatePage(int direction)
    {
        helpMenuPages[currentPage].SetActive(false);
        if (direction > 0 && currentPage >= helpMenuPages.Count - 1)
        {
            helpMenuPages[0].SetActive(true); currentPage = 0;
        }
        else if (direction < 0 && currentPage == 0)
        {
            helpMenuPages[helpMenuPages.Count - 1].SetActive(true); currentPage = helpMenuPages.Count - 1;
        }
        else
        {
            helpMenuPages[currentPage + direction].SetActive(true);
            currentPage += direction;
        }
    }

    void UpdateSelectedDot(float direction)
    {
        if (Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(direction) == 1) direction /= 10;
        if (!Input.GetKey(KeyCode.LeftShift)) selectedDot = Mathf.FloorToInt(selectedDot);
        selectedDot += direction;
        if (selectedDot > 31) selectedDot = -2;
        if (selectedDot < -2) selectedDot = 31;
        if (selectedDot < 16) selected.anchoredPosition = new Vector2(-97 + (selectedDot * 15), 10);
        else selected.anchoredPosition = new Vector2(-97 + ((selectedDot - 16) * 15), -10);
    }

    void SetDotCountForLine(byte dotCount)
    {
        if (dotCount < 1 || dotCount > 32) return;
        for (int i = 0; i < displayLines[0].transform.childCount; i++) displayLines[0].transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < displayLines[1].transform.childCount; i++) displayLines[1].transform.GetChild(i).gameObject.SetActive(false);
        lineLength = dotCount;
        if (dotCount > 16)
        {
            displayLines[0].SetActive(true);
            for (int i = 0; i < displayLines[0].transform.childCount; i++) displayLines[0].transform.GetChild(i).gameObject.SetActive(true);
            displayLines[1].SetActive(true);
            for (int i = 0; i < dotCount - 16; i++) displayLines[1].transform.GetChild(i).gameObject.SetActive(true); 
        }
        else
        {
            displayLines[0].SetActive(true);
            for (int i = 0; i < dotCount + 2; i++) displayLines[0].transform.GetChild(i).gameObject.SetActive(true);
            displayLines[1].SetActive(false);
        }
    }

    void PlaceButton(int type)
    {
        if (selectedDot > lineLength) return;
        for (int i = 0; i < buttonOnDots.Count; i++)
        {
            if (buttonsInLine[i].position == selectedDot)
            {
                buttonsInLine.RemoveAt(i);
                Destroy(buttonOnDots[i]);
                buttonOnDots.RemoveAt(i);
            }
        }
        if (type >= 0)
        {
            GameObject buttonObject = Instantiate(buttonStamp, transform.GetChild(3));
            buttonObject.GetComponent<RectTransform>().anchoredPosition = selected.anchoredPosition;
            buttonObject.GetComponent<Image>().sprite = buttonType[type];
            Button button = new Button();
            button.owner = ownerNum;
            button.position = selectedDot;
            button.type = (byte)type;
            GameObject ownerObject = Instantiate(ownerIndicator, buttonObject.transform);
            ownerObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-8, -8);
            ownerObject.GetComponent<Image>().sprite = owner[ownerNum];
            ownerObject.SetActive(true);
            buttonsInLine.Add(button);
            buttonOnDots.Add(buttonObject);
        }
    }

    // Update is called once per frame
    public void Save()
    {
        print(buttons.Count);
        shart.bpm = bpm;
        for (int i = 0; i < editorLines.Count; i++)
        {
            Line line = new Line();
            line.length = editorLines[i].length;
            line.buttons = editorLines[i].buttons.ToArray();
            line.subtitle = editorLines[i].subtitle;
            lines.Add(line);
        }
        shart.lines = lines.ToArray();
        print(catManager.SaveChart(shart));
        lines.Clear();
    }

    public class EditorLine
    {
        public byte length;
        public List<Button> buttons = new List<Button>();
        public string subtitle;
    }

    public float bpm;
    public List<EditorLine> editorLines = new List<EditorLine>();
    public List<Button> buttonsInLine = new List<Button>();
    public Sprite[] owner; // 0 teacher 1 parappa 2 sfx

    float selectedDot;
    public RectTransform selected;
    public InputField subtitleField;
    int currentEditorLine;
    public int currentPage;
    public GameObject buttonStamp, ownerIndicator, helpMenu;
    public Sprite[] buttonType;
    public List<GameObject> buttonOnDots, helpMenuPages = new List<GameObject>();
    public GameObject[] displayLines;
    public Text lineCount;
    public Image ownerIndicatorGeneral;

    public Chart shart = new Chart();
    public List<Line> lines = new List<Line>();
    public List<Button> buttons = new List<Button>();
    byte lineLength;
    public byte ownerNum;
    ChartManager catManager;
}

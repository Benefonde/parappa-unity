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
    }

    private void Update()
    {
        if (!subtitleField.isFocused) //Sorry for this
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
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            EditorLine editorLine = new EditorLine();
            editorLine.length = 16; // currently forced
            editorLine.buttons = new List<Button>();
            editorLine.buttons = buttons;
            editorLine.subtitle = subtitleField.text;
            editorLines.Add(editorLine);
        }
    }

    void UpdateSelectedDot(float direction)
    {
        selectedDot += direction;
        if (selectedDot > editorLines[currentEditorLine].length - 1) selectedDot = -2;
        if (selectedDot < -2) selectedDot = editorLines[currentEditorLine].length - 1;
        selected.anchoredPosition = new Vector2(-97 + (selectedDot * 15), selected.anchoredPosition.y);
    }

    void PlaceButton(int type)
    {
        for (int i = 0; i < buttonOnDots.Count; i++)
        {
            if (buttons[i].position == selectedDot)
            {
                buttons.RemoveAt(i);
                Destroy(buttonOnDots[i]);
                buttonOnDots.RemoveAt(i);
            }
        }
        if (type >= 0)
        {
            GameObject buttonObject = Instantiate(buttonStamp, transform.GetChild(2));
            buttonObject.GetComponent<RectTransform>().anchoredPosition = selected.anchoredPosition;
            buttonObject.GetComponent<Image>().sprite = buttonType[type];
            Button button = new Button();
            button.owner = 0;
            button.position = selectedDot;
            button.type = (byte)type;
            buttons.Add(button);
            buttonOnDots.Add(buttonObject);
        }
    }

    // Update is called once per frame
    public void Save()
    {
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
        editorLines.Clear();
        lines.Clear();
    }

    public void CheckIfWrong()
    {
        print(catManager.SaveChart(catManager.LoadChart(catManager.SaveChart(shart))));
    }

    public class EditorLine
    {
        public byte length;
        public List<Button> buttons = new List<Button>();
        public string subtitle;
    }

    public float bpm;
    public List<EditorLine> editorLines = new List<EditorLine>();

    float selectedDot;
    public RectTransform selected;
    public InputField subtitleField;
    public int currentEditorLine;
    public GameObject buttonStamp;
    public Sprite[] buttonType;
    public List<GameObject> buttonOnDots = new List<GameObject>();

    public Chart shart = new Chart();
    public List<Line> lines = new List<Line>();
    public List<Button> buttons = new List<Button>();
    ChartManager catManager;
}

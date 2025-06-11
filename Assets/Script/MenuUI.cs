using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUI : MonoBehaviour
{
    public GameObject MenuPanel;
    public Button ExitButton;
    public Button StartButton;
    public ToggleGroup MenuToggleGroup;
    public ToggleGroup RecordToggleGroup;

    [Header("MenuTogglePanel")]
    public GameObject HitScanChallenge;
    public GameObject TrackingChallenge;
    public GameObject Record;

    [Header("RecordTogglePanel")]
    public GameObject HitScan;
    public GameObject Tracking;
    public GameObject RunningTime;

    [Header("Toggle Color")]
    public Color normalColor;
    public Color selectedColor;

    //��ŷ ǥ���Ҷ� ����� ������, RecordPanel�� �ڽ� ������Ʈ�� clone�� ����� �߰�(���� ����ȭ ��)
    [Header("Record Prefab")]
    public TextMeshProUGUI ChallengeRecord;
    public TextMeshProUGUI RunTimeRecord;

    //��� ����Ʈ
    private List<Toggle> Menutoggles = new List<Toggle>();
    private List<Toggle> RecordToggles = new List<Toggle>();

    //�ʱ�ȭ
    private void Start()
    {
        Menutoggles = MenuToggleGroup.GetComponentsInChildren<Toggle>().ToList();
        RecordToggles = RecordToggleGroup.GetComponentsInChildren<Toggle>().ToList();

        foreach (var toggle in Menutoggles)
        {
            toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(Menutoggles));
        }

        foreach (var toggle in RecordToggles)
        {
            toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(RecordToggles));
        }
    }

    //���� ��ư
    public void ExitMenuPanel()
    {
        MenuPanel.SetActive(false);
    }

    //TabŰ ������ �޴� Ȱ��ȭ
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            MenuPanel.SetActive(true);
        }
    }

    //��� ���� �� ���󺯰�� �г� Ȱ��ȭ
    private void OnToggleChanged(List<Toggle> toggles)
    {
        UpdateToggleColors(toggles);

        Toggle selectedToggle = toggles.FirstOrDefault(t => t.isOn);
        if(selectedToggle == null)
        {
            return;
        }

        if(toggles == Menutoggles)
        {
            UpdateMenuPanels(selectedToggle);
        }
        else if(toggles == RecordToggles)
        {
            UpdateRecordPanels(selectedToggle);
        }
    }

    //���� ���� �ڵ�
    private void UpdateToggleColors(List<Toggle> toggles)
    {
        foreach (var toggle in toggles)
        {
            var background = toggle.transform.Find("Background")?.GetComponent<Image>();
            if (background == null)
                continue;

            background.color = toggle.isOn ? selectedColor : normalColor;
        }
    }

    //�޴� �κ� ��� �г� Ȱ��
    private void UpdateMenuPanels(Toggle selectedToggle)
    {
        HitScanChallenge.SetActive(false);
        TrackingChallenge.SetActive(false);
        Record.SetActive(false);
        StartButton.gameObject.SetActive(false);

        if (selectedToggle.name == "HitScanChallengeButton")
        {
            HitScanChallenge.SetActive(true);
            StartButton.gameObject.SetActive(true);
        }
        else if (selectedToggle.name == "TrackingChallengeButton")
        {
            TrackingChallenge.SetActive(true);
            StartButton.gameObject.SetActive(true);
        } 
        else if (selectedToggle.name == "RecordButton")
        {
            Record.SetActive(true);
            StartButton.gameObject.SetActive(false);
        }   
    }
    
    //��� �κ� ��� �г� ����
    private void UpdateRecordPanels(Toggle selectedToggle)
    {
        HitScan.SetActive(false);
        Tracking.SetActive(false);
        RunningTime.SetActive(false);

        if (selectedToggle.name == "HitScanRecord")
        {
            HitScan.SetActive(true);
        }   
        else if (selectedToggle.name == "TrackingRecord")
        {
            Tracking.SetActive(true);
        }    
        else if (selectedToggle.name == "RunningTime")
        {
            RunningTime.SetActive(true);
        }   
    }
}

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

    //랭킹 표시할때 사용할 프리팹, RecordPanel의 자식 오브젝트로 clone을 만들어 추가(정보 동기화 후)
    [Header("Record Prefab")]
    public TextMeshProUGUI ChallengeRecord;
    public TextMeshProUGUI RunTimeRecord;

    //토글 리스트
    private List<Toggle> Menutoggles = new List<Toggle>();
    private List<Toggle> RecordToggles = new List<Toggle>();

    //초기화
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

    //종료 버튼
    public void ExitMenuPanel()
    {
        MenuPanel.SetActive(false);
    }

    //Tab키 누르면 메뉴 활성화
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            MenuPanel.SetActive(true);
        }
    }

    //토글 선택 시 색상변경과 패널 활성화
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

    //색상 변경 코드
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

    //메뉴 부분 토글 패널 활성
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
    
    //기록 부분 토글 패널 변경
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

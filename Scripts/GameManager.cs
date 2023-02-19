using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Property Point")]
    [SerializeField] private int _points = 0;
    [SerializeField] private int _limitOfHigh = 10000;

    [Header("Property Cost")]
    [SerializeField] private int _upCost = 10;
    [SerializeField] private int _downCost = 50;
    [SerializeField] private int _automaticCost = 1000;
    [SerializeField] private int _automaticUpCost = 100;

    [Header("Sprite")]
    [SerializeField] private Sprite[] _muteImage;

    [Header("Property Up/Down")]
    private int _upAutomaticPointUp = 5;
    private int _upAutomaticPointDown = 1;
    private int _pointsToUp = 1;
    private float _timeToDownHigh = 2;

    [Header("Actions")]
    private bool _pressButton = false;
    private bool _beginPlay = false;

    [Header("Component Reference")]
    private Player _player;

    private TextMeshProUGUI _highLevelText;
    private TextMeshProUGUI _upCostText;
    private TextMeshProUGUI _downCostText;
    private TextMeshProUGUI _downCanabisImage;
    private TextMeshProUGUI _automaticCostText;
    private TextMeshProUGUI _automaticUpCanabisImage;
    private TextMeshProUGUI _highLevelUpText;
    private TextMeshProUGUI _automaticCanabisImage;
    private TextMeshProUGUI _automaticUpCostText;
    private TextMeshProUGUI _stopWatchText;

    private Button _automaticButton;
    private Button _upAutomaticButton;
    private Button _downHighButton;

    private Image _muteButton;
    private Slider _highLevelpercent;
    private Image _full;

    private GameObject _menuScreen;
    private GameObject _gameScreen;

    private bool _mute;
    [SerializeField] private float _seconds;



    private void Awake()
    {
        _menuScreen = GameObject.Find("Canvas/MenuScreen").gameObject;
        _gameScreen = GameObject.Find("Canvas/GameScreen").gameObject;

        _highLevelText = GameObject.Find("Canvas/GameScreen/HighLevel").gameObject.GetComponent<TextMeshProUGUI>();
        _highLevelpercent = GameObject.Find("Canvas/GameScreen/HighPercent").gameObject.GetComponent<Slider>();

        _upCostText = GameObject.Find("Canvas/GameScreen/Panel/UpButton/UpText").gameObject.GetComponent<TextMeshProUGUI>();
        _downCostText = GameObject.Find("Canvas/GameScreen/Panel/DownButton/DownText").gameObject.GetComponent<TextMeshProUGUI>();
        _automaticCostText = GameObject.Find("Canvas/GameScreen/Panel/AutomaticButton/AutomaticText").gameObject.GetComponent<TextMeshProUGUI>();
    
        _automaticButton = GameObject.Find("Canvas/GameScreen/Panel/AutomaticButton").gameObject.GetComponent<Button>();
        _automaticCanabisImage = GameObject.Find("Canvas/GameScreen/Panel/AutomaticButton/CanabisIcon").gameObject.GetComponent<TextMeshProUGUI>();
        _automaticUpCanabisImage = GameObject.Find("Canvas/GameScreen/Panel/AutomaticUpButton/CanabisIcon").gameObject.GetComponent<TextMeshProUGUI>();

        _muteButton = GameObject.Find("Canvas/MuteButton").GetComponent<Image>();

        _downHighButton = GameObject.Find("Canvas/GameScreen/Panel/DownButton").gameObject.GetComponent<Button>();
        _downCanabisImage = GameObject.Find("Canvas/GameScreen/Panel/DownButton/CanabisIcon").gameObject.GetComponent<TextMeshProUGUI>();

        _upAutomaticButton = GameObject.Find("Canvas/GameScreen/Panel/AutomaticUpButton").gameObject.GetComponent<Button>();
        _upAutomaticButton.interactable = false;

        _automaticUpCostText = GameObject.Find("Canvas/GameScreen/Panel/AutomaticUpButton/AutomaticUpText").GetComponent<TextMeshProUGUI>();

        _highLevelUpText = GameObject.Find("Canvas/GameScreen/HighLevelUpInfo").GetComponent<TextMeshProUGUI>();

        _stopWatchText = GameObject.Find("Canvas/GameScreen/StopWatch").GetComponent<TextMeshProUGUI>();

        _full =  GameObject.Find("Canvas/GameScreen/HighPercent/Fill Area/Fill/Full").gameObject.GetComponent<Image>();

        _player = GameObject.Find("Player").gameObject.GetComponent<Player>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameScreen.SetActive(false);
        StartCoroutine(DownHighPoints());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) && !_beginPlay)
        {
            _player.StartSong();
            _beginPlay = true;
            _gameScreen.SetActive(true);
            _menuScreen.SetActive(false);
            StartCoroutine(StopWatch());
        }

        UpdateUI();
        SetValueLimit();
    }

    private void UpdateUI()
    {
        _highLevelpercent.value = GetPercentHigh();
        _highLevelText.text = $"{_points}";

        _upCostText.text = $"{_upCost}";
        _automaticCostText.text = $"{_automaticCost}";

        _full.enabled = GetPercentHigh() == 1;

        _downCostText.text = _downCost.ToString();
        _highLevelUpText.text = $"+{_pointsToUp.ToString()}";

        if(!_automaticButton.interactable)
        {
            _downHighButton.interactable = false;
            _downCostText.enabled = false;
            _downCanabisImage.enabled = false;
            _automaticUpCanabisImage.enabled = true;
            _automaticUpCostText.enabled = true;
        }
        else
        {
            _automaticUpCostText.enabled = false;
            _automaticUpCanabisImage.enabled = false;
        }

        if(!_automaticButton.interactable && _points >= _limitOfHigh)
        {
            _stopWatchText.gameObject.SetActive(true);

            string minutes = Mathf.Floor(_seconds / 60).ToString("00");
            string seconds = (_seconds % 60).ToString("00");
            _stopWatchText.text = $"You have finished the game in {minutes}:{seconds}";
        }

        _automaticUpCostText.text = _upAutomaticButton.interactable ? _automaticUpCost.ToString() : "00";
    }

    private void SetValueLimit()
    {

        _upAutomaticPointDown = _pointsToUp;
        
        _upCost = Mathf.Clamp(_upCost, 0, _limitOfHigh/ 3);
        _downCost = Mathf.Clamp(_downCost, 0, _limitOfHigh/ 3);
        _automaticCost = Mathf.Clamp(_automaticCost, 0, _limitOfHigh/ 3);
        _automaticUpCost = Mathf.Clamp(_automaticUpCost, 0, _limitOfHigh/3);
    }

    public void SetUpAutomaticPoints()
    {
        if(_points >= _automaticUpCost)
        {
            _points -= _automaticUpCost;
            _upAutomaticPointUp += 5;
            _automaticUpCost *= 2;
        }
    }

    public void SetAutomaticDrug()
    {
        if (_points >= _automaticCost)
        {
            _points -= _automaticCost;
            _automaticButton.interactable = false;
            _automaticCostText.enabled = false;
            _automaticCanabisImage.enabled = false;
            _upAutomaticButton.interactable = true;
            StartCoroutine(AutomaticDrug());
        }
    }

    private IEnumerator AutomaticDrug()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        //After we have waited 5 seconds print the time again.
        _points += _upAutomaticPointUp;

        if(_player.GetAnimator().GetBool("Smoke"))
        {
            _player.GetAnimator().SetBool("Smoke", false);
            _player.SpawnSmoke();
        }
        else
        {
            _player.GetAnimator().SetBool("Smoke", true);
        }
        _points = Mathf.Clamp(_points, 0, _limitOfHigh);
        StartCoroutine(AutomaticDrug());
    }

    private IEnumerator DownHighPoints()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(_timeToDownHigh);

        //After we have waited 5 seconds print the time again.
        if(!_pressButton)
        {
            _points -= _upAutomaticPointDown;
        }
        _points = Mathf.Clamp(_points, 0, _limitOfHigh);
        if(_automaticButton.interactable)
        {
            StartCoroutine(DownHighPoints());
        }

    }

    private IEnumerator StopWatch() {

        yield return new WaitForSeconds(1);
        if (_automaticButton.interactable || _points >= _limitOfHigh)
        {
            _seconds++;
            StartCoroutine(StopWatch());
        }
    }

    public void SetPressButton(bool PressButton)
    {
        _pressButton = PressButton;
    }

    public void SetResistance()
    {
        if(_points >= _downCost)
        {
            _timeToDownHigh++;
            _points -= _downCost;
            _downCost *= 2;
        }
    }

    public void SetLevelHigh() 
    { 
        _points += _pointsToUp;
        _points = Mathf.Clamp(_points, 0, _limitOfHigh);
    }

    public bool GetBeginPlay() { return _beginPlay; }

    public float GetPercentHigh()
    {
        return (float)_points / _limitOfHigh;
    }

    public void SetPointsToUp()
    {
        if(_points >= _upCost)
        {
            _pointsToUp++;
            _points -= _upCost;
            _upCost *= 2;
        }
    }

    public int GetPoints() { return _points; }

    public int GetLimitOfHigh() { return _limitOfHigh; }

    public void Mute()
    {
        _mute = !_mute;

        if(_mute)
        {
            _muteButton.sprite = _muteImage[0];
        }
        else
        {
            _muteButton.sprite = _muteImage[1];
        }

        _player.GetAudioSource().mute = _mute;
    }
}

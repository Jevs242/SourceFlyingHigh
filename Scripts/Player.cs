using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Sprite")]
    [Tooltip("Different state of the player")]
    [SerializeField] private AnimationClip[] _highStateSprite;
    [Tooltip("Smoke Sprite")]

    [Header("Sound")]
    [SerializeField] private GameObject _smokeParticle;
    [Tooltip("Game's Song")]
    [SerializeField] private AudioClip _loopSong;
    
    [Header("Component Reference")]    
    private GameManager _gameManager;
    private Animator _animator;
    private AudioSource _audioSource;
    private AnimatorOverrideController _animationOverride;

    [Header("Spawn Object")]
    private GameObject _smokeP;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").gameObject.GetComponent<GameManager>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _animationOverride = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animationOverride;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager.GetBeginPlay()) return;

        ClickAction();
        FixSpriteAnimation();
    }

    private bool RayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.transform.tag == "Player")
            {
                return true;
            }
        }
        else
        {
            return false;
        }
        return false;
    }

    private void ClickAction()
    {
        if (RayCast())
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _animator.SetBool("Smoke", true);
                _gameManager.SetLevelHigh();
                _gameManager.SetPressButton(true);
            }
        }

        if (_animator.GetBool("Smoke"))
        {
            if (Input.GetMouseButtonUp(0))
            {
                _animator.SetBool("Smoke", false);
                SpawnSmoke();
                StartCoroutine(DownHighPoints());
            }
        }

    }

    public void SpawnSmoke()
    {
        if (_smokeP == null)
        {
            _smokeP = Instantiate(_smokeParticle, new Vector3(-0.39199999f, -0.689999998f, 1), new Quaternion(0, 0, -0.997682393f, -0.0680437833f)) as GameObject;
            Destroy(_smokeP, 2);
        }
    }

    private IEnumerator DownHighPoints()
    {
        yield return new WaitForSeconds(2);
        if(_animator.GetBool("Smoke") && RayCast())
        {
            StartCoroutine(DownHighPoints());
        }
        else
        {
            _gameManager.SetPressButton(false);
        }
    }

    private void FixSpriteAnimation()
    {
        switch (string.Format("{0:F1}", _gameManager.GetPercentHigh()))
        {
            case "0.1":
                _animationOverride["NoSmoke1"] = _highStateSprite[1];
                break;
            case "0.2":
                _animationOverride["NoSmoke1"] = _highStateSprite[2];
                break;
            case "0.3f":
                _animationOverride["NoSmoke1"] = _highStateSprite[3];
                break;
            case "0.4f":
                _animationOverride["NoSmoke1"] = _highStateSprite[4];
                break;
            case "0.5f":
                _animationOverride["NoSmoke1"] = _highStateSprite[4];
                break;
            case "0.6":
                _animationOverride["NoSmoke1"] = _highStateSprite[5];
                break;
            case "0.7":
                _animationOverride["NoSmoke1"] = _highStateSprite[5];
                break;
            case "0.8":
                _animationOverride["NoSmoke1"] = _highStateSprite[6];
                break;
            case "0.9":
                _animationOverride["NoSmoke1"] = _highStateSprite[6];
                break;
            case "1.0":
                _animationOverride["NoSmoke1"] = _highStateSprite[7];
                break;
            default:
                _animationOverride["NoSmoke1"] = _highStateSprite[0];
                break;
        }
    }

    public void StartSong()
    {
        _audioSource.clip = _loopSong;
        _audioSource.Play();
    }

    public Animator GetAnimator() { return _animator; }

    public AudioSource GetAudioSource() { return _audioSource; }
}

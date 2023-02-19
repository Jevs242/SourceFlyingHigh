using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Transparency : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private bool _isPostProcessing;

    [Header("Component Reference")]
    private SpriteRenderer _spriteRenderer;
    private GameManager _gameManager;
    private Color _colorSprite;
    private PostProcessVolume _postProcess;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_isPostProcessing)
        {
            _postProcess = GameObject.Find("PostProcessing").GetComponent<PostProcessVolume>();
        }
        else
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _colorSprite = _spriteRenderer.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_isPostProcessing )
        {
            _postProcess.weight = _gameManager.GetPercentHigh();
        }
        else
        {
            _spriteRenderer.color = new Color(_colorSprite.r,_colorSprite.b,_colorSprite.g,_gameManager.GetPercentHigh());
        }
    }
}

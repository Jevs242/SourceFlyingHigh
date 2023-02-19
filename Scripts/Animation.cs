using System.Collections;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private float _seconds = 1;

    [Header("Component Reference")]
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        _animator.enabled = false;
        StartCoroutine(TimerAnimation());
    }

    private IEnumerator TimerAnimation()
    {
        yield return new WaitForSeconds(_seconds);
        _animator.enabled = true;
        Destroy(this);
    }
}

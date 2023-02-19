using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItem : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private float _speed = 1;
    [SerializeField] private Direction Dir;
    [SerializeField] private bool IsCloud;
    [SerializeField] private bool IsAlien;

    [Header("Transform")]
    private Vector3 _BeginPosition;
    private Quaternion _BeginRotation;

    private enum Direction
    {
        Left,
        Right
    }

    // Start is called before the first frame update
    void Start()
    {
        _BeginPosition = transform.position;
        _BeginRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        switch(Dir)
        {
            case Direction.Left:
                if(transform.position.x <= -11.5)
                {
                    if(IsAlien)
                    {
                        Dir = Direction.Right;
                        return;
                    }

                    if(IsCloud)
                    {
                        transform.position = new Vector3(13f, transform.position.y);
                        transform.rotation = _BeginRotation;
                    }
                    else
                    {
                        transform.position = _BeginPosition;
                    }
                }
                transform.position -= new Vector3(0.1f * _speed, 0) * Time.deltaTime;
                break;
            case Direction.Right:
                if (transform.position.x >= 11.5)
                {
                    if(IsAlien)
                    {
                        Dir = Direction.Left;
                    }
                    else
                    {
                        transform.position = _BeginPosition;
                    }
                }
                transform.position += new Vector3(0.1f * _speed, 0) * Time.deltaTime;
                break; 
            default:
                break;
        }
    }
}

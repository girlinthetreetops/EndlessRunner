using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent OnRightSwipe;
    public UnityEvent OnLeftSwipe;
    public UnityEvent OnSwipeUp;
    public UnityEvent OnSwipeDown;

    //Computer specific
    public UnityEvent OnPauseToggle;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnLeftSwipe.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnRightSwipe.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnSwipeUp.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnSwipeDown.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnPauseToggle.Invoke();
        }
    }
}
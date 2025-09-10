using System;
using HutongGames.PlayMaker.Actions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//using UnityEngine.InputSystem;

public class TabNavigation : MonoBehaviour
{
    public Selectable[] selectables; // Arreglo con los elementos en orden de tabulación
    
    private int currentIndex = -1;

    private void OnEnable()
    {
        currentIndex = -1;
        NavigateToNext();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        //if (Keyboard.current[Key.Tab].wasPressedThisFrame)
        {
            // Navegar al siguiente elemento
            NavigateToNext();
        }
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        //if (Keyboard.current[Key.Enter].wasPressedThisFrame || Keyboard.current[Key.NumpadEnter].wasPressedThisFrame)
        {
            // Si es un botón, activarlo al presionar Enter
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                Button button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                if (button != null && button.interactable)
                {
                    button.onClick.Invoke();
                }
            }
        }
    }
    
    private void NavigateToNext()
    {
        if (selectables.Length == 0) return;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        // if (Keyboard.current[Key.LeftShift].isPressed || Keyboard.current[Key.RightShift].isPressed)
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = selectables.Length - 1;
        }
        else
        {
            currentIndex++;
            if (currentIndex >= selectables.Length) currentIndex = 0;
        }
        
        Selectable nextSelectable = selectables[currentIndex];
        if (nextSelectable != null && nextSelectable.interactable)
        {
            nextSelectable.Select();
        }
        else
        {
            // Si el elemento no es interactuable, buscar el siguiente
            NavigateToNext();
        }
    }
}
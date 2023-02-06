using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonSceneChange : MonoBehaviour, IPointerDownHandler
{

    [SerializeField]
    private int build_index;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        SceneManager.LoadScene(build_index);
    }
}
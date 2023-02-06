using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music music;

    private void Awake()
    {
        if (music != null && music!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            music = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}

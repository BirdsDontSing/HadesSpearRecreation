using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] AudioClip levelMusic;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        FloatingTextController.InitializeFloatingText();

        AudioHelper.PlayClip2D(levelMusic, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

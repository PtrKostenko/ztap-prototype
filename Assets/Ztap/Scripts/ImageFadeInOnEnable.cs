using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeInOnEnable : MonoBehaviour
{
    [SerializeField] private float time = 2;
    Image _im;

    private void Awake()
    {
        _im = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _im.canvasRenderer.SetAlpha(0.01f);
        _im.CrossFadeAlpha(1, time, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private AudioSource _audio;
    private Image _image;
    
    private bool play = true;
    void Start()
    {
        _image = GetComponent<Image>();
        play = PlayerPrefs.GetInt("play", 1) == 1;
        UpdateImage();
    }
    
    private void UpdateImage()
    {
        if (play)
        {
            _image.color = Color.green;
            _audio.volume = 1;
        }
        else
        {
            _image.color = Color.red;
            _audio.volume = 0;
        }
    }
    // Update is called once per frame

    public void OnPointerClick(PointerEventData eventData)
    {
        play = !play;
        UpdateImage();
        PlayerPrefs.SetInt("play", play?1:0);
    }
}

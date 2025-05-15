using UnityEngine;
using UnityEngine.UI;

public class DinoInfoManager : MonoBehaviour
{
    public static DinoInfoManager Instance;

    public DinoDatabase database;
    public Text infoTextUI;
    public AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public void SetCurrentDino(string dinoName)
    {
        DinoInfo info = database.GetDinoInfo(dinoName);
        if (info != null)
        {
            infoTextUI.text = info.infoText;
            audioSource.clip = info.audioClip;
        }
        else
        {
            infoTextUI.text = "No information available.";
            audioSource.clip = null;
        }
    }

    public void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}

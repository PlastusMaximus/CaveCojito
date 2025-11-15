using UnityEngine;
using UnityEngine.Rendering;

public class FlashCam : MonoBehaviour
{
    [SerializeField] private Light flashCam;
    [SerializeField] private Volume volume;
    [SerializeField] private CanvasGroup AlphaController;
    [SerializeField] private AudioSource flashBang;
    [SerializeField] private AudioSource flashRecharge;
    
    public bool on = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (on)
        {
            AlphaController.alpha -= Time.deltaTime * 2;
            volume.GetComponent<Volume>().weight -= Time.deltaTime * 2;
            flashCam.intensity -= Time.deltaTime * 200;

            if (!flashRecharge.isPlaying)
            {
                volume.GetComponent<Volume>().weight = 0;
                AlphaController.alpha = 0;
                flashCam.intensity = 0;
                flashCam.gameObject.SetActive(false);
                on = false;
            }
        }
    }

    public void flashBanged()
    {
        volume.GetComponent<Volume>().weight = 1;
        flashBang.Play();
        flashRecharge.Play();
        AlphaController.alpha = 1;
        flashCam.intensity = 100;
    }
}

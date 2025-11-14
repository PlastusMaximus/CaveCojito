using UnityEngine;
using UnityEngine.Rendering;

public class FlashCam : MonoBehaviour
{

    [SerializeField] private Volume volume;
    [SerializeField] private CanvasGroup AlphaController;
    [SerializeField] private AudioSource flashBang;
    [SerializeField] private AudioSource flashRecharge;
    
    private bool on = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            flashBanged();
        }

        if (on == true)
        {
            Time.timeScale = .05f;
            AlphaController.alpha = AlphaController.alpha - Time.deltaTime * 2;
            volume.GetComponent<Volume>().weight = volume.GetComponent<Volume>().weight - Time.deltaTime * 2;

            if (AlphaController.alpha <= 0)
            {
                volume.GetComponent<Volume>().weight = 0;
                AlphaController.alpha = 0;
                Time.timeScale = 1;
                on = false;
            }
        }
    }

    private void flashBanged()
    {
        volume.GetComponent<Volume>().weight = 1;
        on = true;
        flashBang.Play();
        flashRecharge.Play();
        AlphaController.alpha = 1;
    }
}

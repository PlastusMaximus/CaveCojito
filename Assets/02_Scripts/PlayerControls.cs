using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private GameObject flashlight;
    private bool flashlightActive;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.gameObject.SetActive(false);    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (flashlightActive == false)
            {
                flashlight.gameObject.SetActive(true);
                flashlightActive = true;
            }
            else
            {
                flashlight.gameObject.SetActive(false);
                flashlightActive = false;
            }
        }
    }
}

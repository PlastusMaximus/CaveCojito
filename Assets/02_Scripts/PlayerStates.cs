using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class PlayerStates : MonoBehaviour
{
    private enum States : int
    {
        WALKING = 0,
        CRAWLING = 1
    }
    
    [SerializeField] private States state = States.WALKING;

    [SerializeField] private CapsuleCollider playerBody;
    [SerializeField] private Camera playerCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (state)
        {
            case States.WALKING:
                playerBody.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                playerCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case States.CRAWLING:
                playerBody.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                playerCamera.transform.rotation = Quaternion.Euler(-180f, 0f, 0f);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

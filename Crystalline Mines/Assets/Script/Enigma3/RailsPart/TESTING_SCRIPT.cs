using UnityEngine;

public class TESTING_SCRIPT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("Reset");

            RailManager.Instance.ResetAllRails();
        }
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string UserName, Email, Mobile, Gender;

    private void Awake()
    {
        instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Init()
    {

    }
}

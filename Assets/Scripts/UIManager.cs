using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject[] ScreenArray;

    #region UI ELEMENTS

    [SerializeField]
    private Button Enter, submit, male, female;

    [SerializeField]
    private TMP_InputField userNameInputfield, emailInputfield, phoneNoInputfield;

    #endregion

    private void Awake()
    {
        instance = this;

        Enter.onClick.AddListener(() => ScreenSetter(1));
        submit.onClick.AddListener(SubmitRegistration);
        male.onClick.AddListener(() => GenderSelection("male"));
        female.onClick.AddListener(() => GenderSelection("female"));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ScreenSetter(int index)
    {
        switch (index)
        {
            case 0: //Home screen
                ScreenArray[0].SetActive(true);
                break;
            case 1: //Registration
                ScreenArray[0].SetActive(false);
                ScreenArray[1].SetActive(true);
                break;
            case 2://Gender Selection
                ScreenArray[1].SetActive(false);
                ScreenArray[2].SetActive(true);
                break;
            case 3://Camera view

                break;
            default:
                break;
        }
    }

    private void EnableVirtualKeyboard()
    {

    }

    private void GetCamera()
    {

    }



    private void GenderSelection(string gender)
    {
        GameManager.instance.Gender = gender;
        ScreenSetter(3);
    }

    private void SubmitRegistration()
    {
        GameManager.instance.UserName = userNameInputfield.text;
        GameManager.instance.Email = emailInputfield.text;
        GameManager.instance.Mobile = phoneNoInputfield.text;

        ScreenSetter(2);
    }


}

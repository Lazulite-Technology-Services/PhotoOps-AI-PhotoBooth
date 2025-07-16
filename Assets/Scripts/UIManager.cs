using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject[] ScreenArray;

    [SerializeField]
    private GameObject CountdownScreenButtonGroup, countDownPanelFooterText;

    #region UI ELEMENTS

    [SerializeField]
    private Button Enter, submit, male, female, capture, retake, next, proceed, regenerate;

    [SerializeField]
    private Button[] environmentSelectionButtonArray;

    [SerializeField]
    private TMP_InputField userNameInputfield, emailInputfield, phoneNoInputfield;
       
    public RawImage CamFeed, ScreenShotImage;

    public RawImage ScreenShotArea;

    [SerializeField]
    private Text countDownText;
    

    #endregion

    private int timer = 3;

    private void Awake()
    {
        instance = this;

        Enter.onClick.AddListener(() => ScreenSetter(1));
        submit.onClick.AddListener(SubmitRegistration);
        male.onClick.AddListener(() => GenderSelection("male"));
        female.onClick.AddListener(() => GenderSelection("female"));
        capture.onClick.AddListener(() => ScreenSetter(4));
        retake.onClick.AddListener(() => StartCoroutine(CountDown()));
        next.onClick.AddListener(() => ScreenSetter(5));
        proceed.onClick.AddListener(() => ScreenSetter(7));
        regenerate.onClick.AddListener(() => ScreenSetter(8));

        for(int i = 0; i < environmentSelectionButtonArray.Length; i++)
        {
            environmentSelectionButtonArray[i].onClick.AddListener(() => ScreenSetter(6));
        }
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
                ScreenArray[2].SetActive(false);
                ScreenArray[3].SetActive(true);
                ScreenCaptureHandler.instance.GetCamera();
                break;
            case 4://Count down
                capture.gameObject.SetActive(false);
                ScreenArray[4].SetActive(true);
                StartCoroutine(CountDown());
                break;
            case 5://AI environment selection
                ScreenCaptureHandler.instance.webcamTexture.Stop();
                ScreenArray[3].SetActive(false);
                ScreenArray[4].SetActive(false);
                ScreenArray[5].SetActive(true);
                break;
            case 6://Final screen showing output
                ScreenArray[5].SetActive(false);
                ScreenArray[6].SetActive(true);
                break;
            case 7:
                ScreenArray[6].SetActive(false);
                ScreenArray[7].SetActive(true);

                StartCoroutine(Reset());
                break;
            case 8://Regenerate
                ScreenArray[6].SetActive(false);
                ScreenArray[5].SetActive(true);
                break;
            default:
                break;
        }
    }

    private void EnableVirtualKeyboard()
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

    private IEnumerator CountDown()
    {
        if(timer == 0)
        {
            StartCoroutine(ScreenCaptureHandler.instance.CaptureUI());
            //Stopcountdown and reset
            StopCoroutine(CountDown());
            CountdownScreenButtonGroup.SetActive(true);
            countDownText.gameObject.SetActive(false);
            countDownPanelFooterText.SetActive(false);
            timer = 3;

            

        }
        else
        {
            countDownPanelFooterText.SetActive(true);

            if (CountdownScreenButtonGroup.activeSelf)
                CountdownScreenButtonGroup.SetActive(false);

            if (!countDownText.gameObject.activeSelf)
                countDownText.gameObject.SetActive(true);

            countDownText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;

            StartCoroutine(CountDown());
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(5f);

        capture.gameObject.SetActive(true);
        countDownPanelFooterText.SetActive(true);
        CountdownScreenButtonGroup.SetActive(false);
        ScreenArray[7].SetActive(false);

        ScreenSetter(0);
    }


}

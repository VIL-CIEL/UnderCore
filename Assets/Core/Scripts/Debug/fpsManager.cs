using UnityEngine;
using TMPro;

public class fpsManager : MonoBehaviour
{
    public GameObject MessageBox;
    private ShowMessage MessBoxScript;
    private TextMeshProUGUI FPSText;
    private bool isToogle;
    void Start()
    {
        FPSText = GetComponent<TextMeshProUGUI>();
        MessBoxScript = MessageBox.GetComponent<ShowMessage>();
        isToogle = false;
        InvokeRepeating("getfps",1,1);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(isToogle){
                isToogle = false;
                MessBoxScript.newMessage("{ DEBUG } : fps > OFF", 2);
            }
            else{
                isToogle = true;
                MessBoxScript.newMessage("{ DEBUG } : fps > ON", 2);
            }
        }
    }

    void getfps()
    {
        if(isToogle)FPSText.text = $"FPS: {(int)(1f/Time.unscaledDeltaTime)}";
        else FPSText.text = "";
    }
}

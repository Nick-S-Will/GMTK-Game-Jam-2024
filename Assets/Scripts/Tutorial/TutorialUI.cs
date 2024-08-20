using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private RoomCameraSwap cameraSwap;
    [SerializeField] private CinemachineVirtualCamera workshopCam, playroomCam;
    [SerializeField] private GameObject workshopTutorialParent, playroomTutorialParent;
    [SerializeField] private TMP_Text nextButtonText;
    [SerializeField] private ProgressHandler progressHandler;
    [SerializeField] private string nextButtonFinalText = "Start";

    private int step = 0;

    private void Awake()
    {
        Assert.IsNotNull(cameraSwap);
        Assert.IsNotNull(workshopCam);
        Assert.IsNotNull(playroomCam);
        Assert.IsNotNull(workshopTutorialParent);
        Assert.IsNotNull(playroomTutorialParent);
        Assert.IsNotNull(nextButtonText);
        Assert.IsNotNull(progressHandler);
    }

    public void Next()
    {
        if (step == 0)
        {
            cameraSwap.SwapTo(playroomCam);
            workshopTutorialParent.SetActive(false);
            playroomTutorialParent.SetActive(true);
            nextButtonText.text = nextButtonFinalText;
        }
        if (step == 1)
        {
            cameraSwap.SwapTo(workshopCam);
            progressHandler.enabled = true;
            Destroy(gameObject);
        }

        step++;
    }
}

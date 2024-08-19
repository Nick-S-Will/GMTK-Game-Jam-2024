using Cinemachine;
using System.Linq;
using UnityEngine;

public class RoomCameraSwap : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] roomCameras;

    public void SwapTo(CinemachineVirtualCamera camera)
    {
        if (!roomCameras.Contains(camera))
        {
            Debug.LogWarning($"Given {nameof(camera)} \"{camera.name}\" is not in the {nameof(roomCameras)} array");
            return;
        }

        foreach (var cam in roomCameras) cam.enabled = false;
        camera.enabled = true;
    }
}

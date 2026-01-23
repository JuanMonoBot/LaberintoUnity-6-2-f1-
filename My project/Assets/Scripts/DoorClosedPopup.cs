using UnityEngine;

public class DoorClosedPopup : MonoBehaviour
{
    [TextArea]
    public string message = "La puerta está cerrada";

    public UI_Menu ui;    

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (ui != null)
            ui.ShowDoorPopup(message);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (ui != null)
            ui.HideDoorPopup();
    }
}


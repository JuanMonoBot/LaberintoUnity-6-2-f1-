using UnityEngine;

public class SwitchPopup : MonoBehaviour
{
    [Header("Mensajes")]
    [TextArea]
    public string message = "Abrir puerta (E)";
    [TextArea]
    public string openedMessage = "Puerta abierta";

    [Header("Referencia doorSwich")]
    public DoorSwitch doorSwitch;  // referencia al DoorSwitch de la puerta asociada

    private UI_Menu ui;  // referencia al script UI_Menu
    //private bool isOpen = false;
    private bool playerInside = false;

    private void Start()
    {
        ui = Object.FindFirstObjectByType<UI_Menu>();
    }

    // Llamado desde el script Door al abrir la puerta
    public void OnDoorOpened()
    {
        if (ui != null && playerInside)
        {
            ui.ShowDoorPopup(openedMessage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (!other.CompareTag("Player")) return;
        
        playerInside = true;

        if (ui == null) return;
        
        bool opened = (doorSwitch != null && doorSwitch.IsActivated);
        string msg = opened ? openedMessage : message;
        ui.ShowDoorPopup(msg);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (ui != null)
            ui.HideDoorPopup();
    }
}

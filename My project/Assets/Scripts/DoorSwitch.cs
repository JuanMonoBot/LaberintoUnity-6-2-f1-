using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    [Header("Puerta asociada")]
    public Door door;                      // arrastra la puerta que abre
       

   [Header("Boton de Activación")]
    public Transform buttonTransform;  // referencia al transform del botón
    public float pressDistance = 0.1f;  // distancia que se mueve el botón al ser presionado
    public float pressTime = 0.2f;      // tiempo que tarda en moverse el botón

    [Header("Popup del interruptor")]
    public SwitchPopup switchPopup;        // referencia al script SwitchPopup asociado

    private bool isActivated = false;
    public bool IsActivated => isActivated;
    private UI_Menu ui;                     // referencia al script UI_Menu

    private Vector3 buttonInitialLocalPos;
    private Coroutine pressRoutine;

    private void Start()
    {
        if (buttonTransform != null)
        {
            buttonInitialLocalPos = buttonTransform.localPosition;
        }

        // Aseguramos que el popup esté en modo cerrado al iniciar
        if (switchPopup != null)
        {
            switchPopup.doorSwitch = this;
        }
    }


    public void Activate()
    {
        if (isActivated) return;

        isActivated = true;

        if (door != null)
            door.Open();

        if (buttonTransform != null)
        {
            if (pressRoutine != null)
                StopCoroutine(pressRoutine);
            pressRoutine = StartCoroutine(PressAnimation());
        }
                    
        // Actualizamos el popup del interruptor
        if (switchPopup != null)
        {
            switchPopup.doorSwitch = this;
        }
    }

    public void ResetSwitch()
    {
        isActivated = false;

        if (buttonTransform != null)
        {
            if (pressRoutine != null)
                StopCoroutine(pressRoutine);
            buttonTransform.localPosition = buttonInitialLocalPos;
        }

        // Regresamos el popup del interruptor a modo cerrado
        if (switchPopup != null)
        {
            switchPopup.OnDoorOpened();
        }

    }

    private System.Collections.IEnumerator PressAnimation()
    {
        Vector3 start = buttonInitialLocalPos;
        Vector3 end = buttonInitialLocalPos + new Vector3(0f, 0f, -pressDistance);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / pressTime;
            buttonTransform.localPosition = Vector3.Lerp(start, end, t);
            yield return null;
        }
        
        buttonTransform.localPosition = end;

    }

}

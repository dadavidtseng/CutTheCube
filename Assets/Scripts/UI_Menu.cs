using UnityEngine;
using UnityEngine.InputSystem;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private Transform           head;
    [SerializeField] private float               spawnDistance = 2;
    [SerializeField] private GameObject          menu;
    [SerializeField] private InputActionProperty showButton;

    private void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);

            menu.transform.position =
                head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }
}
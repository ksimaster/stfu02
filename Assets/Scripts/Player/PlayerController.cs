using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
   
    [Header("Hand")]
    //Objets à gérer via le controlleur
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform hand;
    public GameObject itemInHand;
    
    //Gestion des lancers
    public float force;
    [SerializeField] private AudioClip throwSFX;
    [SerializeField] private AudioSource source;
    
    [Header("Camera")]
    //Gestion de la mobilité de la caméra
    public float sensitivityX = 1f;
    public float sensitivityY = 1f;
    [SerializeField] private float XAxisClamp = 85f;
    [SerializeField] private float YAxisClamp = 85f;
    
    [Header("UIManagement")]
    //Gestion des input de l'UI
    public FoodCustomerUI _foodCustomerUI;
    public GameObject PauseMenu;
    public GameObject RecipeMenu;
    public ClockDigital Clock;
    public AudioSource musicSource;

    private int test = 0;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 targetRotation = playerCamera.localEulerAngles;
        
        //Limite l'angle en X selon les paramètres
        targetRotation.x = ClampAngleAsNegative(targetRotation.x, YAxisClamp);
        //Limite l'angle en Y selon les paramètres
        targetRotation.y = ClampAngleAsNegative(targetRotation.y, XAxisClamp);
        //Supprime toute rotation en Z
        targetRotation.z = 0;
        //Applique la rotation limitée à la caméra
        playerCamera.localEulerAngles = targetRotation;
        
    }
    private float ClampAngleAsNegative(float angle, float clamp)
    {
        //Si l'angle est supérieur à 180°, soustraire 360° pour obtenir son équivalent négatif
        angle = (angle > 180) ? angle - 360 : angle;
        
        //Limite l'angle à l'intervalle entre les valeurs négatives et positives du Clamp
        angle = Mathf.Clamp(angle, -clamp, clamp);
        return angle;
    }

    #region CAMERA
    public void OnLookX(InputValue value)
    {

        playerCamera.Rotate(Vector3.up,value.Get<float>()*sensitivityX);
    }
    
    public void OnLookY(InputValue value)
    {

        playerCamera.Rotate(Vector3.left,value.Get<float>()*sensitivityY);
    }
    #endregion
    
    
    #region INTERACTION
    public void OnInteract()
    {

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 10f))
        {
            
            GameObject interactObj = hit.collider.gameObject;
            //Debug.Log(interactObj.name);
            if (interactObj.CompareTag("Pickup"))
            {
                
                PickUpObject(interactObj);
            }
 
            if (interactObj.CompareTag("Interactable"))
            {
                interactObj.GetComponent<Interactable>().Interact();
            }
            
            if(interactObj.CompareTag("YataiBoard") && itemInHand != null)
            {
                PutOnYatai(hit);
            }
        }
    }

    /*public void OnAddRamen()
    {
        test++;
        _foodCustomerUI.AddRamen("test"+test);
    }

    public void OnRemoveRamen()
    {
        _foodCustomerUI.RemoveRamen("test");
        test--;
    }*/

    public void OnOpenCloseMenu()
    {
        if (PauseMenu.activeSelf && !Clock.IsGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            if (!RecipeMenu.activeSelf)
            {
                musicSource.Play();
            }
        }
        else
        {
            if (!RecipeMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                PauseMenu.SetActive(true);
                musicSource.Pause();
            }
        }
    }

    public void OnOpenCloseRecipe()
    {
        if (RecipeMenu.activeSelf && !Clock.IsGameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            RecipeMenu.SetActive(false);
            if (!PauseMenu.activeSelf)
            {
                musicSource.Play();
            }
        }
        else
        {
            if (!PauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0.0001f;
                RecipeMenu.SetActive(true);
                musicSource.Pause();
            }
        }
    }
    
    public void PickUpObject(GameObject obj)
    {
        if (itemInHand == null)
        {
            try
            {
                obj.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            catch
            {
                return;
            }
            
            obj.transform.parent = hand;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.rotation = Quaternion.Euler(Vector3.zero);
            itemInHand = obj;
        }
    }


    #endregion

    #region THROW

    public void OnShoot()
    {
        if (itemInHand == null)
            return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, 3f))
        {
            if(hit.collider.CompareTag("YataiBoard"))
            {
                PutOnYatai(hit);
                return;
            }
        }
        try {
            Drop(); 
            itemInHand.GetComponent<Rigidbody>().AddForce(playerCamera.forward * force, ForceMode.Impulse); 
            source.PlayOneShot(throwSFX); 
            itemInHand = null;
        }
        catch{}
    }

    private void PutOnYatai(RaycastHit _hit)
    {
        Drop();
        itemInHand.transform.position = _hit.point;
        itemInHand = null;
        
    }
    private void Drop()
    {
        hand.transform.DetachChildren();
        itemInHand.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    #endregion
    
    public void OnDrawGizmos()
    {
        // Draws a 5 unit long red line in front of the object
        Gizmos.color = Color.red;
        Vector3 direction = playerCamera.forward * 3;
        Gizmos.DrawRay(transform.position, direction);
    }
}

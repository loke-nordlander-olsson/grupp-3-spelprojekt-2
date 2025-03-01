using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Port : MonoBehaviour
{
    [SerializeField] private string Inhabitant;
    [SerializeField] private IslandersListData islanders;
    private List<string> otherRecipients = new List<string>();

    private PlayerInputActions inputActions;
    private InputAction interact;
    
    private InventoryController inventoryController;
    private Player player;

    //[SerializeField, Tooltip("quest shipment or smth dunno yet")] private Shipment shipment;
    public InventoryItem item;

    private bool hasBeenPickedUp;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inventoryController = FindObjectOfType<InventoryController>();
        player = FindObjectOfType<Player>();
        foreach (var islander in islanders.islanders)
        {
            if (islander != Inhabitant)
            {
                otherRecipients.Add(islander);
            }
        }
    }

    private void OnEnable()
    {
        interact = inputActions.Player.Interact;
        interact.Enable();
    }

    private void OnDisable()
    {
        interact.Disable();
    }

    private void OnTriggerStay(Collider other)
    {
        //if (interact.WasPressedThisFrame())
        //{
        //    Pickup();
        //}
    }

    void Pickup()
    {
        Instantiate(item.gameObject);
        item.Set();
        if (inventoryController.InsertItem(item))
        {
            Debug.Log("added package");
            hasBeenPickedUp = true;
        }
        else
        {
            Debug.Log("Failed to insert, probably full");
            Destroy(item.gameObject);
        }
           
    }

    bool Delivery()
    {
        bool success = false;
        List<InventoryItem> toRemove = new List<InventoryItem>();
        foreach (var item in player.items)
        {
            if (item.packageData.recipient == Inhabitant)
            {
                toRemove.Add(item);
                inventoryController.mainGrid.RemoveItem(item);
                success = true;
            }
        }

        foreach (var item in toRemove)
        {
            player.items.Remove(item);
        }

        return success;
    }
}

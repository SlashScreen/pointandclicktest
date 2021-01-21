using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI")]
    [Tooltip("Options wheel")]
    public optionspanel opt; //options wheel
    [Tooltip("Inventory drawer")]
    public DrawerScript drawer; //UI drawer
    public List<drawerItem> inventory = new List<drawerItem>(); //list of item IDs for inventory

    JSONItemParser JSON; //json for item reading
    PlayerMain main;


    private void Start()
    {
        main = GetComponent<PlayerMain>();
        JSON = GetComponent<JSONItemParser>();
        main.d.dia.AddCommandHandler("AddItem",AddItem);
        main.d.dia.AddCommandHandler("RemoveItem",RemoveItem);
        main.d.dia.AddCommandHandler("CombineItems",CombineItems);
        main.d.dia.AddFunction("seeIfHasItem", 1 , ItemInInventory);
        main.d.dia.AddFunction("TryCombine", 2 , ItemUsedOnItem);
    }

    //Inventory management fucntions
    drawerItem FindItemWithID(int id){
        //find and returns item in inventory with item id of id
        if (inventory.Exists (x => x.id == id)){ //if there is an item in the list with an id that matches id
            return inventory[inventory.FindIndex(x => x.id == id)]; //return the item
        }
        return new drawerItem(); //if not found, return the default one
    }

    drawerItem NewItem(int id){
        //creates item with id id
        if (JSON == null){
            JSON = GetComponent<JSONItemParser>();
            JSON.Start();
        }

        foreach(var di in JSON.drawer.items){ //loop through item config JSON, find object in there with same ID as the inventory
            //nts could be a lambda predicante
            if(di.id == id){//if id matches
                return di; // return
            }
        }
        return new drawerItem(); //if nothing found, returns item id 0; 0 is nothing
    }
    

    public void AddItem(string[] item){
        //Adds item of ID item into inventory
        inventory.Add(NewItem(int.Parse(item[0]))); //calls function with int argument from string 
        drawer.updateInventory(inventory); //updates
    }

    public void RemoveItem(string[] item){ //POTENTIAL ISSUE: if there is no item of ID in inventory it errors out
        //removes item of ID item
        inventory.Remove(NewItem(int.Parse(item[0]))); //calls function with int argument from string 
        drawer.updateInventory(inventory); //updates
    }

    public object ItemInInventory(Yarn.Value[] item){ 
        //Yarn wrapper for findItemWithID. sets variable based on whether it exists
        return inventory.Exists (x => x.id == item[0].AsNumber);
    }

    public bool ItemInInventory_NoYarn(int item){
        return inventory.Exists (x => x.id == item);
    }

    public void CombineItems(string[] items){ 
        //Combines first 2 strings into item 3 and adds to inventory
        RemoveItem(new string[] {items[0]});
        RemoveItem(new string[] {items[1]});
        AddItem(new string[] {items[2]});
    }

    public object ItemUsedOnItem(Yarn.Value[] items){ //if these 2 items are combined
        int selected = Mathf.FloorToInt(main.d.dia.GetComponent<Yarn.VariableStorage>().GetValue("$selectedInventory").AsNumber); //item in hand
        int combine = Mathf.FloorToInt(main.d.dia.GetComponent<Yarn.VariableStorage>().GetValue("$toCombine").AsNumber); //item clicked
        int item1 = Mathf.FloorToInt(items[0].AsNumber);
        int item2 = Mathf.FloorToInt(items[1].AsNumber);

        if ((selected == item1 && combine == item2) || (selected == item2 && combine == item1)){ //there has to be a better way to do this. maybe like, find in list or something, idk
            return true;
        }else{
            return false;
        }
    }
}

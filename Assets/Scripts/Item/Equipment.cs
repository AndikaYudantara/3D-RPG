using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;
    public SkinnedMeshRenderer skinnedMesh;
    public MeshRenderer mesh;
    public EquipmentMeshRegions[] coveredMeshRegions;
    public int armorModifier;
    public int damageModifier;

    public override void Use()
    {
        base.Use();

        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { Head, Chest, Gloves, Legs, Boots, MainHand, OffHand }
public enum EquipmentMeshRegions { Legs, Arm, Torso };
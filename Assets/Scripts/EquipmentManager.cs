using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singelton

    public static EquipmentManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    public delegate void OnEquipmentChange(Equipment newItem, Equipment oldItem);
    public OnEquipmentChange onEquipmentChange;
    public SkinnedMeshRenderer targetMesh;
    public Transform rightHand, leftHand;
    public Equipment[] defaultItems;

    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentSkinnedMeshes;
    MeshRenderer[] currentMeshes;
    Inventory inventory;

    void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numSlots];
        currentSkinnedMeshes = new SkinnedMeshRenderer[numSlots];
        currentMeshes = new MeshRenderer[numSlots];

        EquipDefaultItems();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            UnequipAll();
        }
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = Unequip(slotIndex);

        if (onEquipmentChange != null)
        {
            onEquipmentChange.Invoke(newItem, oldItem);
        }

        SetEquipmentBlendShape(newItem, 100);

        currentEquipment[slotIndex] = newItem;

        if (newItem.skinnedMesh != null)
        {
            SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.skinnedMesh);
            newMesh.transform.parent = targetMesh.transform;

            newMesh.bones = targetMesh.bones;
            newMesh.rootBone = targetMesh.rootBone;

            currentSkinnedMeshes[slotIndex] = newMesh;
        }

        if (newItem.mesh != null)
        {
            if (slotIndex == 5)
            {
                MeshRenderer newMesh = Instantiate<MeshRenderer>(newItem.mesh);
                newMesh.transform.parent = rightHand;
                newMesh.transform.localPosition = new Vector3(1.49011609e-10f, -7.45058015e-10f, -2.98023217e-10f);
                newMesh.transform.localEulerAngles = new Vector3(11.3028774f, -1.52365546e-05f, 272.406464f);
                newMesh.transform.localScale = new Vector3(1, 1, 1);

                currentMeshes[slotIndex] = newMesh;
            }

            if (slotIndex == 6)
            {
                MeshRenderer newMesh = Instantiate<MeshRenderer>(newItem.mesh);
                newMesh.transform.parent = leftHand;
                newMesh.transform.localPosition = new Vector3(-1.49011609e-10f, -1.26659871e-09f, 1.19209287e-09f);
                newMesh.transform.localEulerAngles = new Vector3(11.3028717f, 1.04479232e-05f, 87.5935211f);
                newMesh.transform.localScale = new Vector3(1, 1, 1);

                currentMeshes[slotIndex] = newMesh;
            }
        }
    }

    public Equipment Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            if (currentSkinnedMeshes[slotIndex] != null)
            {
                Destroy(currentSkinnedMeshes[slotIndex].gameObject);
            }

            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }

            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            SetEquipmentBlendShape(oldItem, 0);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChange != null)
            {
                onEquipmentChange.Invoke(null, oldItem);
            }
            return oldItem;
        }
        return null;
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        EquipDefaultItems();
    }

    void EquipDefaultItems()
    {
        foreach (Equipment item in defaultItems)
        {
            Equip(item);
        }
    }

    void SetEquipmentBlendShape(Equipment item, int weight)
    {
        foreach (EquipmentMeshRegions blendshape in item.coveredMeshRegions)
        {          
            targetMesh.SetBlendShapeWeight((int)blendshape, weight);
        }
    }


}

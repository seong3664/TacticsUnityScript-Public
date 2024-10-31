using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Save_Slot : MonoBehaviour
{
    public TMP_Text[] slotText;
    private Button[] slotButton = new Button[4];
    [SerializeField]
    bool[] savefile = new bool[4];
    private void Awake()
    {
        for (int i = 0; i < slotText.Length; i++)
        {
            slotButton[i] = slotText[i].transform.parent.GetComponent<Button>();
            //slotButton[i].onClick.RemoveAllListeners();
            //slotButton[i].onClick.AddListener(() => { Slot(i); });
        }
    }
    private void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            if (File.Exists(Data_Manager.Instance.path + $"{i}" +".json"))    // �����Ͱ� �ִ� ���
            {
                savefile[i] = true;         // �ش� ���� ��ȣ�� bool�迭 true�� ��ȯ
                Data_Manager.Instance.nowSlot = i;   // ������ ���� ��ȣ ����
                Data_Manager.Instance.LoadUnits(false);    // �ش� ���� ������ �ҷ���
                slotText[i].text = $"Slot{Data_Manager.Instance.nowSlot}";
            }
            else    // �����Ͱ� ���� ���
            {
                savefile[i] = false;
                slotText[i].text = "No data";
            }
        }
        Data_Manager.Instance.DataClear();  
    }
    public void Slot(int number)	
    {
        Data_Manager.Instance.nowSlot = number;
        if (savefile[number])
        {
            Data_Manager.Instance.LoadUnits(true);	
            
            
        }
        else
        {
            Data_Manager.Instance.SaveUnitData();
            savefile[number] = true;
            slotText[number].text = $"Slot{Data_Manager.Instance.nowSlot}";
        }
    }
    public void DeleteSavedData(int slot)
    {
        string filePath = Data_Manager.Instance.path + slot.ToString() + ".json";

        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            slotText[slot].text = "No data";
            savefile[slot] = false;
            Debug.Log($"���� {slot}�� ���� �����Ͱ� �����Ǿ����ϴ�.");
        }
        else
        {
        }
    }
}

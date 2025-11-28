using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationProgressUI : MonoBehaviour
{
    [System.Serializable]


    public class SimulationItem
    {
        public string simulationName;
        public TextMeshProUGUI label;
        public Image checkbox;
    }

    [Header("Lista de simulaciones")]
    public SimulationItem[] items;

    void Start()
    {
               UpdateUI();
    }

    public void UpdateUI()
    {
        for (int i = 0; i < items.Length; i++)
        {
            int done = PlayerPrefs.GetInt("Simulation_" + i, 0);
            items[i].checkbox.gameObject.SetActive(done == 1);

            if (items[i].label != null)
                items[i].label.text = items[i].simulationName;

            if (items[i].checkbox != null)
                items[i].checkbox.gameObject.SetActive(done == 1);
        }
    }
}



using Helper;
using Model.Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Singleton. Responsible for loading the scenarios from file. 
 * 
 */
namespace Managers
{
    public class ScenarioManager : MonoBehaviour
    {
        //MAKE INSTANCE
        private static ScenarioManager _instance;

        private Scenario[] _scenarios;

        private int _selectedScenarioIndex;
        //END MAKE INSTANCE

        public TextAsset ScenarioData; // temp, will prob move scenario loading into its own class

        public static ScenarioManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ScenarioManager>();
                return _instance;
            }
        }

        private void Start()
        {
            _scenarios = JsonHelper.GetJsonArray<Scenario>(ScenarioData.text);
        }

        public Scenario GetSelectedScenario()
        {
            return _scenarios[_selectedScenarioIndex];
        }

        public void StartScenario(int index)
        {
            if (index < _scenarios.Length)
            {
                _selectedScenarioIndex = index;
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.LogError(string.Format("No scenario with index {0}", index));
            }
        }
    }
}
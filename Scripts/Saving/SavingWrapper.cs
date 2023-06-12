using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "new save file";

        private void Awake()
        {
           GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.K))
            {
                Save();
            }

            if (Input.GetKey(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKey(KeyCode.O))
            {
                Debug.Log("Saving file was deleted.");
                Delete();
            }
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}


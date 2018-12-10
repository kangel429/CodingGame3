using UnityEngine;
using UnityEngine.UI;
using System.IO;
namespace DynamicCSharp.Demo
{
    /// <summary>
    /// Responsible for the tank demo gameplay.
    /// </summary>
    public sealed class TankManager : MonoBehaviour
    {
        // Private
        private ScriptDomain domain = null;
        private Vector2 startPosition;
        private Quaternion startRotation;
        
        private const string newTemplate = "BlankTemplate";
        private const string exampleTemplate = "ExampleTemplate";

        // Public
        /// <summary>
        /// The shell prefab that tanks are able to shoot.
        /// </summary>
        public string nextStage;
        public GameObject bulletObject;
        /// <summary>
        /// The tank object that can be controlled via code.
        /// </summary>
        public GameObject tankObject;
        public Sprite playBuSprite;
        public Image buttonPlayimg;

        public bool stopScript;


        CtrManagerObject ctrManagerObject;

        public GameObject stopObject;

        public void StopScriptButton(){
            stopObject.SetActive(true);
            stopObject.transform.position = tankObject.transform.position;
            Debug.Log("dlwp1111111111");
            //stopScript = !stopScript;
            //crash = !crash;
        }


        // Methods
        /// <summary>
        /// Called by Unity.
        /// </summary>
        public void Awake()
        {
            GameObject.Find("ErrorShowLine").GetComponent<Image>().color = new Color(0, 0, 0, 0.3f);
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            stopObject.SetActive(false);
            //ctrManagerObject = tankObject.GetComponent<CtrManagerObject>();
            //if(ScriptDomain.Active== null){

            //    Debug.Log("333333333333");
            //}

            // Create our script domain
            domain = ScriptDomain.CreateDomain("ScriptDomain", true);

            // Find start positions
            startPosition = tankObject.transform.position;
            startRotation = tankObject.transform.rotation;

            // Add listener for new button
            CodeUI.onNewClicked += (CodeUI ui) =>
            {
                //Debug.Log("ddddddddd");


                // Load new file
                ui.codeEditor.text = Resources.Load<TextAsset>(newTemplate).text;
            };

            // Add listener for example button
            CodeUI.onLoadClicked += (CodeUI ui) =>
            {
                // Load example file
                ui.codeEditor.text = Resources.Load<TextAsset>(exampleTemplate).text;
            };

            CodeUI.onCompileClicked += (CodeUI ui) =>
            {
                //ctrManagerObject.mouseDrag = false;
                // Try to run the script
                RunTankScript(ui.codeEditor.text);
            };

        }
        //bool firstStart = true;

        private void Update()
        {
            //if(ctrManagerObject.mouseDrag)
            //{
            //    Debug.Log("mouse drag1111111111");
            //    startPosition = new Vector2(3,-3);
            //    //ctrManagerObject.mouseDrag = false;
            //}


        }


        /// <summary>
        /// Resets the demo game and runs the tank with the specified C# code controlling it.
        /// </summary>
        /// <param name="source">The C# sorce code to run</param>
        public void RunTankScript(string source)
        {
            if (tankObject != null)
            {

                // Strip the old controller script
                TankController old = tankObject.GetComponent<TankController>();

                if (old != null)
                    Destroy(old);

            // Reposition the tank at its start position
                RespawnTank();

            }
            // Compile the script
            ScriptType type = domain.CompileAndLoadScriptSource(source);

            if (type == null)
            {
                Debug.Log("Compile failed" + domain.GetErrorLineValue());
               // Debug.LogError("Compile failed");
                return;
            }else{
                GameObject.Find("ErrorExplain").GetComponent<Text>().text ="";
                GameObject.Find("ErrorShowLine").GetComponent<Image>().color = new Color(0, 0, 0, 0f);
            }

            // Make sure the type inherits from 'TankController'
            if (type.IsSubtypeOf<TankController>() == true && tankObject != null)
            {

                // Attach the component to the object
                ScriptProxy proxy = type.CreateInstance(tankObject);

                // Check for error
                if(proxy == null)
                {
                    // Display error
                    Debug.LogError(string.Format("Failed to create an instance of '{0}'", type.RawType));
                    return;
                }
                // Assign the bullet prefab to the 'TankController' instance
                proxy.Fields["playBuSprite"] = playBuSprite;
                proxy.Fields["buttonPlayimg"] = buttonPlayimg;
                proxy.Fields["bulletObject"] = bulletObject;
                proxy.Fields["nextStage"] = nextStage;
                // Call the run tank method   crash
                //proxy.Fields["crash"] = crash;
                proxy.Call("RunTank");
            }
            else
            {
                //if(tankObject != null){
                //    Debug.LogError("The script must inherit from 'TankController'");
                //}

            }
        }
        bool crash = false;
        /// <summary>
        /// Resets the tank at its starting location.
        /// </summary>
        public void RespawnTank()
        {
            tankObject.transform.position = startPosition;
            tankObject.transform.rotation = startRotation;

        }

        public Text script;


        public void SaveTextScript(){

            //File.WriteAllText(savePath,script.text);
            WriteData(script.text);
            Debug.Log("저장");
            //if (File.Exists(fileName))
            //{
            //    Debug.Log(fileName + " already exists.");
            //    return;
            //}
            //var sr = File.CreateText(fileName);
            //sr.WriteLine(script.text);
           
            //sr.Close();
            ////////

         }
        string source = ""; //읽어낸 텍스트 할당받는 변수

        void WriteData(string strData)
        {
            // FileMode.Create는 덮어쓰기.
            FileStream f = new FileStream(Application.dataPath + "/DynamicCSharp" + "/" + "/Demo" + "/" + "/Resources" + "/" + "Script.txt", FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(f, System.Text.Encoding.Unicode);
            writer.WriteLine(strData);
            writer.Close();
        }

        public void ReadData()
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/DynamicCSharp" + "/" + "/Demo" + "/" + "/Resources" + "/" + "Script.txt");
            source = sr.ReadLine();
            sr.Close();
        }

    }
}

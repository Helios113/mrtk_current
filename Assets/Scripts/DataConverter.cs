#if (UNITY_EDITOR) 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DataConverter : MonoBehaviour
{
    public Object[] dataFolders;
    string subfolder;
    string target;
    string[] files;
    int cnt = 0;
    // Start is called before the first frame update
    public void make()
    {
        foreach (Object folder in dataFolders)
        {
            //get all files in that folder
            files = System.IO.Directory.GetFiles(AssetDatabase.GetAssetPath(folder));
            cnt = PickFolder();
            subfolder = "data_" + cnt;
            
            AssetDatabase.CreateFolder("Assets/Resources", subfolder); //creates new subfolder in resources 
            target = string.Join("/", "Assets/Resources", subfolder) +"/"; //creates new subfolder path 

            List<string> fileNames = new List<string>();

            foreach (string file in files)
            {
                string a = file.Replace("\\", "/");
                string fileName = System.IO.Path.GetFileNameWithoutExtension(a);
                if (System.IO.Path.GetExtension(a) != ".vtu")
                {
                    continue;
                }
                fileNames.Add(subfolder + "/" + fileName); // record file names
                AssetDatabase.CopyAsset(a, target + "/" + fileName + ".xml"); //copy assets with txt extension
            }
            //create config file
            string payload = string.Join(",", fileNames);
            TextAsset ta = new TextAsset(payload);
            AssetDatabase.CreateAsset(ta, string.Format("Assets/Resources/config_{0}.asset", cnt));
        }

    }

    int PickFolder()
    {
        string[] folders = System.IO.Directory.GetDirectories("Assets/Resources");
        int max = int.MinValue;
        int num = 0;
        for(int i =0; i<folders.Length;i++)
        {
            if (folders[i].Contains("data_"))
            {
                string temp = folders[i].Split('/','\\').Last().Replace("data_", "");
                print(temp);
                num = int.Parse(temp);
                if (max < num)
                {
                    max = num;
                }
            }
        }
        return num+1;
    }
}
#endif
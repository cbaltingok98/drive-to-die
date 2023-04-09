using System;
using System.Collections.Generic;
using System.IO;
using _project.Scripts.Berk;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Mimic
{
    [Serializable]
    public class MimicData
    {
        public List<Vector3> moveForce;
        public List<float> joystickDirection;

        public int moveForceIndex;
        public int joystickIndex;

        public MimicData()
        {
            moveForce = new List<Vector3>();
            joystickDirection = new List<float>();
            
            moveForceIndex = 0;
            joystickIndex = 0;
        }

        public MimicData(MimicData newData)
        {
            moveForce = newData.moveForce;
            joystickDirection = newData.joystickDirection;

            moveForceIndex = 0;
            joystickIndex = 0;
        }
    }
    
    public static class MimicDriving
    {
        private static MimicData mimicData;
        private static bool initialized;

        private static string fullPath;
        private const string Directory = "/DrivingData/";
        private const string FileName = "data";
        private const string FileNumber = "1";
        private const string FileExtension = ".txt";
        private const string CD = "/";

        private static string dataRootPath;
        
        public static void Init()
        {
            if (initialized) return;
            
            initialized = true;

            mimicData = new MimicData();
            dataRootPath = Application.streamingAssetsPath + Directory + GameManager.Instance.GetLevelName() + CD;
            
            fullPath = dataRootPath + GameManager.Instance.GetGridIndex() + CD;
            
            if (System.IO.Directory.Exists(fullPath)) return;
            
            System.IO.Directory.CreateDirectory(fullPath);
        }

        public static void SaveMoveForce(Vector3 moveForce)
        {
            Init(); 
            mimicData.moveForce.Add(moveForce);
        }

        public static void SaveJoystickDirection(float joystickDirection)
        {
            Init();
            mimicData.joystickDirection.Add(joystickDirection);
        }

        public static void Save()
        {
            var json = JsonUtility.ToJson(mimicData);
            WriteFile(json);
        }

        public static MimicData Load(int gridPos)
        {
            Init();
            
            const int randFile = Constants.MimicDataFolderAmount; // TODO fix, get based on difficulty then random

            string path = dataRootPath + gridPos + CD + FileName + randFile + FileExtension;
            
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var obj = JsonUtility.FromJson(text, typeof(MimicData));

                mimicData = (MimicData)obj;
                
                return (MimicData)obj;
            }
            
            Printer.Print("AI data file doesn't exist : " + (path), DesiredColor.Red);
            return null;
        }

        public static void Test(MimicData a)
        {
            if (a == mimicData)
            {
                Printer.Print("They ar the same");
            }
            else
            {
                Printer.Print("Different");
            }
        }

        private static void WriteFile(string text)
        {
            string path = dataRootPath + GameManager.Instance.GetGridIndex() + CD;

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);   
            }

            if (File.Exists(path + FileName + FileNumber + FileExtension))
            {
                File.Delete(path + FileName + FileNumber + FileExtension);   
            }

            File.WriteAllText(path + FileName + FileNumber + FileExtension, text);
            
            Printer.Print("DRIVE DATA SAVED TO : " + path, DesiredColor.Green);
        }
    }
}
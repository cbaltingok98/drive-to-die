using System;
using _project.Scripts.Berk;
using UnityEngine;

namespace _project.Scripts.Data
{
    public static class SaveLoadSystem
    {
        private const string AnalyticUserIdKey = "PlayerSaveDataKey";
        
        public static void Save(PlayerData data)
        {
            try
            {
                var json = JsonUtility.ToJson(data);
                var encryptedJson = Hasher.DoMeFavor(json);
                PlayerPrefs.SetString(AnalyticUserIdKey, encryptedJson);
            }
            catch (Exception e)
            {
                string message = "Encrypted player JSON saving failed: + " + e.Message;
                Debug.Log(message);
            }
        }
        
        
        public static PlayerData Load()
        {
            var json = PlayerPrefs.GetString(AnalyticUserIdKey);

            if (string.IsNullOrEmpty(json))
            {
                return GenerateNewPlayer();
            }

            var decryptedJson = Hasher.ThankYou(json);

            try
            {
                //return JsonConvert.DeserializeObject<Player>(decryptedJson) ?? GenerateNewPlayer();
                return JsonUtility.FromJson<PlayerData>(decryptedJson) ?? GenerateNewPlayer();
            }
            catch (Exception e)
            {
                string message = "Decrypted player JSON mapping failed: " + e.Message;
                Debug.Log(message);
            }
            return GenerateNewPlayer();
        }
        
        
        private static PlayerData GenerateNewPlayer()
        {
            var newPlayer = new PlayerData();
            Save(newPlayer);
            return newPlayer;
        }
    }
}
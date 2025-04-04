using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public class TemperatureManager : MonoBehaviour
{
    public static TemperatureManager Instance;

    [System.Serializable]
    private class TemperatureData
    {
        public string tag;
        public double distance;
    }

    private readonly Dictionary<string, double> _TemperatureRecords = new()
    {
        {"Stove", double.MaxValue},
        {"Pot", double.MaxValue},
        {"WarmPot", double.MaxValue},
        {"Soup", double.MaxValue}
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 跨场景存活
        StartCoroutine(SendDataCoroutine());
    }

    public void UpdateTemperatureData(string tag, double newDistance)
    {
        if (_TemperatureRecords.ContainsKey(tag) && newDistance < _TemperatureRecords[tag])
            _TemperatureRecords[tag] = newDistance;
    }

    private IEnumerator SendDataCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);

            // 创建副本以避免枚举期间修改
            var recordsCopy = new List<KeyValuePair<string, double>>(_TemperatureRecords);

            foreach (var record in recordsCopy)
            {
                if (record.Value == double.MaxValue) continue;

                TemperatureData data = new()
                {
                    tag = record.Key,
                    distance = record.Value
                };
                string json = JsonUtility.ToJson(data);

                using (UnityWebRequest www = new UnityWebRequest("http://192.168.43.60:8080/apis/temperature", "GET")) 
                {
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                    www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    www.downloadHandler = new DownloadHandlerBuffer();
                    www.SetRequestHeader("Content-Type", "application/json");

                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                        Debug.LogError($"HTTP Error: {www.error}");
                    else
                        Debug.Log("Temperature data successfully sent");
                }
            }

            // 重置数据
            foreach (var key in new List<string>(_TemperatureRecords.Keys))
                _TemperatureRecords[key] = double.MaxValue;
        }
    }
}

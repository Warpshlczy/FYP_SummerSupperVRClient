using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

[DisallowMultipleComponent]
public class ScentManager : MonoBehaviour
{
    public static ScentManager Instance;
    
    [System.Serializable]
    private class ScentData
    {
        public string tag;
        public double distance;
    }

    private readonly Dictionary<string, double> _scentRecords = new()
    {
        {"BokChoy", double.MaxValue},
        {"Tomato", double.MaxValue},
        {"Onion", double.MaxValue},
        {"PorkBelly", double.MaxValue}
    };

    void Awake()
    {
        // 场景持久化实现‌:ml-citation{ref="2,4" data="citationList"}
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); // 跨场景存活‌:ml-citation{ref="2,4" data="citationList"}
        StartCoroutine(SendDataCoroutine());
    }

    public void UpdateScentData(string tag, double newDistance)
    {
        if (_scentRecords.ContainsKey(tag) && newDistance < _scentRecords[tag])
            _scentRecords[tag] = newDistance;
    }

    private IEnumerator SendDataCoroutine()
{
    while (true)
    {
        yield return new WaitForSeconds(2f);

        // 创建副本以避免枚举期间修改
        var recordsCopy = new List<KeyValuePair<string, double>>(_scentRecords);

        foreach (var record in recordsCopy)
        {
            if (record.Value == double.MaxValue) continue;

            ScentData data = new()
            {
                tag = record.Key,
                distance = record.Value
            };
            string json = JsonUtility.ToJson(data);

            using (UnityWebRequest www = new UnityWebRequest("http://192.168.43.60:8080/apis/foodSmell", "GET"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError($"HTTP Error: {www.error}");
                else
                    Debug.Log("S数据成功发送");
            }
        }

        // 重置数据（保持不变，已安全）
        foreach (var key in new List<string>(_scentRecords.Keys))
            _scentRecords[key] = double.MaxValue;
    }
}
}

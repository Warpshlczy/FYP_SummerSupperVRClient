using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureTrack : MonoBehaviour
{
    // Start is called before the first frame update
    private TemperatureManager _TemperatureManager;
    private Transform _vrCamera;
    void Start()
    {
          // 获取VR主视角摄像机
        _vrCamera = Camera.main?.transform;
        if (_vrCamera == null)
            Debug.LogError("Can't find a VR camera");
        
        // 获取温度管理器单例
       _TemperatureManager =TemperatureManager.Instance;
        if ( _TemperatureManager == null)
            Debug.LogError("场景中缺少温度管理器对象");

        // 验证预制体标签有效性
        if (!System.Array.Exists(new[] { "Stove", "Pot", "WarmPot", "Soup" }, 
            element => element == gameObject.tag))
            Debug.LogError($"无效预制体标签: {gameObject.tag}");
    }

    // Update is called once per frame
    void Update()
    {
         {
        if ( _TemperatureManager == null || _vrCamera == null) return;        
        // 计算三维空间距离（米）‌
        double distance = Vector3.Distance(transform.position, _vrCamera.position);
        _TemperatureManager.UpdateTemperatureData(gameObject.tag, distance);
    }
    }
}

using UnityEngine;

[RequireComponent(typeof(Transform))]
public class FoodSmellTrack : MonoBehaviour
{
    private ScentManager _scentManager;
    private Transform _vrCamera;

    void Start()
    {
        // 获取VR主视角摄像机
        _vrCamera = Camera.main?.transform;
        if (_vrCamera == null)
            Debug.Log("Can't find a VR camera");
        
        // 获取气味管理器单例
        _scentManager = ScentManager.Instance;
        if (_scentManager == null)
            Debug.Log("场景中缺少气味管理器对象");

        // 验证预制体标签有效性
        if (!System.Array.Exists(new[] { "BokChoy", "Tomato", "Onion", "PorkBelly" }, 
            element => element == gameObject.tag))
            Debug.Log($"无效预制体标签: {gameObject.tag}");
    }

    void Update()
    {
        if (_scentManager == null || _vrCamera == null) return;
        
        // 计算三维空间距离（米）‌
        double distance = Vector3.Distance(transform.position, _vrCamera.position);
        _scentManager.UpdateScentData(gameObject.tag, distance);
    }
}

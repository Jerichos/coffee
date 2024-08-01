using UnityEngine;

namespace POLYGONWARE.Coffee.Beans
{
[CreateAssetMenu(fileName = "New_SupportedBeans", menuName = "data/New Supported Beans", order = 0)]
public class SupportedBeansSO : ScriptableObject
{
    [SerializeField] private BeanSO[] _supportedBeans;
    public BeanSO[] SupportedBeans => _supportedBeans;
}
}
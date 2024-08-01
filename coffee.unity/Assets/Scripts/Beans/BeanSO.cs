using UnityEngine;

namespace POLYGONWARE.Coffee.Beans
{
[CreateAssetMenu(fileName = "New_Bean", menuName = "data/new Bean", order = 0)]
public class BeanSO : ScriptableObject
{
    public string ID;
    public string Name;
    [TextArea] public string Description;
    public uint RequiredLevel;
    public double DropRate;
    public double Duration;
    public double V1;
}
}
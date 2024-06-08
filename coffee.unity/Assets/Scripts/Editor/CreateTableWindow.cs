using System;
using System.Collections;
using System.Collections.Generic;
using POLYGONWARE.Coffee.CoffeeGenerators;
using UnityEditor;

namespace POLYGONWARE.Coffee.Editor
{
public class CoffeeGeneratorDataEditorWindow : GenericTableEditorWindow
{
    private List<CoffeeGeneratorData> coffeeGeneratorDataList = new List<CoffeeGeneratorData>();

    protected override Type StructType => typeof(CoffeeGeneratorData);
    protected override IList DataList => coffeeGeneratorDataList;

    [MenuItem("Window/Coffee Generator Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<CoffeeGeneratorDataEditorWindow>("Coffee Generator Data Editor");
    }
}
}
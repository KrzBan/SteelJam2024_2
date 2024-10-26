using UnityEngine;

public class GeneraterGrid : MonoBehaviour
{
    public GameObject blockGameObject;

    private int worldX = 10;
    private int worldZ = 10;

    private int offset = 1;
    private void Start()
    {
        for (int x = 0; x < worldX; x++)
        {
            for (int z = 0; z < worldZ; z++)
            {
                Vector3 pos = new Vector3(x * offset, 0, z * offset);

                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                block.transform.SetParent(this.transform);
            }
        }
    }
}

using UnityEngine;

namespace Tag_System
{
    [CreateAssetMenu(fileName = "Tags", menuName = "Tags")]
    public class Tags : ScriptableObject
    {
        [SerializeField] private string[] tags;

        public string[] GetTags()
        {
            return tags;
        }
    }
}
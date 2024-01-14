using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public class HeartContainer : MonoBehaviour
    {
        public HeartContainer Next { get; private set; }

        [Range(0, 1)] [SerializeField] private float fill;
        [SerializeField] private Image fillImage;

        public void SetHeart(float count)
        {
            fill = count;
            fillImage.fillAmount = fill;
            count--;
            if (Next != null) Next.SetHeart(count);
        }

        public void SetNext(HeartContainer container)
        {
            Next = container;
        }
    }
}
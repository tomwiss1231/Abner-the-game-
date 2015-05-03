using UnityEngine;

namespace Assets.Scripts.Util.Interfaces
{
    public interface IFloatingTextPositioner
	{
        bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size);
	}
}

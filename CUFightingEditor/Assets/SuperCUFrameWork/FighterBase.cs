using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : MonoBehaviour
{
	[SerializeField]
	private AnimationPlayer animationPlayer = null;
	public AnimationPlayer AnimationPlayerCompornent
	{
		get { return animationPlayer; }
	}
	[SerializeField]
	private FighterStatus status = null;
	[SerializeField]
	private PlayerSkill playerSkill = null;
	[SerializeField]
	private AnimationClip nowPlayAnimation = null;

	#region ギズモ
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if(status != null)
		{
			if (PlayerSkillEditorParameter.instance.window.previewCharacter == animationPlayer.gameObject)
			{
				//アニメーションのフラグとか見てやる
				Gizmos.color = Color.green;
				//Gizmos.DrawWireCube(transform.position + status.headHitBox.localPosition + PlayerSkillEditorParameter.instance.window.playerSkill., status.headHitBox.size);
				Gizmos.DrawWireCube(transform.position + status.bodyHitBox.localPosition, status.bodyHitBox.size);
				Gizmos.DrawWireCube(transform.position + status.footHitBox.localPosition, status.footHitBox.size);
			}
			else
			{
				//アニメーションのフラグとか見てやる
				Gizmos.color = Color.green;
				Gizmos.DrawWireCube(transform.position + status.headHitBox.localPosition, status.headHitBox.size);
				Gizmos.DrawWireCube(transform.position + status.bodyHitBox.localPosition, status.bodyHitBox.size);
				Gizmos.DrawWireCube(transform.position + status.footHitBox.localPosition, status.footHitBox.size);
			}
		}
	}
#endif
	#endregion
}

using System.Linq;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Buffs;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Warrior : Util.Abstract.Soldier
    {
        [PunRPC]
        public void WarriorSpecial()
        {
            if (IsAttacking || !SkillBar.DecSkillPoints(10)) return;
            FloatingText.Show("Special!!!", "PlayerSpecialText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
        }

        public override void SpecialHit(int oponId)
        {
            Util.Abstract.Soldier enemy = _player.GetOpponent().GetSoldiers().Where(s => s.Id == oponId).ElementAt(0);;
            GridManager.instance.Special.onClick.RemoveAllListeners();
            photonView.RPC("WarriorSpecial", PhotonTargets.Others);

            if (IsAttacking || !SkillBar.DecSkillPoints(10)) return;
            FloatingText.Show("Special!!!", "PlayerSpecialText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));

            int damage = CalHit();
            if (CheckIfCritical())
                damage *= CriticalHit;
            damage *= SpecialHitParameter;

            enemy.photonView.RPC("AtkDamage", PhotonTargets.All, damage);
            enemy.photonView.RPC("StopHitAni", PhotonTargets.All);
            photonView.RPC("EndAtkAni", PhotonTargets.All);
            if (!enemy.GetHealth().IsAlive()) enemy.photonView.RPC("Die", PhotonTargets.All);
        }

        public override void FillSkillBar()
        {
            SkillBar.AddSkillPoints(8);
        }

        public override void ActivateBuffCast()
        {
            InAttackRange = true;
        }

        [PunRPC]
        public void WorriorBuff(int teamSoldierId)
        {
            Util.Abstract.Soldier teamSoldier = _player.GetSoldiers().Where(s => s.Id == teamSoldierId).ElementAt(0); ;
            WarCryBuff warCryBuff = new WarCryBuff();
            warCryBuff.DoBuff(teamSoldier);
            Buffs.Add(warCryBuff);
            EndOfTurnAction();
        }

        public override void BuffAction(int teamSoldierId)
        {
            photonView.RPC("WorriorBuff", PhotonTargets.All, teamSoldierId);
        }

        protected override string BuffName()
        {
            return "War Cry";
        }

        protected override void Init()
        {
            base.Init();
            Buff = new WarCryBuff();
        }
    }
}
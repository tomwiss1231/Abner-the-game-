using System.Linq;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using Assets.Scripts.Util.Buffs;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Behaviour.Soldier
{
    public class Mage : Util.Abstract.Soldier
    {

        [PunRPC]
        public void MageSpecial()
        {
            if (IsAttacking || !SkillBar.DecSkillPoints(30)) return;
            FloatingText.Show("Special!!!", "PlayerSpecialText",
            new FromWorldPointTextPositioner(Camera.main, transform.position, 2f, 60f));
        }

        public override void SpecialHit(int oponId)
        {
            Util.Abstract.Soldier enemy = _player.GetOpponent().GetSoldiers().Where(s => s.Id == oponId).ElementAt(0);
            GridManager.instance.Special.onClick.RemoveAllListeners();
            photonView.RPC("MageSpecial", PhotonTargets.Others);

            if (IsAttacking || !SkillBar.DecSkillPoints(30)) return;
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
           SkillBar.AddSkillPoints(20);
        }

        public override void ActivateBuffCast()
        {
            foreach (Util.Abstract.Soldier soldier in _player.GetSoldiers())
            {
                soldier.InAttackRange = true;
                soldier.Position.ChangeToBuff();
                AddObserver(soldier);
            }
        }

        [PunRPC]
        public void MageBuff(int teamSoldierId)
        {
            Util.Abstract.Soldier teamSoldier = _player.GetSoldiers().Where(s => s.Id == teamSoldierId).ElementAt(0);
            MageBuff buff = new MageBuff();
            buff.DoBuff(teamSoldier);
            EndOfTurnAction();
        }

        public override void BuffAction(int teamSoldierId)
        {
            photonView.RPC("MageBuff", PhotonTargets.All, teamSoldierId);
        }

        protected override string BuffName()
        {
            return "Heal";
        }
    }
}
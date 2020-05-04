using Homebrew;
using UniRx;
using UnityEngine;

namespace TowerFight
{
    public class ActorPlayer : ActorBase
    {
        private DataPlayer dataPlayer;
        private RangeUnit tower;
                
        protected override void OnStart()
        {
            dataPlayer = GetData<DataPlayer>();
        }
                
        public void StartBattle()
        {
            if (dataPlayer)
                dataPlayer = GetData<DataPlayer>();


            if (dataPlayer.dataTower.prefab)
            {
                tower = dataPlayer.Tower = ManagerUnits.SpawnTower(dataPlayer.dataTower, dataPlayer);

                var spell = tower.GetData<DataSpell>();
                var dataTower = tower.GetData<DataUnit>();
                if (dataPlayer.Player && spell && dataTower)
                {
                    var managerUi = Toolbox.Get<ManagerUI>();
                    managerUi.BattleButtonSpawn(dataPlayer);
                    managerUi.spell.StartCountdown(spell.countdown);
                    managerUi.OnButtonClick.Where(x => x.buttonPressed == ButtonPressed.spell).Subscribe(
                        delegate
                        (UICallBack uICallBack)
                        {
                            dataTower.OnSpellCast.OnNext(tower);
                        }).AddTo(this);

                }
                else if (!dataPlayer.Player)
                {
                    if (GetData<DataAi>().difficult == Difficult.normal)
                    dataPlayer.Tower.GetData<DataUnit>().OnSetDamage.Subscribe(delegate (Unit unit)
                    {
                        dataPlayer.Tower.GetData<DataUnit>().OnSpellCast.OnNext(dataPlayer.Tower);
                    }).AddTo(this);
                }
            }

        }
    }
}
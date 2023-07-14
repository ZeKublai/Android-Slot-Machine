using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Machine_Spin : Slot_Machine_State {
        public float spin_time;
        public override void start(Slot_Machine slot_machine){
                slot_machine.sfx_sources["Spin Start"].Play();
                slot_machine.ui_handler.stop_button.gameObject.SetActive(true);

                // Reset spin time.
                spin_time = 0;

                // Loop through reels and start spinning.
                for(int i = 0; i < slot_machine.reels.Length; i++){
                        slot_machine.reels[i].spin();
                }

                // Set current bet and deduct coins.
                slot_machine.current_bet = slot_machine.player.get_total_bet(slot_machine.player.bet_step);
                slot_machine.player.coins -= slot_machine.current_bet;
                slot_machine.ui_handler.check_disabled();
                slot_machine.player.wins = 0;
        }
        public override void execute(Slot_Machine slot_machine){

                // Exit on max spin time.
                spin_time += Time.deltaTime;
                if(spin_time > slot_machine.spin_time_max){
                        slot_machine.set_state(slot_machine.stop_state);
                }
        }
        public override void end(Slot_Machine slot_machine){
                slot_machine.ui_handler.stop_button.gameObject.SetActive(false);
        }
}

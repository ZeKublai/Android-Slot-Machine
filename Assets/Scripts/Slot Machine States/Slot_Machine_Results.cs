using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Machine_Results : Slot_Machine_State {
        public int total_payouts;
        public float results_time;
        public override void start(Slot_Machine slot_machine){

                // Gather results.
                slot_machine.player.wins = Mathf.Clamp(slot_machine.get_total_payouts()*slot_machine.current_bet, 0, int.MaxValue);

                // Add winnings to the total coins.
                slot_machine.player.coins = Mathf.Clamp(slot_machine.player.coins + slot_machine.player.wins, 0, int.MaxValue);

                // No effects if no lines.
                results_time = 0.3f;
                if(slot_machine.line_results.Count > 0){

                        // Set results effects.
                        slot_machine.lines.gameObject.SetActive(true);
                        slot_machine.main_light.gameObject.SetActive(false);
                        if(slot_machine.line_results.Count > 1){
                                results_time = 0.5f;
                        }

                        // Play SFX
                        slot_machine.sfx_sources["Fanfare"].Play();
                        slot_machine.ui_handler.trumpets.gameObject.SetActive(true);
                }
        }
        public override void execute(Slot_Machine slot_machine){

                // Give short pause before next spin.
                results_time = results_time - Time.deltaTime;
                if(results_time <= 0f){
                        slot_machine.set_state(slot_machine.idle_state);
                }
        }
        public override void end(Slot_Machine slot_machine){
                slot_machine.ui_handler.check_disabled();
        }
}

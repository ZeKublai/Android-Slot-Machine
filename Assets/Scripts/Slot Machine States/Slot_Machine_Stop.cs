using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Machine_Stop : Slot_Machine_State {
        private int reel_size;
        private int reel_index;
        private int target_index;
        private Transform target_transform;
        public override void start(Slot_Machine slot_machine){
                slot_machine.ui_handler.spin_button.interactable = false;
                reel_index = 0;

                // Generate random target symbols list.
                for(int i = 0; i < slot_machine.reels.Length; i++){

                        // Roll for target result.
                        reel_size = slot_machine.reels[i].symbol_config.Length;
                        target_index = Random.Range(0, reel_size);
                        slot_machine.target_symbols[i] = slot_machine.reels[i].symbol_config[target_index];

                        // Add result to the spin results using MOD to loop if the value is negative.
                        for(int j = -1; j < Slot_Machine.COL_SIZE - 1; j++){
                                slot_machine.spin_results[i, j + 1] = slot_machine.reels[i].symbol_config[(reel_size + target_index + j)%reel_size];
                        }
                }

                // Set start transform.
                set_target_transform(slot_machine);
        }
        public override void execute(Slot_Machine slot_machine){

                // Stop reel if it is spinning.
                if(slot_machine.reels[reel_index].get_state() == slot_machine.reels[reel_index].spin_state){

                        // Stop if target value is not set or if target value is near.
                        if(target_transform == null || (
                                target_transform.localPosition.y > slot_machine.reels[reel_index].get_reel_unit()*2f &&
                                target_transform.localPosition.y < slot_machine.reels[reel_index].get_reel_unit()*3f
                        )){
                                slot_machine.reels[reel_index].stop();
                                slot_machine.sfx_sources["Reel Stop"].Play();
                        }
                }

                // Go to next reel if idle state has reached.
                if(slot_machine.reels[reel_index].get_state() == slot_machine.reels[reel_index].idle_state){
                        reel_index = reel_index + 1;

                        // Go back to idle state once all reels have stopped.
                        if(reel_index >= slot_machine.reels.Length){
                                slot_machine.set_state(slot_machine.results_state);
                        }
                        else{

                                // Get next target transform.
                                set_target_transform(slot_machine);
                        }
                }
        }
        public void set_target_transform(Slot_Machine slot_machine){

                // Set to null if target does not exist.
                if(reel_index > slot_machine.target_symbols.Length - 1){
                        target_transform = null;
                        return;
                }

                // Find target transform.
                for(int i = 0; i < slot_machine.reels[reel_index].symbol_config.Length; i++){
                        if(slot_machine.reels[reel_index].symbol_config[i] == slot_machine.target_symbols[reel_index]){
                                target_transform = slot_machine.reels[reel_index].symbol_transforms[i];
                                return;
                        }
                }

                // Default to null if target symbol is not in reel.
                target_transform = null;
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel_Stop : Reel_State {
        public float lerp_progress;
        public Vector3 target_position;
        public Vector3[] target_positions;
        public override void start(Reel reel){

                // Reset lerp progress.
                lerp_progress = 0f;

                // Set target positions.
                target_positions = new Vector3[reel.symbol_transforms.Length];
                for(int i = 0; i < reel.symbol_transforms.Length; i++){
                        target_positions[i] = new Vector3(0f, reel.symbol_transforms[i].localPosition.y - (reel.symbol_transforms[i].localPosition.y % reel.get_reel_unit()), 0f);

                        // Check if the symbol has reached the bottom position
                        if(reel.symbol_transforms[i].localPosition.y <= 0){

                                // Add the the position for the looping and retain the decimals.
                                target_positions[i] = new Vector3(
                                        0f, reel.get_reel_unit()*(reel.symbol_transforms.Length - 1), 0f
                                );
                                reel.symbol_transforms[i].localPosition = new Vector3(
                                        0f, reel.symbol_transforms[i].localPosition.y + (reel.get_reel_unit()*(reel.symbol_transforms.Length)), 0f
                                );
                        }
                }
        }
        public override void execute(Reel reel){

                // Set progress.
                lerp_progress = Mathf.Min(1f, lerp_progress + reel.slot_machine.stop_speed*Time.deltaTime);

                // Update the position of each symbol
                for(int i = 0; i < reel.symbol_transforms.Length; i++){
                        target_position = target_positions[i];

                        // Check if the symbol has reached the bottom position
                        reel.symbol_transforms[i].localPosition = Vector3.Lerp(reel.symbol_transforms[i].localPosition, target_position, lerp_progress);
                }

                // Once lerp is complete proceed to idle state.
                if(lerp_progress >= 1f){
                        reel.set_state(reel.idle_state);
                        return;
                }
        }
}

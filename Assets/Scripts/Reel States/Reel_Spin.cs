using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel_Spin : Reel_State {
        public float spin_speed;
        public override void start(Reel reel){
                spin_speed = Random.Range(reel.slot_machine.min_reel_speed, reel.slot_machine.max_reel_speed);
        }
        public override void execute(Reel reel){

                // Update the position of each symbol
                for(int i = 0; i < reel.symbol_transforms.Length; i++){
                        reel.symbol_transforms[i].localPosition = new Vector3(0f, reel.symbol_transforms[i].localPosition.y - (spin_speed * Time.deltaTime), 0f);

                        // Check if the symbol has reached the bottom position
                        if(reel.symbol_transforms[i].localPosition.y <= 0 - reel.get_reel_unit()){

                                // Add the the position for the looping and retain the decimals.
                                reel.symbol_transforms[i].localPosition = new Vector3(
                                        0f, reel.symbol_transforms[i].localPosition.y + (reel.get_reel_unit()*(reel.symbol_transforms.Length)), 0f
                                );
                        }
                }
        }
}

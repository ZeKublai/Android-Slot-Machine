using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Machine_Idle : Slot_Machine_State {
        private float light_tilt = 30f;
        private float line_cooldown = 1f;
        private int line_index;
        private LineRenderer line_renderer;
        public override void start(Slot_Machine slot_machine){
                slot_machine.ui_handler.check_disabled();

                // End effects if no lines.
                if(slot_machine.line_results.Count <= 0){
                        end_result_effects(slot_machine);
                        return;
                }

                // Set next line.
                line_index = 0;
                line_cooldown = 1f;
                next_line(slot_machine);
        }
        public override void execute(Slot_Machine slot_machine){
                if(slot_machine.sfx_sources["Fanfare"].isPlaying == false){
                        slot_machine.ui_handler.trumpets.gameObject.SetActive(false);
                }                

                // Do nothing if no lines exist..
                if(slot_machine.line_results.Count <= 0){
                        return;
                }

                // Cooldown line timer.
                line_cooldown = line_cooldown - Time.deltaTime;
                if(line_cooldown <= 0){
                        line_cooldown = 1f;
                        line_index = line_index + 1;
                        if(line_index > slot_machine.line_results.Count - 1){
                                line_index = 0;
                        }
                        next_line(slot_machine);
                }
        }
        public override void end(Slot_Machine slot_machine){
                end_result_effects(slot_machine);
        }
        public void next_line(Slot_Machine slot_machine){

                // Disable all lines except for current index.
                if(slot_machine.show_lines == false){
                        for(int i = 0; i < slot_machine.line_objects.Length; i++){
                                slot_machine.line_objects[i].SetActive(false);
                        }
                }
                slot_machine.line_results[line_index].SetActive(true);

                // Point spot lights at the results.
                line_renderer = slot_machine.line_results[line_index].GetComponent<LineRenderer>();
                for(int i = 1; i < line_renderer.positionCount - 1; i++){
                        slot_machine.spot_lights[i - 1].eulerAngles = new Vector3(
                                -1*(slot_machine.config_results[line_index].line_config[i - 1]*light_tilt - light_tilt), 0f, 0f
                        );
                        slot_machine.spot_lights[i - 1].gameObject.SetActive(true);
                }
        }
        public void end_result_effects(Slot_Machine slot_machine){

                // Set results effects.
                slot_machine.lines.gameObject.SetActive(false);
                slot_machine.main_light.gameObject.SetActive(true);
                for(int j = 0; j < slot_machine.spot_lights.Length; j++){
                        slot_machine.spot_lights[j].gameObject.SetActive(false);
                }
        }
}

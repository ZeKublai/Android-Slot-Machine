using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Slot_Machine_State {
        public virtual void start(Slot_Machine slot_machine){}
        public virtual void execute(Slot_Machine slot_machine){}
        public virtual void end(Slot_Machine slot_machine){}
}

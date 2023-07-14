using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reel_State {
        public virtual void start(Reel reel){}
        public virtual void execute(Reel reel){}
        public virtual void end(Reel reel){}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

        // Player Data
        public int start_coins = 2000;
        [HideInInspector] public int coins;
        [HideInInspector] public int wins;
        [HideInInspector] public int bet_step;

        // Start is called before the first frame update
        void Start(){
                coins = start_coins;
                bet_step = 1;
                wins = 0;
        }
        public int get_total_bet(int bet_step){
                return Mathf.Clamp(bet_step*20, 0, int.MaxValue);
        }
        public void add_bet(){
                bet_step = bet_step + 1;
        }
        public void subtract_bet(){
                bet_step = Mathf.Max(0, bet_step - 1);
        }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Handler : MonoBehaviour {

        // UI stats.
        private float ui_duration = 0.2f;

        // Cache components.
        [HideInInspector] public Transform _transform;
        [HideInInspector] public Text coin_text;
        [HideInInspector] public Text bets_text;
        [HideInInspector] public Text wins_text;
        [HideInInspector] public Button spin_button;
        [HideInInspector] public Button stop_button;
        [HideInInspector] public Button plus_button;
        [HideInInspector] public Button minus_button;
        [HideInInspector] public Transform trumpets;

        // Cache objects.
        [HideInInspector] public Player player;
        [HideInInspector] public Slot_Machine slot_machine;

        // Cache calculations.
        private float coin_timer;
        private float wins_timer;
        private int coin_value;
        private int wins_value;
        private Vector3 trumpet_scale;
        void Awake(){
                _transform = transform;
                coin_text = _transform.Find("Coins").Find("Value").GetComponent<Text>();

                Transform controls = _transform.Find("Controls");
                spin_button = controls.Find("Spin Button").GetComponent<Button>();
                stop_button = controls.Find("Stop Button").GetComponent<Button>();
                wins_text = controls.Find("Wins").Find("Value").GetComponent<Text>();

                Transform bets = controls.Find("Bets");
                bets_text = bets.Find("Value").GetComponent<Text>();
                minus_button = bets.Find("Minus Button").GetComponent<Button>();
                plus_button = bets.Find("Plus Button").GetComponent<Button>();

                trumpets = _transform.Find("Trumpets");
                trumpet_scale = trumpets.localScale;
                trumpets.gameObject.SetActive(false);

                coin_timer = 0f;
                wins_timer = 0f;
                coin_value = 0;
                wins_value = 0;
        }

        // Start is called before the first frame update
        void Start(){
                player = FindObjectOfType<Player>();
                slot_machine = FindObjectOfType<Slot_Machine>();
                check_disabled();
                bets_text.text = player.get_total_bet(player.bet_step).ToString();
        }

        // Update is called once per frame
        void Update(){

                // Lerp coin timer.
                if(coin_value != player.coins){
                        coin_timer += Time.deltaTime;
                        coin_text.text = ((int)Mathf.Lerp(coin_value, player.coins, coin_timer/ui_duration)).ToString();
                        if(int.Parse(coin_text.text) == player.coins){
                                coin_timer = 0;
                                coin_value = int.Parse(coin_text.text);
                        }
                }

                // Lerp winnings timer.
                if(wins_value != player.wins){
                        wins_timer += Time.deltaTime;
                        wins_text.text = ((int)Mathf.Lerp(wins_value, player.wins, wins_timer/ui_duration)).ToString();
                        if(int.Parse(wins_text.text) == player.wins){
                                wins_timer = 0;
                                wins_value = int.Parse(wins_text.text);
                        }
                }

                // Oscillate trumpet scale
                trumpets.localScale = trumpet_scale + Vector3.one * Mathf.Sin(Time.time * 20f) * 0.1f;
        }
        public void check_disabled(){
                minus_button.interactable = (player.bet_step > 0);
                plus_button.interactable = (player.get_total_bet(player.bet_step + 1) <= player.coins && player.coins > 0);
                spin_button.interactable = (
                        player.get_total_bet(player.bet_step) <= player.coins &&
                        player.coins > 0 && slot_machine.get_state() == slot_machine.idle_state
                );
        }
}

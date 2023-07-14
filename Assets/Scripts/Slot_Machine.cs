using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Machine : MonoBehaviour {

        // Slot Machine Stuff
        public Symbol[] symbols;
        public Slot_Line[] slot_lines;
        public GameObject line_prefab;
        public int REEL_SIZE;
        public const int ROW_SIZE = 5;
        public const int COL_SIZE = 3;
        public float spin_time_max = 5f;
        public float stop_speed = 4f;
        public float min_reel_speed = 30f;
        public float max_reel_speed = 100f;
        [HideInInspector] public Dictionary<string, AudioSource> sfx_sources;
        [HideInInspector] public bool show_lines;
        [HideInInspector] public int current_bet;
        [HideInInspector] public Symbol[] target_symbols;
        [HideInInspector] public Symbol[,] spin_results;
        [HideInInspector] public List<GameObject> line_results;
        [HideInInspector] public List<Slot_Line> config_results;

        // States
        private Slot_Machine_State _state;
        public Slot_Machine_Idle idle_state = new Slot_Machine_Idle();
        public Slot_Machine_Spin spin_state = new Slot_Machine_Spin();
        public Slot_Machine_Stop stop_state = new Slot_Machine_Stop();
        public Slot_Machine_Results results_state = new Slot_Machine_Results();

        // Cache Components
        private Transform _transform;
        [HideInInspector] public Reel[] reels;
        [HideInInspector] public Transform lines;
        [HideInInspector] public GameObject[] line_objects;
        [HideInInspector] public Light main_light;
        [HideInInspector] public Transform[] spot_lights;

        // Cache Objects
        [HideInInspector] public Player player;
        [HideInInspector] public UI_Handler ui_handler;
        private void Awake(){

                // Get components.
                _transform = transform;

                // Get reel and SFX components for initialization.
                reels = _transform.GetComponentsInChildren<Reel>();
                sfx_sources = new Dictionary<string, AudioSource>();
                foreach(AudioSource sfx_source in _transform.GetComponentsInChildren<AudioSource>()){
                        if(sfx_source.clip != null){
                                sfx_sources[sfx_source.clip.name] = sfx_source;
                        }
                }

                // Initialize results array and lists.
                target_symbols = new Symbol[ROW_SIZE];
                spin_results = new Symbol[ROW_SIZE, COL_SIZE];
                line_objects = new GameObject[slot_lines.Length];
                line_results = new List<GameObject>();
                config_results = new List<Slot_Line>();

                // Get all the reference points for the results set in the scene.
                Transform results = _transform.Find("Results");
                Transform reference_points = results.Find("Reference Points");
                lines = results.Find("Lines");
                for(int i = 0; i < slot_lines.Length; i++){

                        // Instantiate line.
                        GameObject line_object = Instantiate(line_prefab, lines.position, Quaternion.identity, lines);
                        line_object.name = slot_lines[i].name;
                        LineRenderer line_renderer = line_object.GetComponent<LineRenderer>();
                        line_renderer.startColor = slot_lines[i].start_color;
                        line_renderer.endColor = slot_lines[i].end_color;

                        // Set line points based on the given reference points and line configuration data.
                        for(int j = -1; j < ROW_SIZE + 1; j++){
                                int config_index = Mathf.Clamp(j, 0, ROW_SIZE - 1);
                                line_renderer.SetPosition(
                                        j + 1,
                                        reference_points.Find(j.ToString()).Find((slot_lines[i].line_config[config_index]).ToString()).position
                                );
                        }

                        // Add line to array.
                        line_objects[i] = line_object;
                }

                // Disable lines at initialization.
                reference_points.gameObject.SetActive(false);
                lines.gameObject.SetActive(false);

                // Get lights.
                main_light = _transform.Find("Lights").Find("Main Light").GetComponent<Light>();
                spot_lights = new Transform[ROW_SIZE];
                for(int j = 0; j < ROW_SIZE; j++){
                        spot_lights[j] = _transform.Find("Lights").Find("Spot Lights").Find("Spot Light " + j);
                        spot_lights[j].gameObject.SetActive(false);
                }

                // Set initial state.
                _state = idle_state;
        }

        // Start is called before the first frame update
        void Start(){
                player = FindObjectOfType<Player>();
                ui_handler = FindObjectOfType<UI_Handler>();
                show_lines = false;

                // Initialize state if not set.
                _state.start(this);
        }

        // Update is called once per frame
        void Update(){

                // Execute state.
                _state.execute(this);
        }
        public void set_state(Slot_Machine_State state){
                this._state.end(this);
                this._state = state;
                this._state.start(this);
        }
        public Slot_Machine_State get_state(){
                return this._state;
        }
        public void toggle(){

                // Stop spinning if spinning.
                if(get_state() == spin_state){
                        stop();
                        return;
                }

                // Start spin
                spin();
        }
        public void spin(){
                set_state(spin_state);
        }
        public void stop(){
                sfx_sources["Bet Press"].Play();
                set_state(stop_state);
        }
        public void plus(){
                sfx_sources["Bet Press"].Play();
                player.add_bet();
                ui_handler.check_disabled();
                ui_handler.bets_text.text = player.get_total_bet(player.bet_step).ToString();
        }
        public void minus(){
                sfx_sources["Bet Press"].Play();
                player.subtract_bet();
                ui_handler.check_disabled();
                ui_handler.bets_text.text = player.get_total_bet(player.bet_step).ToString();
        }
        public void max(){
                sfx_sources["Bet Press"].Play();

                // Start with current bet step.
                int max_bet_step = player.bet_step;
                while(true){

                        // Check if current is higher.
                        if(player.get_total_bet(max_bet_step) > player.coins){

                                // Decrement until valid.
                                max_bet_step--;
                                if(player.get_total_bet(max_bet_step) <= player.coins){
                                        break;
                                }
                        }
                        else{

                                // If valid check next step if increase is possible.
                                if(
                                        player.get_total_bet(max_bet_step + 1) > 0 &&
                                        player.get_total_bet(max_bet_step + 1) <= player.coins
                                ){
                                        max_bet_step++;
                                }
                                else{
                                        break;
                                }
                        }
                }

                // Set bet step.
                player.bet_step = max_bet_step;
                ui_handler.check_disabled();
                ui_handler.bets_text.text = player.get_total_bet(player.bet_step).ToString();
        }
        public void info(){
                sfx_sources["Bet Press"].Play();

                // Toggle
                show_lines = !show_lines;
                lines.gameObject.SetActive(show_lines);
                for(int i = 0; i < line_objects.Length; i++){
                        line_objects[i].SetActive(show_lines);
                }
        }
        public void print_spin_results(){
                int rows = spin_results.GetLength(0);
                int columns = spin_results.GetLength(1);
                for(int i = 0; i < rows; i++){
                        string row_result = string.Empty;
                        for (int j = 0; j < columns; j++){
                                row_result += spin_results[i, j].ToString() + " ";
                        }
                        Debug.Log(row_result);
                }
        }
        public int get_total_payouts(){

                // Initialize total payouts.
                int total_payouts = 0;

                // Initialize symbol counter.
                Dictionary<Symbol, int> symbol_counter = new Dictionary<Symbol, int>();
                line_results.Clear();
                config_results.Clear();

                // Loop through each line.
                for(int i = 0; i < slot_lines.Length; i++){
                        line_objects[i].SetActive(false);

                        // Get line symbols.
                        symbol_counter.Clear();

                        // Get the symbol IDs from the spin results based on the line configuration
                        for(int j = 0; j < slot_lines[i].line_config.Length; j++){
                                Symbol line_symbol = spin_results[j, slot_lines[i].line_config[j]];
                                if(symbol_counter.ContainsKey(line_symbol)){
                                        symbol_counter[line_symbol] += 1;
                                }
                                else{
                                        symbol_counter[line_symbol] = 1;
                                }
                        }

                        // Get the total counter results to check if there are 3 or more duplicates
                        foreach(KeyValuePair<Symbol, int> pair in symbol_counter){

                                // Add to total if greater than 3.
                                if(pair.Value > 0){
                                        total_payouts += pair.Key.payouts[pair.Value - 1];
                                        if(pair.Value > 2){
                                                config_results.Add(slot_lines[i]);
                                                line_results.Add(line_objects[i]);
                                                line_objects[i].SetActive(true);
                                        }
                                }
                        }
                }
                return total_payouts;
        }
}

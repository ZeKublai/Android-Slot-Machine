using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour {

        // Reel Stuff
        public Symbol[] override_symbol_config;
        public GameObject symbol_prefab;
        public float symbol_size = 2.5f;
        public float symbol_spacing = 0.5f;
        public Symbol[] symbol_config;

        // States
        private Reel_State _state;
        public Reel_Idle idle_state = new Reel_Idle();
        public Reel_Spin spin_state = new Reel_Spin();
        public Reel_Stop stop_state = new Reel_Stop();

        // Cache Components
        private Transform _transform;
        [HideInInspector] public Transform[] symbol_transforms;
        [HideInInspector] public SpriteRenderer[] symbol_sprites;

        // Cache Objects
        [HideInInspector] public Slot_Machine slot_machine;
        private void Awake(){
                _transform = transform;

                // Set initial state.
                _state = idle_state;
        }

        // Start is called before the first frame update
        void Start(){

                // Initialilze symbols.
                slot_machine = FindObjectOfType<Slot_Machine>();
                initialize_symbols();

                // Initialize state if not set.
                _state.start(this);
        }

        // Update is called once per frame
        void Update(){

                // Execute state.
                _state.execute(this);
        }
        public void set_state(Reel_State state){
                this._state.end(this);
                this._state = state;
                this._state.start(this);
        }
        public Reel_State get_state(){
                return this._state;
        }
        public void initialize_symbols(){

                // Use override config if set.
                if(override_symbol_config.Length > 0){
                        symbol_config = override_symbol_config;
                }
                else{

                        // Randomize using slot machine config.
                        symbol_config = new Symbol[slot_machine.REEL_SIZE];
                        for(int i = 0; i < slot_machine.REEL_SIZE; i++){
                                symbol_config[i] = slot_machine.symbols[Random.Range(0, slot_machine.symbols.Length)];
                        }
                }

                // Initialize transforms and sprites.
                symbol_transforms = new Transform[symbol_config.Length];
                symbol_sprites = new SpriteRenderer[symbol_config.Length];

                // Initialize game objects.
                for(int i = 0; i < symbol_config.Length; i++){
                        GameObject symbol_object = Instantiate(symbol_prefab, _transform.position, Quaternion.identity, _transform);

                        // Get symbol components
                        symbol_transforms[i] = symbol_object.transform;
                        symbol_sprites[i] = symbol_object.GetComponent<SpriteRenderer>();

                        // Set position and sprite
                        symbol_transforms[i].localPosition += new Vector3(0f, i*get_reel_unit(), 0f);
                        symbol_sprites[i].sprite = symbol_config[i].sprite;
                }
        }
        public float get_reel_unit(){
                return (symbol_size + symbol_spacing);
        }
        public void spin(){
                set_state(spin_state);
        }
        public void stop(){
                set_state(stop_state);
        }
}

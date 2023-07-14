# ANINO EXAM

Welcome! This repository contains my output for my Anino Exam where I made a Slot Machine App. Have fun experimenting!

# System Setup
## Unity Version: 2020.3.48f1

The main classes in the system setup are as follows:
<br>
### Slot_Machine
Description: Handles the entire game flow, simulating a real slot machine.
Responsibilities:
- Manages various states of the slot machine, including idle, results, spin, stop, etc.
- Communicates with Reel objects to retrieve spin results and control spinning behavior.
- Interacts with the Player object to handle coins, bets, and winnings.
- Updates the UI through the UI Handler to display relevant information on the Canvas.

### Reel
Description: Represents an individual reel of the slot machine.
Responsibilities:
- Manages the behavior and state of the reel, including idle, spinning, and stopping.
- Communicates with the Slot_Machine to provide spin results.
- Includes calculations and functions for stopping at a specific target.

### Player
Description: Manages the player-related functionality within the slot machine game.
Responsibilities:
- Handles betting, coins, and winnings for the player.
- Tracks the current amount of coins and the number of wins.
- Provides methods for adjusting the bet amount and calculating the total bet.

### UI Handler
Description: Updates the user interface elements and manages their visibility and content.
Responsibilities:
- Updates the UI values, such as text content, to reflect the current game state.
- Manages the visibility of different UI elements based on the game flow.
- Handles user interactions with the UI, such as button clicks.

<br>

# Data Sources and Editing

The slot machine game utilizes two Scriptable Objects as data sources: Symbols and Slot Lines. These Scriptable Objects allow for easy customization and expansion of the game's symbols and lines configurations. Here's how to edit them:

### Symbols
Contains data for symbols used in the game.
- To create a new symbol, right-click in the project explorer and choose "Create" > "Symbols".
- Customize the symbol's properties such as sprite, payout values, and ID.
- Modify existing symbols by adjusting their properties.

### Slot Lines
Contains data for line configurations and appearance.
- To create a new line, right-click in the project explorer and choose "Create" > "Slot Lines".
- Customize properties such as points of configuration and colors.
- Modify existing lines by adjusting their properties.

## Integration with Reels and Slot Machine:
- Each Reel object in the scene has a Symbol Scriptable Object array, determining the symbol order on the reel.
- The Slot Machine object contains a Slot Line Scriptable Object array, defining the lines used in the game.
- Customizing Symbol and Slot Line Scriptable Objects allows for creating and customizing symbols and lines.
- Scriptable Objects enable potential additions of custom animations, particle effects, and sound effects to symbols and lines.

<br>

# Scalability and Flexibility

### Scalability: 
The slot machine system is designed to be scalable, accommodating various reel sizes. Although the current implementation is limited to a 5x3 configuration, the underlying code structure allows for scalability to larger reel sizes, even to infinity.

### Flexibility: 
The system exhibits flexibility through the use of Scriptable Objects for symbols and lines. This enables easy customization and addition of new symbols and lines, providing flexibility for future expansion and variation in gameplay.

### Modularity: 
The system follows a modular architecture, allowing independent modifications to different components. The Reels, Slot Machine, Player, and UI Handler can be extended or modified without affecting other parts of the system, facilitating flexibility and adaptability.

### Customization: 
The Scriptable Objects and modular design provide flexibility for integrating custom animations, particle effects, and sound effects. These can be easily added to symbols and lines, enhancing the visual and audio experience of the slot machine game.

- The current implementation reflects a 5x3 configuration due to time constraints, but the system's structure and design concepts are adaptable and scalable to accommodate larger reel sizes, additional symbols, and further customizations.

<br>

# Use of MVC (Model-View-Controller)
The slot machine project follows the Model-View-Controller (MVC) pattern, which provides a clear separation of concerns and promotes modular design. Here's an overview of the use of MVC in the project:

### Model: 
The Model component represents the data and business logic of the slot machine game. It includes classes such as Player, Reel, Lines, and Symbol, which encapsulate the game's data, functionality, and rules.

### View: 
The View component handles the user interface (UI) and visual elements of the slot machine game. The UI Handler class is responsible for updating the UI elements based on the data provided by the Model. It ensures that the UI accurately reflects the game state, such as the player's balance, current bets, and winnings.

### Controller: 
The Controller component acts as an intermediary between the Model and the View. The Slot_Machine class serves as the main controller, managing the game flow and coordinating interactions between the Player, Reel, and UI Handler classes. It receives input from the UI and translates it into actions and updates in the Model and View.

By adhering to the MVC pattern, the project achieves separation of concerns, improves maintainability, and allows for easier extensibility and scalability. Changes to the UI, game rules, or underlying data can be implemented without affecting other components, promoting flexibility and reusability.

<br>

# Possible Future Improvements
### Additional Features: Expand the game with bonus rounds, special symbols, and progressive jackpots to enhance gameplay and engagement.
### Animations and Effects: Better visual and audio effects like transitions, particle effects, and sound effects to make gameplay more immersive.
### Localization: Support multiple languages to reach a wider audience by translating UI text and game content.
### Analytics and Metrics: Implement analytics tools to gather player data for game improvement and balancing.

These improvements will enhance the game's appeal, user experience, and longevity. Prioritize based on project goals and player feedback.

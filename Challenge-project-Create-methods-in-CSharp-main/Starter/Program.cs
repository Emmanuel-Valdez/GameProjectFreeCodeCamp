using System;
using System.Runtime.CompilerServices;

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
int playerState = 0;
string player = states[playerState];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit)
{

    shouldExit = TerminalResized();
    if (shouldExit)
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        return;
    }
    Move();
}

// Returns true if the Terminal was resized 
bool TerminalResized()
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);

}

// check that the player has eaten the food
bool NoFoodRemaining()
{
    bool noFood = false;

    for (int i = 0; i < states[playerState].Length; i++)
    {
        for (int j = 0; j < foods[food].Length; j++)
        {
            if (foodX + j == playerX + i && foodY == playerY)
            {
                noFood = true;
                ChangePlayer();
                FreezePlayer();
                return noFood;
            }
        }
    }
    return noFood;
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
}

// Reads directional input from the Console and moves the player
void Move(bool detectionKey = false)
{
    int lastX = playerX;
    int lastY = playerY;

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX--;
            break;
        case ConsoleKey.RightArrow:
            playerX++;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            if (detectionKey)
            {
                Console.Clear();
                Console.WriteLine("Please do not use another keys than arrows or exit, Program exiting");
                shouldExit = true;
                return;
            }
            break;
    }

    // Clear the characters at the previous position
    clearCharacter(player, lastX, lastY);

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);


    if (NoFoodRemaining())
    {
        clearCharacter(foods[food], foodX, foodY);
        ShowFood();
    }

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

}
void clearCharacter(string asset, int x, int y)
{
    Console.SetCursorPosition(x, y);
    for (int i = 0; i < asset.Length; i++)
    {
        Console.Write(" ");
    }
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}
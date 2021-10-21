using System.Collections;
using System.Collections.Generic;

public class InGameSaves {
    private static bool canMove = true;
    private static bool isBusy = false;

    // Change canMove, so player can or cannot move
    public static void ChangeCanMove() {
        canMove = !canMove;
    }

    // Get canMove value
    public static bool GetCanMove() {
        return canMove;
    }

    // Change isBusy, so the game can detect if player is doing something or not
    // This will help to don't open store when crafting or vice-versa
    public static void ChangeIsBusy() {
        isBusy = !isBusy;
    }

    // Get isBusy value
    public static bool GetIsBusy() {
        return isBusy;
    }
}

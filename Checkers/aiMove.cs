namespace Checkers
{
    public class aiMove
    {
        int AI_MAXPLYLEVEL = 2;

        //Offensive
        int WEIGHT_CAPTUREPIECE = 2;
        int WEIGHT_CAPTUREKING = 1;
        int WEIGHT_CAPTUREDOUBLE = 5;
        int WEIGHT_CAPTUREMULTI = 10;

        //Defensive
        int WEIGHT_ATRISK = 3;
        int WEIGHT_KINGATRISK = 4;

        Tree<Move> gameTree;

        public void makeDecisionTree(Cell[,] board)
        {

            gameTree = new Tree<Move>(new Move());
           



        }
    }
}
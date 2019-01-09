namespace Checkers
{
    public class Pawn
    {
        public bool isKing;
        public readonly bool isAI;

        public Pawn(bool isKing, bool isAI)
        {
            this.isKing = isKing;
            this.isAI = isAI;
        }

        /*
         I wanted to add an image rather than a color but don't have 
         time right now  to do research on how, but this is the idea.
         In case you figure it out, the pawn images are already in the resources directory.
         public Brush Color
        {
            get
            {
                var imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Resources/blackKing.png"));
                if (isAI && !isKing)
                {
                    return Brushes.Black;
                }
                if (isAI && isKing)
                {
                    return Brushes. color here ;
                }
                if (!isAI && !isKing)
                {
                    return Brushes. color here;
                }
                if (!isAI && isKing)
                {
                    return Brushes.Red;
                }
                return Brushes.Transparent;
            }
        }*/
    }
}
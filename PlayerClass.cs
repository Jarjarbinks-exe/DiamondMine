using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace trabalho
{
    public class PlayerClass
    {
        private Point position;
        private bool keysReleased = true;
        private Game1 game;

        public Point Position => position; // igual uma funçao para chamar o position
        public PlayerClass(Game1 game1,int x, int y)
        {
            position = new Point(x, y);
            game = game1;
        }
         // AULA 45MIM
        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            if (keysReleased)
            {
                Point lastPosition = position;
                keysReleased = false;
                if (kState.IsKeyDown(Keys.A))
                {
                    position.X--;
                    game.direction = Direction.left;
                }
                else if (kState.IsKeyDown(Keys.W))
                {
                    game.direction = Direction.up;
                    position.Y--;
                }
                else if (kState.IsKeyDown(Keys.S))
                {
                    game.direction = Direction.down;
                    position.Y++;
                }
                else if (kState.IsKeyDown(Keys.D))
                {
                    game.direction = Direction.right;
                    position.X++;
                }
                else keysReleased = true;

                if (game.FreeTile(position.X, position.Y))
                    position = lastPosition;

                if (game.HasSaida(position.X, position.Y))
                {
                    game.aa = true;
                }
                if (game.Hasproximo1(position.X, position.Y))
                {
                    game.ab = true;
                }

                if (game.HasPortal_1(position.X, position.Y))
                {
                    position = new Point(2, 9);
                }
                if (game.HasPortal_2(position.X, position.Y))
                {
                    position = new Point(1, 10);
                }
                if (game.HasDiamante(position.X, position.Y))
                {
                    game.dd = true;
                }
            }
            else
            {
                if (kState.IsKeyUp(Keys.A) && kState.IsKeyUp(Keys.W) &&
                    kState.IsKeyUp(Keys.S) && kState.IsKeyUp(Keys.D))
                {
                    keysReleased = true;
                }
            }
        }


    }
}

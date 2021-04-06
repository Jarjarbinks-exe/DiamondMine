using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;

namespace trabalho
{
    public enum Direction
    {
        up, down, left, right
    }


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private char[,] level;
        public bool aa;
        public bool ab;
        public bool dd;
        public bool tt;
        public int contadorlvl = 0;


        public List<Point> terra_s;
        public List<Point> diamante_s;
        public List<Point> cavado_;
        public List<Point> portal_1;
        public List<Point> portal_2;
        public List<Point> saida_;
        public List<Point> proximo1_;
        public List<Point> _pedra;


        private Texture2D[][] player;
        private Texture2D bomba, pedra, parede, saida, diamante, terra, cavado, portal1, portal2, proximo_nivel;
        int tileSize = 32;
        public PlayerClass personagem;
        private SpriteFont arial_12;
        private string[] levelNames = new[] { "lvl1.txt", "lvl2.txt" };
        public int currentLevel = 0;
        private double leveltime = 0f;
        public float quatidade_diamantes = 0;
        public int live_count = 3;
        private bool rDown = false;
        private bool isWin = false;

        public Direction direction = Direction.down;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            LoadLevel(levelNames[currentLevel]);
            //tirar o fundo e deixa so o jogo
            _graphics.PreferredBackBufferHeight =  tileSize * (1+ level.GetLength(1));
            _graphics.PreferredBackBufferWidth = tileSize * level.GetLength(0);
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            bomba = Content.Load<Texture2D>("tnt");
            pedra = Content.Load<Texture2D>("pedra2");
            parede = Content.Load<Texture2D>("paredepedra");
            saida = Content.Load<Texture2D>("saida2");
            diamante = Content.Load<Texture2D>("diamante");
            terra = Content.Load<Texture2D>("terra");
            portal1 = Content.Load<Texture2D>("saida4");
            proximo_nivel = Content.Load<Texture2D>("saida2");
            portal2 = Content.Load<Texture2D>("saida4");

            arial_12 = Content.Load<SpriteFont>("arial12");

            player = new Texture2D[4][];
            player[(int)Direction.down] = new[] {
                Content.Load<Texture2D>("Character4"),
                Content.Load<Texture2D>("Character5"),
                Content.Load<Texture2D>("Character6") };

            player[(int)Direction.up] = new[] {
                Content.Load<Texture2D>("Character7"),
                Content.Load<Texture2D>("Character8"),
                Content.Load<Texture2D>("Character9") };

            player[(int)Direction.left] = new[] {
                Content.Load<Texture2D>("Character1"),
                Content.Load<Texture2D>("Character10") };

            player[(int)Direction.right] = new[] {

                Content.Load<Texture2D>("Character2"),
                Content.Load<Texture2D>("Character3")};

        }
        private void remove_terra()
        {
            
        }
        protected override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            if ((kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.D) || kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.S)) && HasTerra(personagem.Position.X,personagem.Position.Y) == true)
            {
                for (int x = 0; x < level.GetLength(0); x++)
                {
                    for (int y = 0; y < level.GetLength(0); y++)
                    {
                        if (level[x, y] == '.')
                        {
                               terra_s.Add(new Point(x,y));

                        }
                    }
                } 
                foreach (Point p in terra_s.ToArray())
                {
                    if (personagem.Position.X == p.X && personagem.Position.Y == p.Y)
                    {
                      terra_s.Remove(p);
                        cavado_.Add(p);

                    }
                }

            }
            if ((kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.D) || kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.S)) && HasDiamante(personagem.Position.X, personagem.Position.Y) == true)
            {
                for (int x = 0; x < level.GetLength(0); x++)
                {
                    for (int y = 0; y < level.GetLength(0); y++)
                    {
                        if (level[x, y] == 'D')
                        {
                            diamante_s.Add(new Point(x, y));

                        }
                    }
                }
                foreach (Point p in diamante_s.ToArray())
                {
                    if (personagem.Position.X == p.X && personagem.Position.Y == p.Y)
                    {
                        cavado_.Add(p);
                        diamante_s.Remove(p);

                    }
                }

            }
            foreach (Point p in _pedra.ToArray()) 
                {
                    if (personagem.Position.X == p.X) { }
                    {
                        for (int x = 0; x < level.GetLength(0); x++)
                        {
                            for (int y = 0; y < level.GetLength(0); y++)
                            {
                                if (level[x, y] == 'X')
                                {
                                    _pedra.Add(new Point(x, y));

                                }
                            }
                        }
                    }
                }
    

            foreach (Point p in _pedra.ToArray())
            {
                // se o y+1 tiver espaço cai
                if (hascavado(p.X, p.Y + 1)==true)
                {
                    _pedra.Add(new Point(p.X, p.Y + 1));
                    _pedra.Remove(p);
                }
                // se o y+1 tiver pedra/diamante e o x+1 e x-1, a pedra cai pro x-1/y+1 ou x+1/x+1
                else if (HasDiamante(p.X,p.Y + 1)== true || haspedra(p.X,p.Y + 1) == true)
                {
                    if (hascavado(p.X - 1, p.Y) == true)
                    {
                        _pedra.Add(new Point(p.X - 1, p.Y));
                        _pedra.Remove(p);

                        if (hascavado(p.X - 1, p.Y + 1))
                        {
                            _pedra.Add(new Point(p.X - 1, p.Y + 1));
                            _pedra.Remove(p);
                        }
                    }
                    else if (hascavado(p.X + 1, p.Y))
                    {
                        _pedra.Add(new Point(p.X + 1, p.Y));
                        _pedra.Remove(p);

                        if (hascavado(p.X + 1, p.Y + 1))
                        {
                            _pedra.Add(new Point(p.X + 1, p.Y + 1));
                            _pedra.Remove(p);
                        }
                    }
                }
            }

            if (!isWin) leveltime += gameTime.ElapsedGameTime.TotalSeconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            else if (Keyboard.GetState().IsKeyDown(Keys.R)) rDown = false;

            if (Victory())
            {
                if (Victory())
                {
                    isWin = true;
                }else { Exit(); }
            }
            if (proximo())
            {
                if (currentLevel < levelNames.Length -1 )
                {
                    currentLevel++;
                    Initialize();

                }
            }
            if (dd == true && (kState.IsKeyDown(Keys.A) || kState.IsKeyDown(Keys.D) || kState.IsKeyDown(Keys.W) || kState.IsKeyDown(Keys.S)))
            {
                quatidade_diamantes++;
                dd = false;
            }
            if (!rDown && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                rDown = true;
                live_count--;
                if (isWin || live_count < 0)
                {
                    currentLevel = 0;
                    leveltime = 0f;
                    live_count = 3;
                    quatidade_diamantes = 0;
                    isWin = false;
                    Initialize();
                } 
                else if(live_count>0)
                {
                    live_count--;
                    Initialize();

                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R)) { rDown = false; }

                if (!isWin) personagem.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            Rectangle position = new Rectangle(0, 0, tileSize, tileSize);
            for (int x = 0; x < level.GetLength(0); x++)
            {

                for (int y = 0; y < level.GetLength(0); y++)
                {
                    position.X = x * tileSize;
                    position.Y = y * tileSize;
                    switch (level[x, y])
                    {
                        case ' ':
                            break;
                        case '.':
                            _spriteBatch.Draw(terra, position, Color.White);
                            break;
                        case '#':
                            _spriteBatch.Draw(bomba, position, Color.White);
                            break;
                        case 'X':
                            _spriteBatch.Draw(pedra, position, Color.White);
                            break;
                        case '0':
                            _spriteBatch.Draw(saida, position, Color.White);
                            break;
                        case 'D':
                            _spriteBatch.Draw(diamante, position, Color.White);
                            break;
                        case 'P':
                            _spriteBatch.Draw(parede, position, Color.White);
                            break;
                        case '?':
                            _spriteBatch.Draw(portal1, position, Color.White);
                            break;
                        case '=':
                            _spriteBatch.Draw(portal2, position, Color.White);
                            break;
                        case '1':
                            _spriteBatch.Draw(proximo_nivel, position, Color.White);
                            break;

                    }
                }
            }

            foreach (Point b in terra_s)
            {
                position.X = b.X * tileSize;
                position.Y = b.Y * tileSize;
                _spriteBatch.Draw(terra, position, Color.White);
            }

            position.X = personagem.Position.X * tileSize;
            position.Y = personagem.Position.Y * tileSize;
            _spriteBatch.Draw(player[(int)direction][0], position, Color.White);
            _spriteBatch.DrawString(arial_12, string.Format("timer : {0:f0}              diamonds : {1}              lifes : {2}", leveltime, quatidade_diamantes, live_count)
                , new Vector2(5, level.GetLength(1) * tileSize + 5), Color.Red);

            if (isWin)
            {
                string win = $"demorou {leveltime:f1} segundos para ganhar!";
                Vector2 winMeansures = arial_12.MeasureString(win) / 2f;
                Vector2 windowCenter = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferWidth) / 2;
                Vector2 pos = windowCenter - winMeansures;
                _spriteBatch.DrawString(arial_12, win, pos, Color.LightBlue);
            }
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.End();
            base.Draw(gameTime);

        }


        private bool Victory()
        {
            if (aa==true)
            {
                return true;
            }
            return false;
        }
        private bool proximo()
        {
            if (ab == true)
            {
                return true;
            }
            return false;

        }
        
        

        public bool Hasproximo1(int x, int y)
        {
            foreach (Point b in proximo1_)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasPortal_1(int x, int y)
        {
            foreach (Point b in portal_1)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasSaida(int x, int y)
        {
            foreach (Point b in saida_)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
        public bool HasPortal_2(int x, int y)
        {
            foreach (Point b in portal_2)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasTerra(int x, int y) 
        {
            foreach(Point b in terra_s)
            {
                if (b.X == x && b.Y == y) 
                {
                    return true;
                }
            }
            return false;
        }  
         public bool HasDiamante(int x, int y)
        {
            foreach (Point b in diamante_s)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;
                }
            }

            return false;
        }
        public bool hascavado(int x, int y)
        {
            foreach (Point b in cavado_)
            {
                if (b.X == x && b.Y == y)
                {
                    return true;

                }
            }

            return false;
        }
        public bool haspedra(int x, int y)
        {
            foreach(Point b in _pedra)
            {
                if(b.X==x && b.Y == y){
                    return true;
                }
            }
            return false;
        }

        public bool FreeTile(int x,int y)
        {
            if (level[x, y] == '.') return false;
            if (HasTerra(x, y)) return false; 
            if (level[x, y] == 'D') return false;
            if (HasDiamante(x, y)) return false;
            if (level[x, y] == '?') return false;
            if (HasPortal_1(x, y)) return false;
            if (level[x, y] == '=') return false;
            if (HasPortal_2(x, y)) return false;
            if (level[x, y] == '0') return false;
            if (HasSaida(x, y)) return false;
            if (level[x, y] == '1') return false;
            if (Hasproximo1(x, y)) return false;
            if (level[x, y] == ' ') return false;
            if (hascavado(x, y)) return true;
            if (level[x, y] == 'X') return true;
            if (haspedra(x, y)) return false;
            return true;
        }

        void LoadLevel(string levelFile)
        {

            string[] linhas = File.ReadAllLines($"content/" + levelFile);

            int nlinhas = linhas.Length;
            int ncolunas = linhas[0].Length;
            diamante_s = new List<Point>();
            terra_s = new List<Point>();
            cavado_ = new List<Point>();
            portal_1 = new List<Point>();
            portal_2 = new List<Point>();
            saida_ = new List<Point>();
            proximo1_ = new List<Point>();
            _pedra = new List<Point>();

            level = new char[ncolunas, nlinhas];

            for (int x = 0; x < ncolunas; x++)
            {
                for(int y = 0; y < nlinhas; y++)
                {
                    if(linhas[y][x]== 'Y')
                    {
                        personagem = new PlayerClass(this,x, y);
                        level[x, y] = ' ';
                    } 
                     else if (linhas[y][x] == 'D')
                    {
                        diamante_s.Add(new Point(x, y));
                        level[x, y] = 'D';
                    }
                    else if (linhas[y][x] == '.')
                    {
                        terra_s.Add(new Point(x, y));
                        level[x, y] = ' ';
                    }

                    else if (linhas[y][x] == '?')
                    {
                        portal_1.Add(new Point(x, y));
                        level[x, y] = '?';
                    }
                    else if (linhas[y][x] == '=')
                    {
                        portal_2.Add(new Point(x, y));
                        level[x, y] = '=';
                    }
                    else if (linhas[y][x] == '0')
                    {
                        saida_.Add(new Point(x, y));
                        level[x, y] = '0';
                    }
                    else if (linhas[y][x] == '1')
                    {
                        proximo1_.Add(new Point(x, y));
                        level[x, y] = '1';
                    }
                    else
                    {
                        level[x, y] = linhas[y][x];
                    }
                }
            }


        }

    }
}

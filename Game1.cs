using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Threading;

namespace Froggy
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    
    struct Unit
    {
        public int x;
        public int y;
        public Texture2D pic;
        public int lives;
        public double speed;
    }

    struct Flame
    {
        public int x;
        public int y;
        public Texture2D pic;
        public int index;
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch sb;
        
        Texture2D girl_a;
        Texture2D girl_b;
        Texture2D hurt;
        Texture2D car;
        Texture2D truck;
        Texture2D human;
        Texture2D duck;
        Texture2D pig;
        Texture2D bull;
        Texture2D fire;

        Texture2D win;
        Texture2D gameover;
        Texture2D face1;
        Texture2D face2;
        Texture2D face3;
        Texture2D bridge;
        Texture2D candy;
        Texture2D life;
        Texture2D bg;
        Texture2D[] flames = new Texture2D[6];


        Rectangle bg2;

        SpriteFont font;

        SoundEffect MoveSound;
        SoundEffect HitSound;

        //Song gameSong;

        int W, H;

        Unit player;
        Unit bri;
        Unit sweet;
        Unit live;
        
                
        List<Unit> RockList;
        List<Flame> FlameList;
        int flameTime;
        int smokeTime;

        Random randomGenerator;

        KeyboardState currentKey;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = W = 700;
            graphics.PreferredBackBufferHeight = H = 700;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            RockList = new List<Unit>();
            randomGenerator = new Random();

            FlameList = new List<Flame>();
            flameTime = 0;
            smokeTime = 0;

            player.lives = 3;
            player.speed = 7.0;

            player.x = W / 2;
            player.y = H - 77;


            bri.x = randomGenerator.Next(20, W-100);
            bri.y = 377;
            bri.pic = bridge;
            bri.speed = 0;
            bri.lives = 0;

            sweet.x = randomGenerator.Next(20, W-100);
            sweet.y = 70;
            sweet.pic = candy;
            sweet.speed = 0;
            sweet.lives = 0;


            int bg_h = 650;
            int bg_w = 700;
            bg2 = new Rectangle(0, H - bg_h, bg_w, bg_h);

           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sb = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            girl_a = Content.Load<Texture2D>("Back");
            girl_b = Content.Load<Texture2D>("Back2");
            hurt = Content.Load<Texture2D>("hurt");

            car = Content.Load<Texture2D>("car");
            truck = Content.Load<Texture2D>("truck");
            human = Content.Load<Texture2D>("human");
            duck = Content.Load<Texture2D>("duck");
            pig = Content.Load<Texture2D>("pig");
            bull = Content.Load<Texture2D>("bull");

            bridge = Content.Load<Texture2D>("bridge");
            candy = Content.Load<Texture2D>("candy");
            face1 = Content.Load<Texture2D>("face1");
            face2 = Content.Load<Texture2D>("face2");
            face3 = Content.Load<Texture2D>("face3");
            gameover = Content.Load<Texture2D>("go");
            win = Content.Load<Texture2D>("win");

            font = Content.Load<SpriteFont>("gameFont");

            bg = Content.Load<Texture2D>("bg2");

            MoveSound = Content.Load<SoundEffect>("movesound");
            HitSound = Content.Load<SoundEffect>("hitsound");

            for (int i = 0; i < 6; i++)
            {
                flames[i] = Content.Load<Texture2D>("flame" + i.ToString());
            }

            player.pic = girl_a;

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentKey = Keyboard.GetState();

            if (currentKey.IsKeyDown(Keys.Up))
            {
                player.y = (int)(player.y - player.speed);
                player.pic = girl_b;
                MoveSound.Play();
            }
            else if (currentKey.IsKeyDown(Keys.Down))
            {
                player.y = (int)(player.y + player.speed);
                player.pic = girl_b;
                MoveSound.Play();
            }
            else if (currentKey.IsKeyDown(Keys.Left))
            {
                player.x = (int)(player.x - player.speed);
                player.pic = girl_b;
                MoveSound.Play();
            }
            else if (currentKey.IsKeyDown(Keys.Right))
            {
                player.x = (int)(player.x + player.speed);
                player.pic = girl_b;
                MoveSound.Play();
            }
            else
            {
                player.pic = girl_a;
            }

            player.x = MathHelper.Clamp(player.x, 0, W - player.pic.Width);
            player.y = MathHelper.Clamp(player.y, 50, (H - player.pic.Height - 20));

            flameTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            int spawnChance = randomGenerator.Next(0, 1000);

            if (flameTime % 2000 == 0)
            {
                    // car
                    Unit newRock = new Unit();
                    newRock.x = -250;
                    newRock.y = 272;
                    newRock.pic = car;
                    newRock.speed = 3;
                    newRock.lives = 1;
                    RockList.Add(newRock);
                    
                }

            if (flameTime % 6000 == 0)
            {
                    // truck
                    Unit newRock = new Unit();
                    newRock.x = -200;
                    newRock.y = 200;
                    newRock.pic = truck;
                    newRock.speed = 1;
                    newRock.lives = 2;
                    RockList.Add(newRock);
                    
                }

            if (flameTime % 5000 == 0)
            {
                    // human
                    Unit newRock = new Unit();
                    newRock.x = W;
                    newRock.y = 315;
                    newRock.pic = human;
                    newRock.speed = -1;
                    newRock.lives = 1;                    
                    RockList.Add(newRock);
                    
            }

            if (flameTime % 1400 == 0)
            {
                    // duck
                    Unit newRock = new Unit();
                    newRock.x = W;
                    newRock.y = 580;
                    newRock.pic = duck;
                    newRock.speed = -1;
                    newRock.lives = 1;
                    RockList.Add(newRock);
                    
                }
            if (flameTime % 2200 == 0)
            {
                // pig
                Unit newRock = new Unit();
                newRock.x = 0;
                newRock.y = 505;
                newRock.pic = pig;
                newRock.speed = 1;
                newRock.lives = 2;
                RockList.Add(newRock);

            }

            if (flameTime % 2000 == 0)
            {
                // bull
                Unit newRock = new Unit();
                newRock.x = W;
                newRock.y = 443;
                newRock.pic = bull;
                newRock.speed = -5;
                newRock.lives = 2;
                RockList.Add(newRock);

            }



            if (flameTime % 2000 == 0)
            {
                // car
                Unit newRock = new Unit();
                newRock.x = -250;
                newRock.y = 272;
                newRock.pic = car;
                newRock.speed = 3;
                newRock.lives = 1;
                RockList.Add(newRock);

            }


            // update explosions
            smokeTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            for (int i = FlameList.Count - 1; i >= 0; i--)
            {
                Flame f = FlameList[i];

                if (smokeTime > 30)
                {
                    f.index++;
                    smokeTime = 0;
                }
                if (f.index < 6)
                {
                    f.pic = flames[f.index];
                    FlameList[i] = f;
                }
                else
                {
                    FlameList.RemoveAt(i);
                }

            }


                if (player.lives == 3)
                {
                    live.pic = face3;
                    live.x = 20;
                    live.y = 0;
            }
                else if(player.lives == 2)
                {
                    live.pic = face2;
                    live.x = 20;
                    live.y = 0;

            }
                else if (player.lives == 1)
                {
                    live.pic = face1;
                    live.x = 20;
                    live.y = 0;
            }


            else
            {
                live.pic = gameover;
                live.x = 100;
                live.y = 180;
            }


            // update moving rocks            
            List<Unit> newList = new List<Unit>();
            for (int i = 0; i < RockList.Count; i++)
            {                
                Unit oldRock = RockList[i];
                Unit newRock = new Unit();

                newRock.x = (int)(oldRock.x + oldRock.speed);
                newRock.y = oldRock.y;
                newRock.pic = oldRock.pic;
                newRock.lives = oldRock.lives;
                newRock.speed = oldRock.speed;   
                            
                
                if (isCollide(player, newRock))
                {
                    player.lives = player.lives - newRock.lives;
                    player.pic = hurt;

                    if (newRock.lives > 0)
                    {
                        Flame f = new Flame();
                        f.x = player.x;
                        f.y = newRock.y;
                        f.pic = flames[0];
                        f.index = 0;
                        FlameList.Add(f);
                        HitSound.Play();
                    }
                    //else if (newRock.lives < 0)
                    //{
                    //  itemSound.Play();
                    //}
                    newRock.lives = 0;
                }


                    if (newRock.y < H)
                {
                    newList.Add(newRock);
                }
                
            }
            RockList = newList;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Honeydew);

            // TODO: Add your drawing code here
            sb.Begin();

            sb.Draw(bg, bg2, Color.White);

            sb.Draw(bridge, new Vector2(bri.x, bri.y), Color.White);
            sb.Draw(candy, new Vector2(sweet.x, sweet.y), Color.White);
            sb.DrawString(font, "F   R   O   G   G   Y ", new Vector2(450, 10), Color.HotPink);


            foreach (Unit rock in RockList)
            {
                sb.Draw(rock.pic, new Vector2(rock.x, rock.y), Color.White);
            }

            sb.Draw(player.pic, new Vector2(player.x, player.y), Color.White);

            foreach (Flame flame in FlameList)
             {
               sb.Draw(flame.pic, new Vector2(flame.x, flame.y), Color.White);
            }

                sb.Draw(live.pic, new Vector2(live.x, live.y), Color.White);


            sb.End();

            base.Draw(gameTime);
        }

        static bool isCollide(Unit unit1, Unit unit2)
        {
            Rectangle rect1 = new Rectangle(unit1.x, unit1.y, unit1.pic.Width, unit1.pic.Height);
            Rectangle rect2 = new Rectangle(unit2.x, unit2.y, unit2.pic.Width, unit2.pic.Height);

            if (rect1.Intersects(rect2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}

﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPG
{

    enum Dir
    {
        Down,
        Up,
        Left,
        Right
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D player_Sprite;
        Texture2D playerDown;
        Texture2D playerUp;
        Texture2D playerLeft;
        Texture2D playerRight;

        Texture2D eyeEnemy_Sprite;
        Texture2D snakeEnemy_Sprite;

        Texture2D bush_Sprite;
        Texture2D tree_Sprite;

        Texture2D heart_Sprite;
        Texture2D bullet_Sprite;

        Player player = new Player();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            player_Sprite = Content.Load<Texture2D>("Player/player");
            playerDown = Content.Load<Texture2D>("Player/playerDown");
            playerUp = Content.Load<Texture2D>("Player/playerUp");
            playerLeft = Content.Load<Texture2D>("Player/playerLeft");
            playerRight = Content.Load<Texture2D>("Player/playerRight");

            eyeEnemy_Sprite = Content.Load<Texture2D>("Enemies/eyeEnemy");
            snakeEnemy_Sprite = Content.Load<Texture2D>("Enemies/snakeEnemy");

            bush_Sprite = Content.Load<Texture2D>("Obstacles/bush");
            tree_Sprite = Content.Load<Texture2D>("Obstacles/tree");

            heart_Sprite = Content.Load<Texture2D>("Misc/heart");
            bullet_Sprite = Content.Load<Texture2D>("Misc/bullet");

            player.animations[0] = new AnimatedSprite(playerDown, 1, 4);
            player.animations[1] = new AnimatedSprite(playerUp, 1, 4);
            player.animations[2] = new AnimatedSprite(playerLeft, 1, 4);
            player.animations[3] = new AnimatedSprite(playerRight, 1, 4);

            Enemy.enemies.Add(new Snake(new Vector2(100, 400)));
            Enemy.enemies.Add(new Eye(new Vector2(300, 450)));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            foreach (Projectile proj in Projectile.projectiles)
            {
                proj.Update(gameTime);
            }

            foreach(Enemy en in Enemy.enemies)
            {
                en.Update(gameTime, player.Position);
            }

            foreach(Projectile proj in Projectile.projectiles)
            {
                foreach(Enemy en in Enemy.enemies)
                {
                    int sum = proj.Radius + en.Radius;
                    if(Vector2.Distance(proj.Position, en.Position) < sum)
                    {
                        proj.Collided = true;
                        en.Health--;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided);
            Enemy.enemies.RemoveAll(e => e.Health <= 0);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            player.anim.Draw(spriteBatch, new Vector2(player.Position.X - 48, player.Position.Y - 48));

            spriteBatch.Begin();

            foreach(Enemy en in Enemy.enemies)
            {
                Texture2D spriteToDraw;
                int rad;

                if (en.GetType() == typeof(Snake))
                {
                    spriteToDraw = snakeEnemy_Sprite;
                    rad = 50;
                } else
                {
                    spriteToDraw = eyeEnemy_Sprite;
                    rad = 73;
                }

                spriteBatch.Draw(spriteToDraw, new Vector2(en.Position.X - rad, en.Position.Y - rad), Color.White);
            }

            foreach (Projectile proj in Projectile.projectiles)
            {
                spriteBatch.Draw(bullet_Sprite, new Vector2(proj.Position.X - proj.Radius, proj.Position.Y - proj.Radius), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

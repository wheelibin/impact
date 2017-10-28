using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Factories;
using Impact.Game.Helpers;
using Impact.Game.Layers;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class GameScene : CCScene
    {
        private CCLayerColor _backgroundLayer;
        private CCLayer _gameLayer;
        private CCLayer _hudLayer;
        private NewLevelLayer _newLevelLayer;
        private GameOverLayer _gameOverLayer;
        private LevelCompleteLayer _levelCompleteLayer;

        private List<Paddle> _paddles = new List<Paddle>();
        private CCLabel _scoreLabel;
        private CCLabel _livesLabel;
        private CCLabel _levelLabel;

        private CCEventListenerTouchAllAtOnce _popupLayerEventListener;
        private CCEventListenerTouchAllAtOnce _levelCompleteLayerEventListener;

        private readonly List<Brick> _bricks = new List<Brick>();
        private readonly List<Powerup> _powerups = new List<Powerup>();
        private readonly List<ScoreUp> _scoreUps = new List<ScoreUp>();
        private readonly List<IPowerup> _activatedPowerups = new List<IPowerup>();
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly List<Wormhole> _wormholes = new List<Wormhole>();
        private readonly List<Projectile> _projectiles = new List<Projectile>();

        private readonly ScoreManager _scoreManager = new ScoreManager();
        private readonly CollisionManager _collisionManager = new CollisionManager();

        private const int PopupLayerEventPriority = 1;
        private const int DefaultEventPriority = 2;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            //GameStateManager.Instance.CheatModeEnabled = true;

            //Preload the entire spritesheet
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(GameConstants.GameEntitiesSpriteSheet, GameConstants.GameEntitiesSpriteSheetImage);

            AddLayers();

            RegisterEventHandlers();

            AddEntities();

            if (GameStateManager.Instance.DebugMode)
            {
                LevelManager.Instance.CurrentLevel = 0;
            }

            //Load first level
            LoadLevel(LevelManager.Instance.CurrentLevel);
        }

        public override void OnExit()
        {
            UnsubscribeCustomEventHandlers();
            base.OnExit();
        }

        private void AddLayers()
        {
            //CCColor4B backgroundColour = GameConstants.BackgroundColours.Dequeue();
            //GameConstants.BackgroundColours.Enqueue(backgroundColour);

            _backgroundLayer = new CCLayerColor(GameConstants.BackgroundColour);
            var sprite = new CCSprite("GameBackground.png")
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            _backgroundLayer.AddChild(sprite);
            AddChild(_backgroundLayer);

            _gameLayer = new CCLayer();
            _hudLayer = new CCLayer();

            //Draw a line under the hud
            CCDrawNode hudDrawNode = new CCDrawNode();
            _hudLayer.AddChild(hudDrawNode);


            var rect = new CCRect(0, _backgroundLayer.VisibleBoundsWorldspace.MaxY - (_backgroundLayer.VisibleBoundsWorldspace.MaxY - GameConstants.WorldTop), _backgroundLayer.VisibleBoundsWorldspace.MaxX, _backgroundLayer.VisibleBoundsWorldspace.MaxY - GameConstants.WorldTop);

            hudDrawNode.DrawRect(rect, new CCColor4B(0, 0, 0, 64));
            hudDrawNode.DrawLine(new CCPoint(0, GameConstants.WorldTop), new CCPoint(GameConstants.WorldWidth, GameConstants.WorldTop), new CCColor4B(255, 255, 255, 1));

            AddChild(_gameLayer);
            AddChild(_hudLayer);

        }

        private void RegisterEventHandlers()
        {
            SubscribeCustomEventHandlers();

            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = HandleTouchesMoved,
                OnTouchesBegan = HandleTouchesBegan
            };
            _gameLayer.AddEventListener(touchListener, DefaultEventPriority);

        }

        /// <summary>
        /// Subscribe to our various game events.
        /// YOU MUST ADD A CORRESPONDING UNSUBSCRIBE IN THE METHOD BELOW
        /// </summary>
        private void SubscribeCustomEventHandlers()
        {
            BallFactory.Instance.BallCreated += BallFactory_BallCreated;
            BrickFactory.Instance.BrickCreated += BrickFactory_BrickCreated;
            BrickFactory.Instance.BrickDestroyed += BrickFactory_BrickDestroyed;

            _collisionManager.BrickHitButNotDestroyed += CollisionManager_BrickHitButNotDestroyed;
            _collisionManager.PaddleHit += CollisionManager_PaddleHit;
            _collisionManager.PowerupCollected += CollisionManager_PowerupCollected;
            _collisionManager.ScoreUpCollected += CollisionManager_ScoreUpCollected;
            _collisionManager.MissedBall += CollisionManager_MissedBall;
            _collisionManager.WormholeInUse += CollisionManager_WormholeInUse;
            _collisionManager.BallIsOutsideWormholes += CollisionManager_BallIsOutsideWormholes;


            GameStateManager.Instance.LevelStarted += GameStateManager_LevelStarted;
            GameStateManager.Instance.LivesChanged += GameStateManager_LivesChanged;
            WormholeFactory.Instance.WormholeCreated += WormholeFactory_WormholeCreated;
            ScoreUpFactory.Instance.ScoreUpCreated += ScoreUpFactory_ScoreUpCreated;
            ScoreUpFactory.Instance.ScoreUpDestroyed += ScoreUpFactory_ScoreUpDestroyed;
            PowerUpFactory.Instance.PowerupDestroyed += PowerupFactory_PowerupDestroyed;
            _scoreManager.ScoreUpdated += ScoreManager_ScoreUpdated;
            ProjectileFactory.Instance.ProjectileCreated += ProjectileFactory_ProjectileCreated;
            ProjectileFactory.Instance.ProjectileDestroyed += ProjectileFactory_ProjectileDestroyed;

            PaddleFactory.Instance.PaddleCreated += PaddleFactory_PaddleCreated;

        }

        /// <summary>
        /// Unsubscribe from our various game events
        /// </summary>
        private void UnsubscribeCustomEventHandlers()
        {
            BallFactory.Instance.BallCreated -= BallFactory_BallCreated;
            BrickFactory.Instance.BrickCreated -= BrickFactory_BrickCreated;
            BrickFactory.Instance.BrickDestroyed -= BrickFactory_BrickDestroyed;

            _collisionManager.BrickHitButNotDestroyed -= CollisionManager_BrickHitButNotDestroyed;
            _collisionManager.PaddleHit -= CollisionManager_PaddleHit;
            _collisionManager.PowerupCollected -= CollisionManager_PowerupCollected;
            _collisionManager.ScoreUpCollected -= CollisionManager_ScoreUpCollected;
            _collisionManager.MissedBall -= CollisionManager_MissedBall;

            GameStateManager.Instance.LevelStarted -= GameStateManager_LevelStarted;
            GameStateManager.Instance.LivesChanged -= GameStateManager_LivesChanged;

            WormholeFactory.Instance.WormholeCreated -= WormholeFactory_WormholeCreated;
            ScoreUpFactory.Instance.ScoreUpCreated -= ScoreUpFactory_ScoreUpCreated;
            ScoreUpFactory.Instance.ScoreUpDestroyed -= ScoreUpFactory_ScoreUpDestroyed;
            PowerUpFactory.Instance.PowerupDestroyed -= PowerupFactory_PowerupDestroyed;
            _scoreManager.ScoreUpdated -= ScoreManager_ScoreUpdated;
            ProjectileFactory.Instance.ProjectileCreated -= ProjectileFactory_ProjectileCreated;
            ProjectileFactory.Instance.ProjectileDestroyed -= ProjectileFactory_ProjectileDestroyed;

            PaddleFactory.Instance.PaddleCreated -= PaddleFactory_PaddleCreated;
        }

        private void AddEntities()
        {

            //Create main paddle (others may be created during level load)
            PaddleFactory.Instance.CreateNew(GameConstants.PaddleInitialPosition);

            //Create one ball
            BallFactory.Instance.CreateNew();

            _scoreLabel = new CCLabel("000000", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MaxX - 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 35,
                AnchorPoint = CCPoint.AnchorUpperRight
            };
            _hudLayer.AddChild(_scoreLabel);

            _levelLabel = new CCLabel($"LEVEL: {LevelManager.Instance.CurrentLevel}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MinX + 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 35,
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _hudLayer.AddChild(_levelLabel);

            _livesLabel = new CCLabel($"LIVES: {GameStateManager.Instance.Lives}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MinX + 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 85,
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _hudLayer.AddChild(_livesLabel);


        }

        #region Event Handlers
        private void BallFactory_BallCreated(Ball ball)
        {
            _balls.Add(ball);
            _gameLayer.AddChild(ball, GameConstants.BallZOrder);

            if (GameStateManager.Instance.DebugMode)
            {
                var movedTouchListener = new CCEventListenerTouchAllAtOnce { OnTouchesMoved = Ball_OnTouchesMoved };
                AddEventListener(movedTouchListener, _balls.First());
            }

        }

        private void BrickFactory_BrickCreated(Brick brick)
        {
            _bricks.Add(brick);
            _gameLayer.AddChild(brick);

            if (brick.Powerup != null)
            {
                _powerups.Add(brick.Powerup);
                _gameLayer.AddChild(brick.Powerup, GameConstants.PowerupZOrder);
            }
        }

        private void BrickFactory_BrickDestroyed(Brick brick)
        {

            PlayRandomBrickSound();

            var explosion = new CCParticleExplosion(new CCPoint(brick.BoundingBoxTransformedToWorld.Center.X, brick.BoundingBoxTransformedToWorld.Center.Y))
            {
                TotalParticles = 5,
                AutoRemoveOnFinish = true
            };
            _gameLayer.AddChild(explosion);

            _bricks.Remove(brick);
            brick.RemoveFromParent();

            _scoreManager.BrickDestroyed();
        }

        private void ProjectileFactory_ProjectileCreated(Projectile projectile)
        {
            foreach (Paddle paddle in _paddles)
            {
                if (paddle.Weapon.IsSingleShot)
                {
                    if (_projectiles.Any())
                    {
                        return;
                    }
                }
            }

            _projectiles.Add(projectile);
            _gameLayer.AddChild(projectile);

            CCAudioEngine.SharedEngine.PlayEffect(_paddles[0].Weapon.FireSound);
        }

        private void ProjectileFactory_ProjectileDestroyed(Projectile bullet)
        {
            _projectiles.Remove(bullet);
            bullet.RemoveFromParent();
        }

        private void WormholeFactory_WormholeCreated(Wormhole wormhole)
        {
            _wormholes.Add(wormhole);
            _gameLayer.AddChild(wormhole);
        }

        private void CollisionManager_PaddleHit()
        {
            CCAudioEngine.SharedEngine.PlayEffect(GameConstants.PaddleHitSound);
        }

        private void CollisionManager_BrickHitButNotDestroyed()
        {
            CCAudioEngine.SharedEngine.PlayEffect(GameConstants.BrickHitButNotDestroyedSound);
        }

        private void CollisionManager_ScoreUpCollected(ScoreUp scoreUp)
        {
            CCAudioEngine.SharedEngine.PlayEffect(GameConstants.ScoreUpSound);
            _scoreManager.ScoreUpCollected(scoreUp.Score);
            scoreUp.RemoveFromParent();
            _scoreUps.Remove(scoreUp);
        }

        private void CollisionManager_PowerupCollected(Powerup powerup)
        {
            powerup.Activate();
            powerup.RemoveFromParent();
            _activatedPowerups.Add(powerup);

            _powerups.Remove(powerup);

            _scoreManager.PowerupCollected();
        }

        private void CollisionManager_MissedBall(Ball ball)
        {

            ball.RemoveFromParent();
            _balls.Remove(ball);

            if (_balls.Count == 0)
            {
                //Stop the level
                GameStateManager.Instance.StartStopLevel(false);

                //Create another ball
                BallFactory.Instance.CreateNew();

                //Reduce the lives
                GameStateManager.Instance.LoseLife();

                //Remove any scoreUps
                RemoveAllScoreUps();

                if (GameStateManager.Instance.Lives == 0)
                {
                    //game over
                    ShowGameOverPopup();
                }
            }

        }

        private void CollisionManager_WormholeInUse(Wormhole wormhole)
        {
            _wormholes.Single(w => w.ObjectName == wormhole.ObjectName).InUse = true;
        }

        private void CollisionManager_BallIsOutsideWormholes()
        {
            foreach (Wormhole wormhole in _wormholes.Where(w => w.InUse))
            {
                wormhole.InUse = false;
            }
        }

        private void GameStateManager_LivesChanged()
        {
            _livesLabel.Text = $"LIVES: {GameStateManager.Instance.Lives}";
        }

        private void ScoreUpFactory_ScoreUpCreated(ScoreUp scoreUp)
        {
            _scoreUps.Add(scoreUp);
            _gameLayer.AddChild(scoreUp);
        }

        private void ScoreUpFactory_ScoreUpDestroyed(ScoreUp scoreUp)
        {
            _scoreUps.Remove(scoreUp);
            scoreUp.RemoveFromParent();
        }

        private void ScoreManager_ScoreUpdated()
        {
            _scoreLabel.Text = _scoreManager.Score.ToString("000000");
        }

        private void Ball_OnTouchesMoved(List<CCTouch> touches, CCEvent arg2)
        {
            if (touches.Count > 0)
            {

                Ball ball = _balls.First();

                CCTouch firstTouch = touches[0];

                ball.PositionX = firstTouch.Location.X - 100;
                ball.PositionY = firstTouch.Location.Y + 100;

            }
        }

        private void GameStateManager_LevelStarted(bool started)
        {
            if (started || GameStateManager.Instance.DebugMode)
            {
                //Main game loop
                Schedule(RunGameLogic);

                //Drop scoreUps every 10 seconds
                Schedule(frameTime => { ScoreUpFactory.Instance.CreateNew(); }, 10);

                //Track level running time
                Schedule(frameTime => { GameStateManager.Instance.LevelRunTime += frameTime; }, 1);

            }
            else
            {
                UnscheduleAll();
            }

        }

        private void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (!GameStateManager.Instance.LevelHasStarted)
            {
                GameStateManager.Instance.StartStopLevel(!GameStateManager.Instance.DebugMode);
            }

            if (touches.Count > 0)
            {
                CCTouch firstTouch = touches[0];

                foreach (Paddle paddle in _paddles)
                {
                    paddle.PositionX = firstTouch.Location.X;
                }

            }
        }

        private void HandleTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            foreach (Paddle paddle in _paddles)
            {
                if (paddle.Weapon != null)
                {
                    paddle.FireProjectile();
                }
            }
        }

        private void PaddleFactory_PaddleCreated(Paddle paddle)
        {
            _paddles.Add(paddle);
            _gameLayer.AddChild(paddle);
        }

        /// <summary>
        /// This higher priority event prevents event propogation from triggering the GameScene TouchesMoved event
        /// </summary>
        private void PopupLayer_TouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            touchEvent.StopPropogation();
        }

        private void LevelCompleteLayer_TouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            DisposeLevelCompleteLayer();
            touchEvent.StopPropogation();

            //Load next level
            LoadNextLevel();
        }

        private void PowerupFactory_PowerupDestroyed(Powerup powerup)
        {
            _powerups.Remove(powerup);
            powerup.RemoveFromParent();
        }


        #endregion

        /// <summary>
        /// The main game loop, checks for collisions and whether the level has been completed
        /// </summary>
        private void RunGameLogic(float frameTimeInSeconds)
        {
            //Collision logic
            _collisionManager.HandleCollisions(_gameLayer, _paddles, _balls, _bricks, _powerups, _wormholes, _scoreUps, _projectiles);

            //Remove projectiles if they go off screen
            for (int p = _projectiles.Count - 1; p >= 0; p--)
            {
                Projectile projectile = _projectiles[p];
                if (projectile.PositionY > GameConstants.WorldTop)
                {
                    projectile.RemoveFromParent();
                    _projectiles.Remove(projectile);
                }
            }

            //Level Complete?
            bool allBricksAreIndistructible = _bricks.All(b => b.IsIndestructible);
            if (_bricks.Count == 0 || allBricksAreIndistructible)
            {
                CompleteLevel();
            }

        }

        private void CompleteLevel()
        {
            GameStateManager.Instance.StartStopLevel(false);

            //Save progress
            Settings.HighestCompletedLevel = Math.Max(Settings.HighestCompletedLevel, LevelManager.Instance.CurrentLevel);

            //Score bonuses
            int timeBonus = _scoreManager.AddTimeBonus(GameStateManager.Instance.LevelRunTime);
            int livesBonus = _scoreManager.AddLivesBonus(GameStateManager.Instance.Lives, LevelManager.Instance.CurrentLevelProperties.Lives);

            //Save high score
            _scoreManager.SaveCurrentLevelHighScore();

            //Show level complete layer
            ShowLevelCompleteLayer(timeBonus, livesBonus);

        }

        private void ResetEntities()
        {
            //Reset powerups
            foreach (Powerup powerup in _powerups)
            {
                powerup.RemoveFromParent();
            }
            _powerups.Clear();
            foreach (IPowerup powerup in _activatedPowerups)
            {
                powerup.Deactivate();
            }
            _activatedPowerups.Clear();

            //Reset ball(s)
            BallFactory.Instance.ResetBalls(_balls);

            //Remove any scoreUps
            RemoveAllScoreUps();

            //Reset bricks
            foreach (Brick brick in _bricks)
            {
                brick.RemoveFromParent();
            }
            _bricks.Clear();

            //Reset wormholes
            foreach (Wormhole wormhole in _wormholes)
            {
                wormhole.RemoveFromParent();
            }
            _wormholes.Clear();
        }

        private void RemoveAllScoreUps()
        {
            //Remove any scoreUps
            foreach (ScoreUp scoreUp in _scoreUps)
            {
                scoreUp.RemoveFromParent();
            }
            _scoreUps.Clear();
        }

        private void PlayRandomBrickSound()
        {
            if (!GameStateManager.Instance.DebugMode)
            {

                string sound = GameConstants.BrickHitSounds.Dequeue();
                CCAudioEngine.SharedEngine.PlayEffect(sound);
                GameConstants.BrickHitSounds.Enqueue(sound);

                //Random rnd = new Random();
                //int r = rnd.Next(GameConstants.BrickHitSounds.Count);
                //CCAudioEngine.SharedEngine.PlayEffect(GameConstants.BrickHitSounds[r]);
            }
        }

        private void LoadLevel(int level, bool showNewLevelPopup = true)
        {

            ResetEntities();

            LevelManager.Instance.LoadLevel(level, _paddles[0], _balls, _scoreManager);

            foreach (Ball ball in _balls)
            {
                ball.ApplyGravity = LevelManager.Instance.CurrentLevelProperties.Gravity;
            }

            GameStateManager.Instance.SetLives(LevelManager.Instance.CurrentLevelProperties.Lives);

            _scoreManager.ResetScore();

            GameStateManager.Instance.StartStopLevel(false);

            if (showNewLevelPopup)
            {
                ShowNewLevelPopup(GameStateManager.Instance.Lives);
            }

        }

        private void LoadNextLevel()
        {
            LevelManager.Instance.CurrentLevel = LevelManager.Instance.GetNextLevel(LevelManager.Instance.CurrentLevel);
            LoadLevel(LevelManager.Instance.CurrentLevel);
        }

        #region Popups / Layers

        //New level popup
        private void ShowNewLevelPopup(int lives)
        {
            _newLevelLayer = new NewLevelLayer(lives, LevelManager.Instance.CurrentLevelProperties.HighScore);
            AddChild(_newLevelLayer);

            _popupLayerEventListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = PopupLayer_TouchesMoved
            };

            _newLevelLayer.AddEventListener(_popupLayerEventListener, PopupLayerEventPriority);
            _newLevelLayer.PlayButtonPressed += NewLevelLayer_PlayButtonPressed;
            _newLevelLayer.MainMenuButtonPressed += NewLeveLayer_MainMenuButtonPressed;
        }
        private void NewLevelLayer_PlayButtonPressed()
        {
            DisposeNewLevelPopup();
            _levelLabel.Text = $"LEVEL: {LevelManager.Instance.CurrentLevel}";
        }
        private void NewLeveLayer_MainMenuButtonPressed()
        {
            DisposeNewLevelPopup();
            GameController.GoToScene(new TitleScene(GameView));
        }
        private void DisposeNewLevelPopup()
        {
            _newLevelLayer.PlayButtonPressed -= NewLevelLayer_PlayButtonPressed;
            _newLevelLayer.MainMenuButtonPressed -= NewLeveLayer_MainMenuButtonPressed;
            _newLevelLayer.RemoveEventListener(_popupLayerEventListener);
            _newLevelLayer.RemoveFromParent();
            _newLevelLayer.Dispose();
        }

        //Game over popup
        private void ShowGameOverPopup()
        {
            _gameOverLayer = new GameOverLayer();
            AddChild(_gameOverLayer);

            _popupLayerEventListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = PopupLayer_TouchesMoved
            };

            _gameOverLayer.AddEventListener(_popupLayerEventListener, PopupLayerEventPriority);
            _gameOverLayer.PlayButtonPressed += GameOverLayer_PlayButtonPressed;
            _gameOverLayer.MainMenuButtonPressed += GameOverLayer_MainMenuButtonPressed;
        }
        private void GameOverLayer_PlayButtonPressed()
        {
            DisposeGameOverLayer();
            LoadLevel(LevelManager.Instance.CurrentLevel, showNewLevelPopup: false);
        }
        private void GameOverLayer_MainMenuButtonPressed()
        {
            DisposeGameOverLayer();
            GameController.GoToScene(new TitleScene(GameView));
        }
        private void DisposeGameOverLayer()
        {
            _gameOverLayer.PlayButtonPressed -= GameOverLayer_PlayButtonPressed;
            _gameOverLayer.MainMenuButtonPressed -= GameOverLayer_MainMenuButtonPressed;
            _gameOverLayer.RemoveEventListener(_popupLayerEventListener);
            _gameOverLayer.RemoveFromParent();
            _gameOverLayer.Dispose();
        }

        //Level complete layer
        private void ShowLevelCompleteLayer(int timeBonus, int livesBonus)
        {
            _levelCompleteLayer = new LevelCompleteLayer(timeBonus, livesBonus, _scoreManager);
            AddChild(_levelCompleteLayer);

            _levelCompleteLayerEventListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = PopupLayer_TouchesMoved,
                OnTouchesBegan = LevelCompleteLayer_TouchesBegan
            };
            _levelCompleteLayer.AddEventListener(_levelCompleteLayerEventListener, PopupLayerEventPriority);
        }
        private void DisposeLevelCompleteLayer()
        {
            _levelCompleteLayer.RemoveEventListener(_levelCompleteLayerEventListener);
            _levelCompleteLayer.RemoveFromParent();
            _levelCompleteLayer.Dispose();
        }

        #endregion

    }
}

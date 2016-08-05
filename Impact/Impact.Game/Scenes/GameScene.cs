using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Factories;
using Impact.Game.Layers;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class GameScene : CCScene
    {
        private CCLayer _gameLayer;
        private CCLayer _hudLayer;
        private NewLevelLayer _newLevelLayer;

        private Paddle _paddle;
        private CCLabel _scoreLabel;
        private CCLabel _livesLabel;
        private CCLabel _levelLabel;

        private CCEventListenerTouchAllAtOnce _newLevelPopupEventListener;

        private readonly List<Brick> _bricks = new List<Brick>();
        private readonly List<Powerup> _powerups = new List<Powerup>();
        private readonly List<ScoreUp> _scoreUps = new List<ScoreUp>();
        private readonly List<IPowerup> _activatedPowerups = new List<IPowerup>();
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly List<Wormhole> _wormholes = new List<Wormhole>();

        private readonly ScoreManager _scoreManager = new ScoreManager();
        private readonly CollisionManager _collisionManager = new CollisionManager();

        private int _lives = 3;

        private const int NewLevelLayerEventPriority = 1;
        private const int DefaultEventPriority = 2;

        public GameScene(CCGameView gameView) : base(gameView)
        {
            GameManager.Instance.CheatModeEnabled = false;

            //Preload the entire spritesheet
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(GameConstants.GameEntitiesSpriteSheet, GameConstants.GameEntitiesSpriteSheetImage);

            AddLayers();

            RegisterEventHandlers();

            AddEntities();

            if (GameManager.Instance.DebugMode)
            {
                LevelManager.Instance.CurrentLevel = 0;
            }

            //Load first level
            LoadLevel(LevelManager.Instance.CurrentLevel);
        }

        private void AddLayers()
        {
            //var backgroundLayer = new CCLayer();
            //CCSprite backgroundImage = new CCSprite("background.png")
            //{
            //    AnchorPoint = CCPoint.AnchorLowerLeft
            //};
            //backgroundLayer.AddChild(backgroundImage);

            var backgroundLayer = new CCLayerColor(GameConstants.BackgroundColour);
            AddChild(backgroundLayer);
            
            _gameLayer = new CCLayer();
            _hudLayer = new CCLayer();
                        
            AddChild(_gameLayer);
            AddChild(_hudLayer);

        }

        private void RegisterEventHandlers()
        {
            BallFactory.Instance.BallCreated += BallFactory_BallCreated;
            BrickFactory.Instance.BrickCreated += BrickFactory_BrickCreated;
            BrickFactory.Instance.BrickDestroyed += BrickFactory_BrickDestroyed;

            _collisionManager.BrickHitButNotDestroyed += CollisionManager_BrickHitButNotDestroyed;
            _collisionManager.PaddleHit += CollisionManager_PaddleHit;
            _collisionManager.PowerupCollected += CollisionManager_PowerupCollected;
            _collisionManager.ScoreUpCollected += CollisionManager_ScoreUpCollected;
            _collisionManager.MissedBall += CollisionManagerMissedBall;

            GameManager.Instance.LevelStarted += GameManager_LevelStarted;
            WormholeFactory.Instance.WormholeCreated += WormholeFactory_WormholeCreated;
            ScoreUpFactory.Instance.ScoreUpCreated += ScoreUpFactory_ScoreUpCreated;
            _scoreManager.ScoreUpdated += ScoreManager_ScoreUpdated;


            // Register for touch events
            var touchListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = HandleTouchesMoved
            };
            AddEventListener(touchListener, DefaultEventPriority);
            
        }
        
        private void AddEntities()
        {
            _paddle = new Paddle();
            _gameLayer.AddChild(_paddle);

            //Create one ball
            BallFactory.Instance.CreateNew();

            _scoreLabel = new CCLabel("000000", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MaxX - 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 50,
                AnchorPoint = CCPoint.AnchorUpperRight
            };
            _hudLayer.AddChild(_scoreLabel);

            _levelLabel = new CCLabel($"LEVEL: {LevelManager.Instance.CurrentLevel}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MinX + 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 50,
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _hudLayer.AddChild(_levelLabel);

            _livesLabel = new CCLabel($"LIVES: {_lives}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                PositionX = _gameLayer.VisibleBoundsWorldspace.MinX + 50,
                PositionY = _gameLayer.VisibleBoundsWorldspace.MaxY - 100,
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _hudLayer.AddChild(_livesLabel);

            
        }

        #region Event Handlers
        private void BallFactory_BallCreated(Ball ball)
        {
            _balls.Add(ball);
            _gameLayer.AddChild(ball, GameConstants.BallZOrder);

            if (GameManager.Instance.DebugMode)
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
            _scoreManager.ScoreUpCollected(scoreUp.Score);

            scoreUp.RemoveFromParent();
            _scoreUps.Remove(scoreUp);
        }

        private void CollisionManager_PowerupCollected(Powerup powerup)
        {
            powerup.Activate();
            _activatedPowerups.Add(powerup);

            powerup.RemoveFromParent();
            _powerups.Remove(powerup);

            _scoreManager.PowerupCollected();
        }

        private void CollisionManagerMissedBall(Ball ball)
        {

            ball.RemoveFromParent();
            _balls.Remove(ball);

            if (_balls.Count == 0)
            {
                //Stop the level
                GameManager.Instance.StartStopLevel(false);

                //Create another ball
                BallFactory.Instance.CreateNew();

                //Reduce the lives
                _lives -= 1;

                //Remove any scoreUps
                RemoveAllScoreUps();

                if (_lives == 0)
                {
                    //game over
                    GameManager.Instance.StartStopLevel(false);
                    GameController.GoToScene(new LevelSelectScene(GameView));
                }
            }
            
        }

        private void ScoreUpFactory_ScoreUpCreated(ScoreUp scoreUp)
        {
            _scoreUps.Add(scoreUp);
            _gameLayer.AddChild(scoreUp);
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

        private void GameManager_LevelStarted(bool started)
        {
            if (started || GameManager.Instance.DebugMode)
            {
                Schedule(RunGameLogic);
                //Schedule(UpdateTimer, 0.25f);
                Schedule(frameTime => {ScoreUpFactory.Instance.CreateNew();}, 10);
            }
            else
            {
                UnscheduleAll();
                //Unschedule(UpdateTimer);
            }
            
        }

        private void HandleTouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (!GameManager.Instance.LevelHasStarted)
            {
                GameManager.Instance.StartStopLevel(!GameManager.Instance.DebugMode);
            }
        }

        /// <summary>
        /// This higher priority event prevents event propogation from triggering the GameScene TouchesMoved event
        /// </summary>
        private void NewLevelLayer_TouchesMoved(List<CCTouch> touches, CCEvent touchEvent)
        {
            touchEvent.StopPropogation();
        }

        #endregion

        /// <summary>
        /// The main game loop, checks for collisions and whether the level has been completed
        /// </summary>
        private void RunGameLogic(float frameTimeInSeconds)
        {
            _livesLabel.Text = $"LIVES: {_lives}";
            _collisionManager.HandleCollisions(_gameLayer, _paddle, _balls, _bricks, _powerups, _wormholes, _scoreUps);

            //Level Complete?
            bool allBricksAreIndistructible = _bricks.All(b => b.IsIndestructible);
            if (_bricks.Count == 0 || allBricksAreIndistructible)
            {
                //Reset powerups
                foreach (IPowerup powerup in _activatedPowerups)
                {
                    powerup.Deactivate();
                }
                _powerups.Clear();
                
                //Reset ball(s)
                BallFactory.Instance.ResetBalls(_balls);

                //Remove any scoreUps
                RemoveAllScoreUps();

                //Load next level
                LevelManager.Instance.CurrentLevel++;
                LoadLevel(LevelManager.Instance.CurrentLevel);

            }

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
            if (!GameManager.Instance.DebugMode)
            {

                string sound = GameConstants.BrickHitSounds.Dequeue();
                CCAudioEngine.SharedEngine.PlayEffect(sound);
                GameConstants.BrickHitSounds.Enqueue(sound);

                //Random rnd = new Random();
                //int r = rnd.Next(GameConstants.BrickHitSounds.Count);
                //CCAudioEngine.SharedEngine.PlayEffect(GameConstants.BrickHitSounds[r]);
            }
        }

        private void LoadLevel(int level)
        {
            foreach (Brick brick in _bricks)
            {
                brick.RemoveFromParent();
            }
            _bricks.Clear();

            foreach (Wormhole wormhole in _wormholes)
            {
                wormhole.RemoveFromParent();
            }
            _wormholes.Clear();

            LevelManager.Instance.LoadLevel(level, _paddle, _balls);

            foreach (Ball ball in _balls)
            {
                ball.ApplyGravity = LevelManager.Instance.CurrentLevelProperties.Gravity;
            }

            _lives = 3;

            GameManager.Instance.StartStopLevel(false);

            ShowNewLevelPopup(_lives);

        }

        private void ShowNewLevelPopup(int lives)
        {
            _newLevelLayer = new NewLevelLayer(lives);
            AddChild(_newLevelLayer);

            _newLevelPopupEventListener = new CCEventListenerTouchAllAtOnce
            {
                OnTouchesMoved = NewLevelLayer_TouchesMoved
            };

            _newLevelLayer.AddEventListener(_newLevelPopupEventListener, NewLevelLayerEventPriority);
            _newLevelLayer.PlayButtonPressed += NewLevelLayer_PlayButtonPressed;
        }

        private void NewLevelLayer_PlayButtonPressed()
        {
            _newLevelLayer.PlayButtonPressed -= NewLevelLayer_PlayButtonPressed;
            _newLevelLayer.RemoveEventListener(_newLevelPopupEventListener);
            _newLevelLayer.RemoveFromParent();
            _newLevelLayer.Dispose();

            _livesLabel.Text = $"LEVEL: {LevelManager.Instance.CurrentLevel}";
        }
    }
}

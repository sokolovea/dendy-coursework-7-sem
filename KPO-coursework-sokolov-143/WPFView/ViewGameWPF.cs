using BaseMenuView;
using Engine;
using Engine.DynamicObjects;
using Engine.DynamicObjects.Tank;
using Engine.StaticObjects;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFView.Custom;
using Point = Engine.Field.Point;

namespace WPFView
{
    /// <summary>
    /// Реализация вида игрового поля в WPF
    /// </summary>
    public class ViewGameWPF : IViewBase
    {
        /// <summary>
        /// Модель игры
        /// </summary>
        private readonly GameModel _gameModel;

        /// <summary>
        /// Текущее графическое окно
        /// </summary>
        private readonly Window _window;

        /// <summary>
        /// Графическая область для рисования
        /// </summary>
        private readonly Canvas _gameCanvas;

        /// <summary>
        /// Текущее состояние и настройки игры
        /// </summary>
        private GameCurrentStateAndSettingsSingleton _gameSettings = GameCurrentStateAndSettingsSingleton.GetInstance();

        /// <summary>
        /// Уже отрисованные на поле танки
        /// </summary>
        private Dictionary<Guid, Image> _tankImages = new();

        /// <summary>
        /// Уже отрисованные на поле снаряды
        /// </summary>
        private Dictionary<Guid, Ellipse> _bulletEllipses = new();

        /// <summary>
        /// Уже отрисованные блоки
        /// </summary>
        private readonly Dictionary<(int x, int y), Image> _fieldStaticSprites = new();

        /// <summary>
        /// Графическая область для рисования
        /// </summary>
        public Canvas GameCanvas
        {
            get
            {
                return _gameCanvas;
            }
        }

        /// <summary>
        /// Конструирует графическую версию игры
        /// </summary>
        /// <param name="parGameModel">Модель игры</param>
        public ViewGameWPF(GameModel parGameModel)
        {
            _gameModel = parGameModel;


            _window = WindowSingleton.GetInstance().GetWindow();

            _gameCanvas = new Canvas
            {
                Width = _gameModel.Field.GetWidthInPixels(),
                Height = _gameModel.Field.GetHeightInPixels(),
                Background = Brushes.Black
            };
            StackPanel stackPanel = new StackPanel();
            stackPanel.Children.Add(_gameCanvas);

            CustomLabel playerTankInfo = new CustomLabel(_gameSettings.GetImportantInfoForView());
            _gameModel.PlayerTankInfoUpdate += () =>
            {
                playerTankInfo.Content = _gameSettings.GetImportantInfoForView();
            };

            stackPanel.Children.Add(playerTankInfo);
            _window.Content = stackPanel;

            _gameModel.ObjectMoved += OnObjectMoved;
            _gameModel.BulletDestroyed += OnBulletDestroyed;
            _gameModel.EnemyTankDestroyed += OnTankDestroyed;

            _gameModel.StaticObjectDestroyed += OnStaticObjectDestroyed;
            _gameModel.StaticObjectRefreshed += OnStaticObjectRefreshed;

            DrawFieldObjects();
            _gameModel.Start();

        }

        /// <summary>
        /// Нарисовать (перерисовать) игровое поле
        /// </summary>
        public void Draw()
        {
            DrawGameObjects();
        }

        /// <summary>
        /// Остановить игру
        /// </summary>
        public void Stop()
        {
            _gameModel.Stop();
            _gameModel.ObjectMoved -= OnObjectMoved;
            _gameModel.BulletDestroyed -= OnBulletDestroyed;
            _gameModel.EnemyTankDestroyed -= OnTankDestroyed;

            _gameModel.StaticObjectDestroyed += OnStaticObjectDestroyed;
            _gameModel.StaticObjectRefreshed -= OnStaticObjectRefreshed;
        }

        /// <summary>
        /// Нарисовать элементы игрового поля
        /// </summary>
        private void DrawGameObjects()
        {
            foreach (var elTank in _gameModel.Tanks)
            {
                DrawTank(elTank, false);
            }
            DrawTank(_gameModel.Player.PlayerTank, true);

            foreach (var elBullet in _gameModel.Bullets)
            {
                DrawBullet(elBullet);
            }

        }

        /// <summary>
        /// Получить спрайт для танка
        /// </summary>
        /// <param name="parTank">Танк</param>
        /// <param name="parIsPlayer">Принадлежит ли танк игроку</param>
        /// <returns>Спрайт танка</returns>
        private ImageSource GetTankImage(AbstractTank parTank, bool parIsPlayer)
        {
            string imagePath = "";
            if (parIsPlayer)
            {
                imagePath = parTank.Direction switch
                {
                    DirectionEnum.Up => "pack://application:,,,/WPFView;component/Sprites/Tanks/Player/tank_up.png",
                    DirectionEnum.Down => "pack://application:,,,/WPFView;component/Sprites/Tanks/Player/tank_down.png",
                    DirectionEnum.Left => "pack://application:,,,/WPFView;component/Sprites/Tanks/Player/tank_left.png",
                    DirectionEnum.Right => "pack://application:,,,/WPFView;component/Sprites/Tanks/Player/tank_right.png",
                    _ => "pack://application:,,,/WPFView;component/Sprites/Tanks/Player/tank_up.png" // По умолчанию
                };
            }
            else if (parTank is GeneralTank)
            {
                imagePath = parTank.Direction switch
                {
                    DirectionEnum.Up => "pack://application:,,,/WPFView;component/Sprites/Tanks/General/tank_up.png",
                    DirectionEnum.Down => "pack://application:,,,/WPFView;component/Sprites/Tanks/General/tank_down.png",
                    DirectionEnum.Left => "pack://application:,,,/WPFView;component/Sprites/Tanks/General/tank_left.png",
                    DirectionEnum.Right => "pack://application:,,,/WPFView;component/Sprites/Tanks/General/tank_right.png",
                    _ => "pack://application:,,,/WPFView;component/Sprites/Tanks/General/tank_up.png" // По умолчанию
                };
            }
            else if (parTank is ArmoredTank)
            {
                imagePath = parTank.Direction switch
                {
                    DirectionEnum.Up => "pack://application:,,,/WPFView;component/Sprites/Tanks/Armored/tank_up.png",
                    DirectionEnum.Down => "pack://application:,,,/WPFView;component/Sprites/Tanks/Armored/tank_down.png",
                    DirectionEnum.Left => "pack://application:,,,/WPFView;component/Sprites/Tanks/Armored/tank_left.png",
                    DirectionEnum.Right => "pack://application:,,,/WPFView;component/Sprites/Tanks/Armored/tank_right.png",
                    _ => "pack://application:,,,/WPFView;component/Sprites/Tanks/Armored/tank_up.png" // По умолчанию
                };
            }
            else if (parTank is HighSpeedTank)
            {
                imagePath = parTank.Direction switch
                {
                    DirectionEnum.Up => "pack://application:,,,/WPFView;component/Sprites/Tanks/HighSpeed/tank_up.png",
                    DirectionEnum.Down => "pack://application:,,,/WPFView;component/Sprites/Tanks/HighSpeed/tank_down.png",
                    DirectionEnum.Left => "pack://application:,,,/WPFView;component/Sprites/Tanks/HighSpeed/tank_left.png",
                    DirectionEnum.Right => "pack://application:,,,/WPFView;component/Sprites/Tanks/HighSpeed/tank_right.png",
                    _ => "pack://application:,,,/WPFView;component/Sprites/Tanks/HighSpeed/tank_up.png" // По умолчанию
                };
            }
            else if (parTank is RapidFireTank)
            {
                imagePath = parTank.Direction switch
                {
                    DirectionEnum.Up => "pack://application:,,,/WPFView;component/Sprites/Tanks/RapidFire/tank_up.png",
                    DirectionEnum.Down => "pack://application:,,,/WPFView;component/Sprites/Tanks/RapidFire/tank_down.png",
                    DirectionEnum.Left => "pack://application:,,,/WPFView;component/Sprites/Tanks/RapidFire/tank_left.png",
                    DirectionEnum.Right => "pack://application:,,,/WPFView;component/Sprites/Tanks/RapidFire/tank_right.png",
                    _ => "pack://application:,,,/WPFView;component/Sprites/Tanks/RapidFire/tank_up.png" // По умолчанию
                };
            }

            return new BitmapImage(new Uri(imagePath));
        }

        /// <summary>
        /// Отрисовать танк
        /// </summary>
        /// <param name="parTank">Танк</param>
        /// <param name="parIsPlayer">Принадлежит ли танк игроку</param>
        private void DrawTank(AbstractTank parTank, bool parIsPlayer)
        {
            if (parTank == null)
            {
                return;
            }

            _gameCanvas.Dispatcher.Invoke(() =>
            {
                if (!_tankImages.ContainsKey(parTank.Id))
                {
                    var image = new Image
                    {
                        Width = parTank.Width,
                        Height = parTank.Height,
                        Source = GetTankImage(parTank, parIsPlayer)
                    };
                    Canvas.SetLeft(image, parTank.Point.X - parTank.Width / 2);
                    Canvas.SetTop(image, parTank.Point.Y - parTank.Height / 2);
                    _gameCanvas.Children.Add(image);

                    // Добавляем танк в словарь
                    _tankImages[parTank.Id] = image;
                }
                else
                {
                    var existingTank = _tankImages[parTank.Id] as Image;
                    if (existingTank == null) return;


                        Canvas.SetLeft(existingTank, parTank.Point.X - parTank.Width / 2);
                        Canvas.SetTop(existingTank, parTank.Point.Y - parTank.Height / 2);

                        var currentSource = existingTank.Source as BitmapImage;
                        var newSource = GetTankImage(parTank, parIsPlayer) as BitmapImage;
                        if (currentSource?.UriSource != newSource?.UriSource)
                        {
                            existingTank.Source = newSource;
                        }
                }
            });
        }

        /// <summary>
        /// Отрисовать пулю
        /// </summary>
        /// <param name="parBullet"></param>
        private void DrawBullet(Bullet parBullet)
        {
            _gameCanvas.Dispatcher.Invoke(() =>
            {
                if (!_bulletEllipses.ContainsKey(parBullet.Id))
                {
                    var ellipse = new Ellipse
                    {
                        Width = 4,
                        Height = 4,
                        Fill = Brushes.Yellow
                    };
                    Canvas.SetLeft(ellipse, parBullet.Point.X);
                    Canvas.SetTop(ellipse, parBullet.Point.Y);
                    _gameCanvas.Children.Add(ellipse);
                    _bulletEllipses[parBullet.Id] = ellipse;
                }
                else
                {
                    var existingBullet = _bulletEllipses[parBullet.Id];
                    var currentLeft = Canvas.GetLeft(existingBullet);
                    var currentTop = Canvas.GetTop(existingBullet);

                    if (currentLeft != parBullet.Point.X || currentTop != parBullet.Point.Y)
                    {
                        Canvas.SetLeft(existingBullet, parBullet.Point.X);
                        Canvas.SetTop(existingBullet, parBullet.Point.Y);
                    }
                }
            });
        }

        /// <summary>
        /// Событие уничтожения пули
        /// </summary>
        /// <param name="parBullet">Уничтожаемая пуля</param>
        private void OnBulletDestroyed(Bullet parBullet)
        {
            _gameCanvas.Dispatcher.Invoke(() =>
            {
                parBullet.OnObjectDestroyed();
                PlayBulletExplosionAnimation(parBullet.Point);
                if (_bulletEllipses.ContainsKey(parBullet.Id))
                {
                    _gameCanvas.Children.Remove(_bulletEllipses[parBullet.Id]);
                    _bulletEllipses.Remove(parBullet.Id);
                }
            });
        }

        /// <summary>
        /// Событие уничтожения танка
        /// </summary>
        /// <param name="parTank">Уничтоженный танк</param>
        /// <param name="parIsKilledByPlayer">Уничтожен ли танк игроком</param>
        private void OnTankDestroyed(AbstractTank parTank, bool parIsKilledByPlayer)
        {
            _gameCanvas.Dispatcher.Invoke(() =>
            {
                PlayTankExplosionAnimation(parTank.Point);
                if (_tankImages.ContainsKey(parTank.Id))
                {
                    _gameCanvas.Children.Remove(_tankImages[parTank.Id]);
                    _tankImages.Remove(parTank.Id);
                }
            });
        }

        /// <summary>
        /// Уничтожение блока
        /// </summary>
        /// <param name="parAbstractStaticObject">Блок игрового поля</param>
        private void OnStaticObjectDestroyed(AbstractStaticObject parAbstractStaticObject)
        {
            int x = (int)parAbstractStaticObject.Point.X;
            int y = (int)parAbstractStaticObject.Point.Y;
            if (_fieldStaticSprites.ContainsKey((x, y)))
            {
                _gameCanvas.Dispatcher.Invoke(() =>
                {
                    var image = _fieldStaticSprites[(x, y)];
                    _gameCanvas.Children.Remove(image);
                    _fieldStaticSprites.Remove((x, y));
                });
            }
        }

        /// <summary>
        /// Обновление статического блока игрового поля
        /// </summary>
        /// <param name="parAbstractStaticObject">Блок для обновления</param>
        private void OnStaticObjectRefreshed(AbstractStaticObject parAbstractStaticObject)
        {
            int x = (int)parAbstractStaticObject.Point.X;
            int y = (int)parAbstractStaticObject.Point.Y;
            _gameCanvas.Dispatcher.Invoke(() =>
            {
                if (_fieldStaticSprites.ContainsKey((x, y)))
                {
                    var image = _fieldStaticSprites[(x, y)];
                    _gameCanvas.Children.Remove(image);
                    _fieldStaticSprites.Remove((x, y));
                }
                if (!_fieldStaticSprites.ContainsKey((x, y)))
                {
                    var image = new Image
                    {
                        Width = _gameModel.Field.CellSize,
                        Height = _gameModel.Field.CellSize,
                        Source = GetImageSource(parAbstractStaticObject)
                    };
                    Canvas.SetLeft(image, x * _gameModel.Field.CellSize);
                    Canvas.SetTop(image, y * _gameModel.Field.CellSize);
                    _gameCanvas.Children.Add(image);

                    _fieldStaticSprites[(x, y)] = image;
                }
            });
        }

        /// <summary>
        /// Отрисовать все игровое поле (статические блоки)
        /// </summary>
        private void DrawFieldObjects()
        {
            _gameCanvas.Children.Clear();
            for (int x = 0; x < _gameModel.Field.GetWidthInCells(); x++)
            {
                for (int y = 0; y < _gameModel.Field.GetHeightInCells(); y++)
                {
                    var cell = _gameModel.Field.CellField[x, y];

                    // Если ячейка содержит объект и он еще не нарисован
                    if (cell.StaticObject != null)
                    {
                        if (!_fieldStaticSprites.ContainsKey((x, y)))
                        {
                            var image = new Image
                            {
                                Width = _gameModel.Field.CellSize,
                                Height = _gameModel.Field.CellSize,
                                Source = GetImageSource(cell.StaticObject)
                            };
                            Canvas.SetLeft(image, x * _gameModel.Field.CellSize);
                            Canvas.SetTop(image, y * _gameModel.Field.CellSize);
                            _gameCanvas.Children.Add(image);

                            _fieldStaticSprites[(x, y)] = image;
                        }
                    }
                    else
                    {
                        if (_fieldStaticSprites.ContainsKey((x, y)))
                        {
                            var image = _fieldStaticSprites[(x, y)];
                            _gameCanvas.Children.Remove(image);
                            _fieldStaticSprites.Remove((x, y));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получить путь к спрайту для блока
        /// </summary>
        /// <param name="parAbstractStaticObject">Блок игрового поля</param>
        /// <returns></returns>
        private ImageSource GetImageSource(AbstractStaticObject parAbstractStaticObject)
        {
            if (parAbstractStaticObject is Wall)
            {
                return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/wall.png"));
            }
            if (parAbstractStaticObject is IndestructibleWall)
            {
                return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/indestructible_wall.png"));
            }
            if (parAbstractStaticObject is Bush)
            {
                return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/bush.png"));
            }
            if (parAbstractStaticObject is Water)
            {
                return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/water.png"));
            }
            if (parAbstractStaticObject is Staff)
            {
                return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/staff.png"));
            }
            return new BitmapImage(new Uri("pack://application:,,,/WPFView;component/Sprites/water.png"));
        }



        /// <summary>
        /// Событие перемещения объекта
        /// </summary>
        /// <param name="parObj">Перемещаемый объект</param>
        /// <param name="parNewPosition">Новые координаты объекта</param>
        private void OnObjectMoved(AbstractDynamicObject parObj, Point parNewPosition)
        {
            if (parObj is AbstractTank)
            {
                DrawTank((AbstractTank)parObj, ((AbstractTank)parObj).Id == _gameModel.Player.PlayerTank.Id);
            }
            else
            {
                DrawBullet((Bullet)parObj);
            }
        }

        /// <summary>
        /// Анимация взрыва танка
        /// </summary>
        /// <param name="parPosition">Координаты для взрыва</param>
        private void PlayTankExplosionAnimation(Point parPosition)
        {
            var explosion = new Ellipse
            {
                Width = 50,
                Height = 50,
                Fill = Brushes.Orange,
                Opacity = 1
            };

            Canvas.SetLeft(explosion, parPosition.X - 25);
            Canvas.SetTop(explosion, parPosition.Y - 25);
            _gameCanvas.Children.Add(explosion);

            var scaleTransform = new ScaleTransform(1, 1);
            explosion.RenderTransform = scaleTransform;

            var scaleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(0.2),
                AutoReverse = true
            };

            var opacityAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                AutoReverse = false
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

            explosion.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

            opacityAnimation.Completed += (s, e) => _gameCanvas.Children.Remove(explosion);
        }

        /// <summary>
        /// Анимация взрыва пули
        /// </summary>
        /// <param name="parPosition">Координаты для взрыва</param>
        private void PlayBulletExplosionAnimation(Point parPosition)
        {
            var explosion = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = Brushes.Red,
                Opacity = 1
            };

            Canvas.SetLeft(explosion, parPosition.X - 5);
            Canvas.SetTop(explosion, parPosition.Y - 5);
            _gameCanvas.Children.Add(explosion);

            var scaleTransform = new ScaleTransform(1, 1);
            explosion.RenderTransform = scaleTransform;

            var scaleAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(0.1),
                AutoReverse = true
            };

            var opacityAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleAnimation);

            explosion.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);

            opacityAnimation.Completed += (s, e) => _gameCanvas.Children.Remove(explosion);
        }


    }
}

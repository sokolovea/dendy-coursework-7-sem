using Engine.DynamicObjects.Tank;
using Engine.DynamicObjects;
using Engine.Field;
using System.Threading.Tasks;
using Engine.StaticObjects;

namespace TestTank
{
    /// <summary>
    /// Тесты для танков
    /// </summary>
    public class UnitTestTank
    {
        /// <summary>
        /// Проверяет факт создания танка с валидными параметрами по умолчанию
        /// </summary>
        [Fact]
        public void DefaultPropertiesAreSetCorrectly()
        {
            var tank = new GeneralTank(new Point(0, 0));

            Assert.Equal(AbstractTank.DEFAULT_SPEED, tank.Speed);
            Assert.Equal(AbstractTank.DEFAULT_FIRING_RATE, tank.FiringRate);
            Assert.Equal(AbstractTank.DEFAULT_DAMAGE, tank.Damage);
            Assert.Equal(AbstractTank.DEFAULT_HEALTH, tank.Health);
        }

        /// <summary>
        /// Проверяет факт нанесения урона танку снарядом
        /// </summary>
        [Fact]
        public void ApplyDamageReducesHealthCorrectly()
        {
            var tank = new GeneralTank(new Point(0, 0));
            var bullet = new Bullet(new Point(0, 0), DirectionEnum.Up, Guid.NewGuid(), 20);
            tank.ApplyDamage(bullet);
            Assert.Equal(80, tank.Health);
        }


        /// <summary>
        /// Проверяет, что снаряд не может поразить родительский танк
        /// </summary>
        [Fact]
        public void ApplyDamageByParentTankShouldNotBe()
        {
            var tank = new GeneralTank(new Point(0, 0));
            tank.Health = 100;
            var bullet = new Bullet(new Point(), DirectionEnum.Right, tank.Id, 20);
            if (bullet == null)
            {
                Assert.Fail();
            }
            tank.ApplyDamage(bullet);
            Assert.Equal(100, tank.Health);
        }


        /// <summary>
        /// Проверяет, что здоровье танка после нанесения урона не может стать меньше 0
        /// </summary>
        [Fact]
        public void ApplyDamageDoesNotReduceHealthBelowZero()
        {
            var tank = new GeneralTank(new Point(0, 0));
            tank.Health = 5;
            var bullet = new Bullet(new Point(0, 0), DirectionEnum.Up, Guid.NewGuid(), 200);
            bullet.Damage = 10;
            tank.ApplyDamage(bullet);
            Assert.Equal(0, tank.Health);
        }

        /// <summary>
        /// Проверка на то, что танк может выпустить хотя бы один снаряд без ограничений на максимальную скорострельность
        /// </summary>
        [Fact]
        public void FireCreatesBulletWhenCooldownAllows()
        {
            var tank = new GeneralTank(new Point(0, 0));
            var bullet = tank.Fire();
            Assert.NotNull(bullet);
        }


        /// <summary>
        /// Проверка на ограничение скорострельности танка
        /// </summary>
        [Fact]
        public void FireShouldNotCreateBulletWhenCooldown()
        {
            var tank = new GeneralTank(new Point(0, 0));
            tank.Fire();
            var bulletSecond = tank.Fire();
            Assert.Null(bulletSecond);
        }

        /// <summary>
        /// Проверка на то, что спустя FireCoolDown времени танк может выстрелить снова
        /// </summary>
        [Fact]
        public void FireShouldCreateBulletAfterCooldown()
        {
            var tank = new GeneralTank(new Point(0, 0));
            var bulletFirst = tank.Fire();
            Assert.NotNull(bulletFirst);
            bulletFirst.OnObjectDestroyed();
            Thread.Sleep((int)(tank.FireCooldown.TotalMilliseconds * 1.1));
            var bulletSecond = tank.Fire();
            Assert.NotNull(bulletSecond);
        }

        /// <summary>
        /// Проверяет, что танк не стреляет, если он уничтожен
        /// </summary>
        [Fact]
        public void DestroyedTankCannotFire()
        {
            var tank = new GeneralTank(new Point(5, 5)) { Health = 0 };
            var bullet = tank.Fire();
            Assert.Null(bullet);
        }

        /// <summary>
        /// Проверка на ограничение максимального количества активных (летящих) снарядов для танка
        /// </summary>
        [Fact]
        public void UpdateActiveBulletsCounterDecreasesBulletCount()
        {
            var tank = new GeneralTank(new Point(0, 0));
            var bullet = tank.Fire();
            Assert.NotNull(bullet);
            Thread.Sleep((int)(tank.FireCooldown.TotalMilliseconds * 1.1));
            bullet.OnObjectDestroyed();
            bullet = tank.Fire();
            Assert.NotNull(bullet);
            Thread.Sleep((int)(tank.FireCooldown.TotalMilliseconds * 1.1));
            bullet = tank.Fire();
            Assert.Null(bullet);
        }


        /// <summary>
        /// Проверка на то, что танк правильно определяет направление к заданной точке
        /// </summary>
        [Fact]
        public void GetDirectionToPointRightDirection()
        {
            var tank = new GeneralTank(new Point(2, 2));
            var direction = tank.GetDirectionToPoint(new Point(5, 2));
            Assert.Equal(DirectionEnum.Right, direction);
        }

        /// <summary>
        /// Проверка на то, что танк может находить путь к цели на пустом поле
        /// </summary>
        [Fact]
        public void FindPathWithLeePathFound()
        {
            var field = new GameField(10, 10, 1);
            var tank = new GeneralTank(new Point(2, 2));
            var path = tank.FindPathWithLee(field, new Point(0, 0), new Point(5, 5), 1, 1);
            Assert.NotNull(path);
        }

        /// <summary>
        /// Проверка на то, что танк может находить путь к цели при наличии преграды с брешью
        /// </summary>
        [Fact]
        public void FindPathWithLeePathFoundWithPartialObstacle()
        {
            var field = new GameField(5, 5, 1);
            field.CellField[1, 0].StaticObject = new Water();
            field.CellField[1, 1].StaticObject = new Water();
            //брешь
            field.CellField[1, 3].StaticObject = new Water();
            field.CellField[1, 4].StaticObject = new Water();
            var tank = new GeneralTank(new Point());
            var path = tank.FindPathWithLee(field, new Point(0, 0), new Point(4, 4), 1, 1);
            Assert.NotNull(path);
        }

        /// <summary>
        /// Проверка на то, что танк не может находить путь к цели при наличии сплошной преграды
        /// </summary>
        [Fact]
        public void FindPathWithLeeNoPathFound()
        {
            var field = new GameField(5, 5, 1);
            field.CellField[1, 0].StaticObject = new Water();
            field.CellField[1, 1].StaticObject = new Water();
            field.CellField[1, 2].StaticObject = new Water();
            field.CellField[1, 3].StaticObject = new Water();
            field.CellField[1, 4].StaticObject = new Water();
            var tank = new GeneralTank(new Point());
            var path = tank.FindPathWithLee(field, new Point(0, 0), new Point(4, 4), 1, 1);
            Assert.Null(path);
        }

        /// <summary>
        /// Проверка на то, что пустая ячейка является валидной для размещения танка
        /// </summary>
        [Fact]
        public void IsValidCellForTankValidCell()
        {
            var field = new GameField(10, 10, 1);
            var tank = new GeneralTank(new Point());
            var result = tank.IsValidCellForTank(field, new Point(5, 5), 1, 1);
            Assert.True(result);
        }

        /// <summary>
        /// Проверка на то, что ячейка за пределами поля не является валидной для размещения танка
        /// </summary>
        [Fact]
        public void IsValidCellForTankInvalidCellOutOfBounds()
        {
            var field = new GameField(10, 10, 1);
            var tank = new GeneralTank(new Point());
            var result = tank.IsValidCellForTank(field, new Point(15, 15), 1, 1);
            Assert.False(result);
        }

        /// <summary>
        /// Проверка на то, что препятствие не является валидной клеткой для размещения танка
        /// </summary>
        [Fact]
        public void IsValidCellForTankInvalidCellObstacle()
        {
            var field = new GameField(10, 10, 1);
            field.CellField[5, 5].StaticObject = new Wall();
            var tank = new GeneralTank(new Point());
            var result = tank.IsValidCellForTank(field, new Point(5, 5), 1, 1);
            Assert.False(result);
        }


        /// <summary>
        /// Проверка на то, что другая точка находится в зоне поражения, если между точками нет препятствий
        /// </summary>
        [Fact]
        public void HasDirectLineOfSightClearLine()
        {
            var field = new GameField(10, 10, 1);
            var tank = new GeneralTank(new Point());
            var result = tank.HasDirectLineOfSight(field, new Point(0, 0), new Point(5, 5));
            Assert.True(result);
        }


        /// <summary>
        /// Проверка на то, что разрушимая стена на прямом пути к цели
        /// может потенциально быть уничтожена танком, т.е цель за ней может быть поражена
        /// </summary>
        [Fact]
        public void HasDirectLineOfSightDoesNotBlockedByWall()
        {
            var field = new GameField(1, 10, 1);
            field.CellField[0, 2].StaticObject = new Wall();
            var tank = new GeneralTank(new Point());
            var result = tank.HasDirectLineOfSight(field, new Point(0, 0), new Point(0, 5));
            Assert.True(result);
        }

        /// <summary>
        /// Проверка на то, что неразрушимая стена на прямом пути к цели
        /// сигнализирует о невозможности поражения ее пулей
        /// </summary>
        [Fact]
        public void HasDirectLineOfSightBlockedLine()
        {
            var field = new GameField(1, 10, 1);
            field.CellField[0, 2].StaticObject = new IndestructibleWall();
            var tank = new GeneralTank(new Point());
            var result = tank.HasDirectLineOfSight(field, new Point(0, 0), new Point(0, 5));
            Assert.False(result);
        }

        /// <summary>
        /// Изменение направления для танка выполняется правильно
        /// </summary>
        [Fact]
        public void ChangeDirectionChangesToNewDirection()
        {
            var tank = new GeneralTank(new Point(5, 5)) { Direction = DirectionEnum.Up };
            tank.ChangeDirection(new Point(5, 8));
            Assert.Equal(DirectionEnum.Down, tank.Direction);
        }


        /// <summary>
        /// Тест на то, что после задержки танк может поменять направление
        /// </summary>
        [Fact]
        public void TestTankCanChangeDirectionAfterPause()
        {
            var tank = new GeneralTank(new Point(0, 0));
            tank.ChangeDirection(new Point(10, 10));
            var initialDirection = tank.Direction;
            Thread.Sleep(tank.DirectionChangeCooldown.Milliseconds + 100);
            tank.ChangeDirection(new Point(-10, -10));
            var directionAfterChange = tank.Direction;
            Assert.NotEqual(initialDirection, directionAfterChange);
        }


        /// <summary>
        /// Счетчик активных снарядов у танка не может стать меньше 0
        /// </summary>
        [Fact]
        public void UpdateActiveBulletsCounterSubtractToZero()
        {
            var tank = new GeneralTank(new Point(5, 5));
            tank.ActiveBullets = 1;
            tank.UpdateActiveBulletsCounter();
            tank.UpdateActiveBulletsCounter();
            tank.UpdateActiveBulletsCounter();
            Assert.Equal(0, tank.ActiveBullets);
        }
    }
}
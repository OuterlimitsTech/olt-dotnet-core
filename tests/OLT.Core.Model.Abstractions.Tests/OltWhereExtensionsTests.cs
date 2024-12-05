using Moq;
using OLT.Core;

namespace System.Linq.Tests
{
    public class OltWhereExtensionsTests
    {
        public class TestEntity : IOltEntity
        {
            public int Id { get; set; }
        }

        public class TestEntitySearcher : OltSearcher<TestEntity>
        {
            private readonly int _id;

            public TestEntitySearcher(int id)
            {
                _id = id;
            }

            public override IQueryable<TestEntity> BuildQueryable(IQueryable<TestEntity> queryable)
            {
                return queryable.Where(p => p.Id == _id);
            }
        }

      

        [Fact]
        public void Where_WithSingleSearcher_ShouldApplySearcher()
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 }
            }.AsQueryable();

            var searcher = new TestEntitySearcher(1);

            // Act
            var result = entities.Where(searcher);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
        }


        public class TestEntityGreaterThanSearcher : OltSearcher<TestEntity>
        {
            private readonly int _startId;

            public TestEntityGreaterThanSearcher(int startId)
            {
                _startId = startId;
            }

            public override IQueryable<TestEntity> BuildQueryable(IQueryable<TestEntity> queryable)
            {
                return queryable.Where(p => p.Id > _startId);
            }
        }


        public class TestEntityLessThanSearcher : OltSearcher<TestEntity>
        {
            private readonly int _endId;

            public TestEntityLessThanSearcher(int endId)
            {
                _endId = endId;
            }

            public override IQueryable<TestEntity> BuildQueryable(IQueryable<TestEntity> queryable)
            {
                return queryable.Where(p => p.Id < _endId);
            }
        }

        [Fact]
        public void Where_WithMultipleSearchers_ShouldApplyAllSearchers()
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 },
                new TestEntity { Id = 4 },
                new TestEntity { Id = 5 },
            }.AsQueryable();


            // Act
            var result = entities.Where(new TestEntityGreaterThanSearcher(1), new TestEntityLessThanSearcher(3));

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().Id);
        }


        
        [Fact]
        public void Where_Exceptions()
        {
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = 1 },
                new TestEntity { Id = 2 },
                new TestEntity { Id = 3 },
                new TestEntity { Id = 4 },
                new TestEntity { Id = 5 },
            }.AsQueryable();

            
            IOltSearcher<TestEntity>[] searchers = null;

            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where(null, searchers));
            Assert.Throws<ArgumentNullException>(() => entities.Where(searchers));


            IOltSearcher<TestEntity> searcher = null;
            Assert.Throws<ArgumentNullException>(() => OltWhereExtensions.Where(null, searcher));
            Assert.Throws<ArgumentNullException>(() => entities.Where(searcher));
        }

    }
}

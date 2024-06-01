// using GeekOff.Data;
// using NSubstitute;

// public class SharedFixture
// {
//     public _contextGo MockContext { get; private set; }

//     public SharedFixture()
//     {
//         MockContext = Substitute.For<_contextGo>();

//         // Seed data
//         var testData = new List<YourEntity>
//         {
//             new YourEntity { Id = 1, Name = "Entity 1" },
//             new YourEntity { Id = 2, Name = "Entity 2" },
//             // Add more seed data as needed
//         };

//         MockContext.YourEntities.Returns(testData);
//     }
// }
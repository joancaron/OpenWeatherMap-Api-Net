using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenWeatherMap.Tests
{
    [TestFixture]
    public class SearchClientTests : OpenWeatherMapTestsBase
    {
        [Test]
        public async Task Can_Get_Search_By_City_Name()
        {
            var result = await OpenWeatherMapTestClient.Search.GetByName(CityName);
            TestAllProperties(result);
            Assert.IsTrue(result.Mode == "name");
            Assert.IsTrue(result.List.FirstOrDefault(x => x.City.Name == CityName) != null);
        }

        [Test]
        public async Task Can_Get_Search_By_City_Name_MetricSystem()
        {
            var result = await OpenWeatherMapTestClient.Search.GetByName(CityName, MetricSystem.Metric);
            TestAllProperties(result);
            Assert.IsTrue(result.Mode == "name");
            Assert.IsTrue(result.List.FirstOrDefault(x => x.City.Name == CityName) != null);
            Assert.IsTrue(result.List.FirstOrDefault(x => x.Temperature.Unit == Celsius) != null);
        }

        [Test]
        public async Task Can_Get_Search_By_City_Name_MetricSystem_Language()
        {
            var resultIt = await OpenWeatherMapTestClient.Search.GetByName(CityName, MetricSystem.Metric, OpenWeatherMapLanguage.IT);
            var resultFr = await OpenWeatherMapTestClient.Search.GetByName(CityName, MetricSystem.Metric, OpenWeatherMapLanguage.FR);
            TestAllProperties(resultIt);
            TestAllProperties(resultFr);

            Assert.IsTrue(resultIt.Mode == "name");
            Assert.IsTrue(resultIt.List.FirstOrDefault(x => x.City.Name == CityName) != null);
            Assert.IsTrue(resultIt.List.FirstOrDefault(x => x.Temperature.Unit == Metric) != null);

            Assert.IsTrue(resultFr.Mode == "name");
            Assert.IsTrue(resultFr.List.FirstOrDefault(x => x.City.Name == CityName) != null);
            Assert.IsTrue(resultFr.List.FirstOrDefault(x => x.Temperature.Unit == Metric) != null);

            Assert.AreNotEqual(resultFr.List.First().Weather.Value, resultIt.List.First().Weather.Value);
        }

        [Test]
        public void Can_Get_Search_By_City_Name_Exception()
        {
            Assert.ThrowsAsync<OpenWeatherMapException>(() => OpenWeatherMapTestClient.CurrentWeather.GetByName("abcdefgh"));
        }

        [Test]
        public async Task Can_Get_Search_By_City_Coordinates()
        {
            var result = await OpenWeatherMapTestClient.Search.GetByCoordinates(new Coordinates { Latitude = Latitude, Longitude = Longitude });
            TestAllProperties(result);
            Assert.IsTrue(result.Mode == "center");
            Assert.IsTrue(result.List.FirstOrDefault(x => x.City.Name == CityName) != null);
        }

        [Test]
        public async Task Can_Get_Search_By_City_Coordinates_MetricSystem()
        {
            var result = await OpenWeatherMapTestClient.Search.GetByCoordinates(
                new Coordinates { Latitude = Latitude, Longitude = Longitude },
                MetricSystem.Metric);
            TestAllProperties(result);
            Assert.IsTrue(result.Mode == "center");
            Assert.IsTrue(result.List.FirstOrDefault(x => x.City.Name == CityName) != null);
            Assert.IsTrue(result.List.FirstOrDefault(x => x.Temperature.Unit == Celsius) != null);
        }

        [Test]
        public async Task Can_Get_Search_By_City_Coordinates_MetricSystem_Language()
        {
            var resultIt = await OpenWeatherMapTestClient.Search.GetByCoordinates(
                new Coordinates { Latitude = Latitude, Longitude = Longitude },
                MetricSystem.Metric,
                OpenWeatherMapLanguage.IT);

            var resultFr = await OpenWeatherMapTestClient.Search.GetByCoordinates(
                new Coordinates { Latitude = Latitude, Longitude = Longitude },
                MetricSystem.Metric,
                OpenWeatherMapLanguage.FR);

            TestAllProperties(resultIt);
            Assert.IsTrue(resultIt.Mode == "center");
            Assert.IsTrue(resultIt.List.FirstOrDefault(x => x.City.Name == CityName) != null);

            TestAllProperties(resultFr);
            Assert.IsTrue(resultFr.Mode == "center");
            Assert.IsTrue(resultFr.List.FirstOrDefault(x => x.City.Name == CityName) != null);

            Assert.AreNotEqual(resultIt.List.First().Weather.Value, resultFr.List.First().Weather.Value);
        }

        [Test]
        public void Can_Get_Search_By_Coordinates_Exception()
        {
            Assert.ThrowsAsync<OpenWeatherMapException>(() => OpenWeatherMapTestClient.CurrentWeather.GetByCoordinates(
                new Coordinates { Latitude = -9999, Longitude = -9999 }));
        }

        [Test]
        public async Task Can_Get_Search_Count()
        {
            var result = await OpenWeatherMapTestClient.Search.GetByCoordinates(
                new Coordinates() { Longitude = -2.15, Latitude = 57 },
                MetricSystem.Internal,
                OpenWeatherMapLanguage.EN,
                2);
            TestAllProperties(result);
            Assert.IsTrue(result.List.Count() == 2);
        }

        [Test]
        public async Task Can_Get_Search_Accurate()
        {
            var resultAccurate = await OpenWeatherMapTestClient.Search.GetByName(
                CityName,
                MetricSystem.Internal,
                OpenWeatherMapLanguage.EN,
                null,
                Accuracy.Accurate);

            var resultLike = await OpenWeatherMapTestClient.Search.GetByName(
                CityName,
                MetricSystem.Internal,
                OpenWeatherMapLanguage.EN,
                null,
                Accuracy.Like);

            TestAllProperties(resultAccurate);
            TestAllProperties(resultLike);

            Assert.IsTrue(resultAccurate.List.Count() == 2 && resultLike.List.Count() == 4);
        }

        public void TestAllProperties(SearchResponse response)
        {
            Assert.IsNotNull(response.Count);
            Assert.IsNotNull(response.List);
            Assert.IsNotNull(response.List);
            Assert.IsNotNull(response.Mode);
        }
    }
}

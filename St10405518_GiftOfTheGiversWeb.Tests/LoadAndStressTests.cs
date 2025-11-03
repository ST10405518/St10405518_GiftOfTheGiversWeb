using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace St10405518_GiftOfTheGiversWeb.Tests.LoadTesting
{
    [TestClass]
    public class LoadAndStressTests
    {
        private HttpClient _client;
        private string _baseUrl = "http://localhost:5136"; // YOUR CORRECT URL!

        [TestInitialize]
        public void Setup()
        {
            // Bypass SSL for development
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            _client = new HttpClient(handler);
            _client.Timeout = TimeSpan.FromSeconds(30);
        }

        [TestMethod]
        public async Task LoadTest_HomePage_20ConcurrentUsers()
        {
            // Arrange
            var tasks = new List<Task>();
            var responseTimes = new ConcurrentBag<long>();
            var errors = new ConcurrentBag<string>();
            int concurrentUsers = 20; // Reduced for stability
            var stopwatch = new Stopwatch();

            Debug.WriteLine($"=== LOAD TEST STARTING ===");
            Debug.WriteLine($"Target URL: {_baseUrl}");
            Debug.WriteLine($"Users: {concurrentUsers}");

            // Act - Simulate multiple users
            stopwatch.Start();

            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var requestStopwatch = Stopwatch.StartNew();
                    try
                    {
                        var response = await _client.GetAsync(_baseUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            requestStopwatch.Stop();
                            responseTimes.Add(requestStopwatch.ElapsedMilliseconds);
                            Debug.WriteLine($"✅ Request succeeded: {requestStopwatch.ElapsedMilliseconds}ms");
                        }
                        else
                        {
                            errors.Add($"HTTP Error: {response.StatusCode}");
                            Debug.WriteLine($"❌ HTTP Error: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Exception: {ex.Message}");
                        Debug.WriteLine($"❌ Exception: {ex.Message}");
                    }
                }));
            }

            // Wait for all requests to complete
            await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Calculate results
            var successRate = (responseTimes.Count * 100.0) / concurrentUsers;
            var averageResponseTime = responseTimes.Any() ? responseTimes.Average() : 0;
            var maxResponseTime = responseTimes.Any() ? responseTimes.Max() : 0;
            var minResponseTime = responseTimes.Any() ? responseTimes.Min() : 0;

            // Output results
            Debug.WriteLine($"=== LOAD TEST RESULTS (20 Users) ===");
            Debug.WriteLine($"Total Users: {concurrentUsers}");
            Debug.WriteLine($"Successful Requests: {responseTimes.Count}");
            Debug.WriteLine($"Failed Requests: {errors.Count}");
            Debug.WriteLine($"Success Rate: {successRate:F2}%");
            Debug.WriteLine($"Average Response Time: {averageResponseTime:F2}ms");
            Debug.WriteLine($"Max Response Time: {maxResponseTime}ms");
            Debug.WriteLine($"Min Response Time: {minResponseTime}ms");
            Debug.WriteLine($"Total Test Duration: {stopwatch.Elapsed.TotalSeconds:F2}s");

            // Show sample errors
            if (errors.Any())
            {
                Debug.WriteLine("Sample Errors:");
                foreach (var error in errors.Take(3))
                {
                    Debug.WriteLine($"  - {error}");
                }
            }

            // Assertions
            Assert.IsTrue(successRate >= 80, $"Success rate too low: {successRate:F2}%");
            Assert.IsTrue(averageResponseTime < 5000, $"Response time too high: {averageResponseTime:F2}ms");
        }

        [TestMethod]
        public async Task StressTest_HomePage_50ConcurrentUsers()
        {
            // Arrange
            var tasks = new List<Task>();
            var responseTimes = new ConcurrentBag<long>();
            var errors = new ConcurrentBag<string>();
            int concurrentUsers = 50; // Stress test
            var stopwatch = new Stopwatch();

            Debug.WriteLine($"=== STRESS TEST STARTING ===");
            Debug.WriteLine($"Target URL: {_baseUrl}");
            Debug.WriteLine($"Users: {concurrentUsers}");

            // Act - Simulate stress conditions
            stopwatch.Start();

            for (int i = 0; i < concurrentUsers; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var requestStopwatch = Stopwatch.StartNew();
                    try
                    {
                        var response = await _client.GetAsync(_baseUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            requestStopwatch.Stop();
                            responseTimes.Add(requestStopwatch.ElapsedMilliseconds);
                        }
                        else
                        {
                            errors.Add($"HTTP Error: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        errors.Add($"Exception: {ex.Message}");
                    }
                }));
            }

            await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Calculate results
            var successRate = (responseTimes.Count * 100.0) / concurrentUsers;
            var averageResponseTime = responseTimes.Any() ? responseTimes.Average() : 0;

            Debug.WriteLine($"=== STRESS TEST RESULTS (50 Users) ===");
            Debug.WriteLine($"Total Users: {concurrentUsers}");
            Debug.WriteLine($"Successful Requests: {responseTimes.Count}");
            Debug.WriteLine($"Failed Requests: {errors.Count}");
            Debug.WriteLine($"Success Rate: {successRate:F2}%");
            Debug.WriteLine($"Average Response Time: {averageResponseTime:F2}ms");
            Debug.WriteLine($"Total Test Duration: {stopwatch.Elapsed.TotalSeconds:F2}s");

            // More lenient assertion for stress test
            Assert.IsTrue(successRate >= 60, $"Stress test success rate too low: {successRate:F2}%");
        }

        [TestMethod]
        public async Task LoadTest_MultipleEndpoints_15ConcurrentUsers()
        {
            // Test multiple pages
            var endpoints = new[]
            {
                "/",
                "/Home/About",
                "/Home/Contact"
            };

            var tasks = new List<Task>();
            var responseTimes = new ConcurrentBag<long>();
            int usersPerEndpoint = 5; // 5 users × 3 endpoints = 15 total users

            Debug.WriteLine($"=== MULTI-ENDPOINT LOAD TEST ===");

            foreach (var endpoint in endpoints)
            {
                for (int i = 0; i < usersPerEndpoint; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        var requestStopwatch = Stopwatch.StartNew();
                        try
                        {
                            var response = await _client.GetAsync(_baseUrl + endpoint);
                            if (response.IsSuccessStatusCode)
                            {
                                requestStopwatch.Stop();
                                responseTimes.Add(requestStopwatch.ElapsedMilliseconds);
                                Debug.WriteLine($"✅ {endpoint} succeeded: {requestStopwatch.ElapsedMilliseconds}ms");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"❌ {endpoint} failed: {ex.Message}");
                        }
                    }));
                }
            }

            await Task.WhenAll(tasks);

            // Calculate results
            var totalRequests = endpoints.Length * usersPerEndpoint;
            var successRate = (responseTimes.Count * 100.0) / totalRequests;

            Debug.WriteLine($"=== MULTI-ENDPOINT RESULTS ===");
            Debug.WriteLine($"Total Requests: {totalRequests}");
            Debug.WriteLine($"Successful: {responseTimes.Count}");
            Debug.WriteLine($"Success Rate: {successRate:F2}%");

            Assert.IsTrue(successRate >= 70, $"Multi-endpoint success rate too low: {successRate:F2}%");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client?.Dispose();
        }
    }
}
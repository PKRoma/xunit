using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

public class XunitTestRunnerTests
{
	public class Messages
	{
		[Fact]
		public static async ValueTask Passing()
		{
			var messageBus = new SpyMessageBus();
			var attribute = new SpyBeforeAfterTest();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.Passing), @explicit: true, timeout: 42);
			var runner = new TestableXunitTestRunner(test, beforeAfterTestAttributes: [attribute], messageBus: messageBus, explicitOption: ExplicitOption.On);

			await runner.RunAsync();

			Assert.Null(runner.Aggregator.ToException());
			Assert.Collection(
				messageBus.Messages,
				msg =>
				{
					var starting = Assert.IsAssignableFrom<ITestStarting>(msg);
					verifyTestMessage(starting);
					Assert.True(starting.Explicit);
					Assert.Equal("test-display-name", starting.TestDisplayName);
					Assert.Equal(42, starting.Timeout);
					// Trait comes from an assembly-level trait attribute on this test assembly
					var trait = Assert.Single(starting.Traits);
					Assert.Equal("Assembly", trait.Key);
					var value = Assert.Single(trait.Value);
					Assert.Equal("Trait", value);
				},
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestClassConstructionStarting>(msg)),
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestClassConstructionFinished>(msg)),
				msg =>
				{
					var beforeStarting = Assert.IsAssignableFrom<IBeforeTestStarting>(msg);
					verifyTestMessage(beforeStarting);
					Assert.Equal("SpyBeforeAfterTest", beforeStarting.AttributeName);
				},
				msg =>
				{
					var beforeFinished = Assert.IsAssignableFrom<IBeforeTestFinished>(msg);
					verifyTestMessage(beforeFinished);
					Assert.Equal("SpyBeforeAfterTest", beforeFinished.AttributeName);
				},
				// Test method is invoked here
				msg =>
				{
					var afterStarting = Assert.IsAssignableFrom<IAfterTestStarting>(msg);
					verifyTestMessage(afterStarting);
					Assert.Equal("SpyBeforeAfterTest", afterStarting.AttributeName);
				},
				msg =>
				{
					var afterFinished = Assert.IsAssignableFrom<IAfterTestFinished>(msg);
					verifyTestMessage(afterFinished);
					Assert.Equal("SpyBeforeAfterTest", afterFinished.AttributeName);
				},
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestClassDisposeStarting>(msg)),
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestClassDisposeFinished>(msg)),
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestPassed>(msg)),
				msg => verifyTestMessage(Assert.IsAssignableFrom<ITestFinished>(msg))
			);

			static void verifyTestMessage(ITestMessage testMethod)
			{
				Assert.Equal("assembly-id", testMethod.AssemblyUniqueID);
				Assert.Equal("test-case-id", testMethod.TestCaseUniqueID);
				Assert.Equal("test-class-id", testMethod.TestClassUniqueID);
				Assert.Equal("test-collection-id", testMethod.TestCollectionUniqueID);
				Assert.Equal("test-method-id", testMethod.TestMethodUniqueID);
				Assert.Equal("test-id", testMethod.TestUniqueID);
			}
		}

		[Fact]
		public static async ValueTask StaticPassing()
		{
			var messageBus = new SpyMessageBus();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.StaticPassing));
			var runner = new TestableXunitTestRunner(test, messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				// Test method is invoked here
				msg => Assert.IsAssignableFrom<ITestPassed>(msg),
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask Failed()
		{
			var messageBus = new SpyMessageBus();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.Failing));
			var runner = new TestableXunitTestRunner(test, messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionFinished>(msg),
				// Test method is invoked here
				msg => Assert.IsAssignableFrom<ITestClassDisposeStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassDisposeFinished>(msg),
				msg =>
				{
					var failed = Assert.IsAssignableFrom<ITestFailed>(msg);
					Assert.Equal(-1, failed.ExceptionParentIndices.Single());
					Assert.Equal("Xunit.Sdk.TrueException", failed.ExceptionTypes.Single());
				},
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask SkippedViaAttribute()
		{
			var messageBus = new SpyMessageBus();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.SkippedViaAttribute));
			var runner = new TestableXunitTestRunner(test, messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg =>
				{
					var skipped = Assert.IsAssignableFrom<ITestSkipped>(msg);
					Assert.Equal("Don't run me", skipped.Reason);
				},
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask SkippedViaException()
		{
			var messageBus = new SpyMessageBus();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.SkippedViaException));
			var runner = new TestableXunitTestRunner(test, messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionFinished>(msg),
				// Test method is invoked here
				msg => Assert.IsAssignableFrom<ITestClassDisposeStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassDisposeFinished>(msg),
				msg =>
				{
					var skipped = Assert.IsAssignableFrom<ITestSkipped>(msg);
					Assert.Equal("This isn't a good time", skipped.Reason);
				},
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask NotRun()
		{
			var messageBus = new SpyMessageBus();
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.ExplicitTest));
			var runner = new TestableXunitTestRunner(test, messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestNotRun>(msg),
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask BeforeThrows()
		{
			var messageBus = new SpyMessageBus();
			var attribute = new SpyBeforeAfterTest { ThrowInBefore = true };
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.Failing));
			var runner = new TestableXunitTestRunner(test, beforeAfterTestAttributes: [attribute], messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionFinished>(msg),
				msg => Assert.IsAssignableFrom<IBeforeTestStarting>(msg),
				msg => Assert.IsAssignableFrom<IBeforeTestFinished>(msg),
				// No after messages because nothing to clean up, and test method is NOT invoked
				msg => Assert.IsAssignableFrom<ITestClassDisposeStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassDisposeFinished>(msg),
				msg =>
				{
					var failed = Assert.IsAssignableFrom<ITestFailed>(msg);
					Assert.Equal(-1, failed.ExceptionParentIndices.Single());
					Assert.Equal("SpyBeforeAfterTest+BeforeException", failed.ExceptionTypes.Single());
				},
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask AfterThrows()
		{
			var messageBus = new SpyMessageBus();
			var attribute = new SpyBeforeAfterTest { ThrowInAfter = true };
			var test = TestData.XunitTest<ClassUnderTest>(nameof(ClassUnderTest.Failing));
			var runner = new TestableXunitTestRunner(test, beforeAfterTestAttributes: [attribute], messageBus: messageBus);

			await runner.RunAsync();

			Assert.Collection(
				messageBus.Messages,
				msg => Assert.IsAssignableFrom<ITestStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassConstructionFinished>(msg),
				msg => Assert.IsAssignableFrom<IBeforeTestStarting>(msg),
				msg => Assert.IsAssignableFrom<IBeforeTestFinished>(msg),
				// Test method is invoked here
				msg => Assert.IsAssignableFrom<IAfterTestStarting>(msg),
				msg => Assert.IsAssignableFrom<IAfterTestFinished>(msg),
				msg => Assert.IsAssignableFrom<ITestClassDisposeStarting>(msg),
				msg => Assert.IsAssignableFrom<ITestClassDisposeFinished>(msg),
				msg =>
				{
					var failed = Assert.IsAssignableFrom<ITestFailed>(msg);
					Assert.Equivalent(new[] { -1, 0, 0 }, failed.ExceptionParentIndices);
					Assert.Collection(
						failed.ExceptionTypes,
						type => Assert.Equal("System.AggregateException", type),
						type => Assert.Equal("Xunit.Sdk.TrueException", type),
						type => Assert.Equal("SpyBeforeAfterTest+AfterException", type)
					);
				},
				msg => Assert.IsAssignableFrom<ITestFinished>(msg)
			);
		}

		[Fact]
		public static async ValueTask BeforeAfterExecutionOrder()
		{
			var messages = new List<string>();
			var attribute1 = new RecordingBeforeAfter(messages, 1);
			var attribute2 = new RecordingBeforeAfter(messages, 2);
			var test = TestData.XunitTest<RecordingTestClass>(nameof(RecordingTestClass.ExecutionRecorder));
			var invoker = new TestableXunitTestRunner(test, beforeAfterTestAttributes: [attribute1, attribute2], constructorArguments: [messages]);

			await invoker.RunAsync();

			Assert.Collection(messages,
				msg => Assert.Equal("Before #1", msg),
				msg => Assert.Equal("Before #2", msg),
				msg => Assert.Equal("Test method invocation", msg),
				msg => Assert.Equal("After #2", msg),
				msg => Assert.Equal("After #1", msg)
			);
		}

		class ClassUnderTest : IDisposable
		{
			public void Dispose() { }

			[Fact]
			public async Task Passing() => await Task.Yield();

			[Fact]
			public static void StaticPassing() { }

			[Fact]
			public void Failing() => Assert.True(false);

			[Fact(Skip = "Don't run me")]
			public void SkippedViaAttribute() { }

			[Fact]
			public void SkippedViaException() => Assert.Skip("This isn't a good time");

			[Fact(Explicit = true)]
			public void ExplicitTest() => Assert.Fail("Should not run");
		}

#pragma warning disable xUnit1041 // We push this argument in by hand rather than relying on fixtures
		class RecordingTestClass(List<string> messages)
#pragma warning restore xUnit1041
		{
			[Fact]
			public void ExecutionRecorder() => messages.Add("Test method invocation");
		}

		class RecordingBeforeAfter(List<string> messages, int identifier) : BeforeAfterTestAttribute
		{
			public override ValueTask After(MethodInfo methodUnderTest, IXunitTest test)
			{
				messages.Add("After #" + identifier);
				return default;
			}

			public override ValueTask Before(MethodInfo methodUnderTest, IXunitTest test)
			{
				messages.Add("Before #" + identifier);
				return default;
			}
		}
	}

	class TestableXunitTestRunner(
		IXunitTest test,
		IReadOnlyCollection<IBeforeAfterTestAttribute>? beforeAfterTestAttributes = null,
		object?[]? constructorArguments = null,
		ExplicitOption? explicitOption = null,
		IMessageBus? messageBus = null,
		object?[]? testMethodArguments = null) :
			XunitTestRunner
	{
		readonly IReadOnlyCollection<IBeforeAfterTestAttribute> beforeAfterTestAttributes = beforeAfterTestAttributes ?? [];
		readonly object?[] constructorArguments = constructorArguments ?? [];
		readonly ExplicitOption explicitOption = explicitOption ?? ExplicitOption.Off;
		readonly IMessageBus messageBus = messageBus ?? new SpyMessageBus();
		readonly IXunitTest test = test;
		readonly object?[] testMethodArguments = testMethodArguments ?? [];

		public readonly ExceptionAggregator Aggregator = new();
		public readonly CancellationTokenSource TokenSource = new();

		public ValueTask<RunSummary> RunAsync() =>
			RunAsync(test, messageBus, constructorArguments, testMethodArguments, test.TestCase.SkipReason, explicitOption, Aggregator, TokenSource, beforeAfterTestAttributes);
	}
}

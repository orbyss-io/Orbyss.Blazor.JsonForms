using Orbyss.Components.JsonForms.Context.Notifications;
using System.Collections;
using System.Reflection;

namespace Orbyss.Components.JsonForms.Tests.Context.Notifications
{
    [TestFixture]
    public class JsonFormNotificationHandlerTests
    {
        [Test]
        public void When_Subscribe_Then_Adds_Subscriber()
        {
            // Arrange
            var sut = new JsonFormNotificationHandler();

            // Pre-Assert
            AssertSubscribersLength(sut, 0);

            // Act
            _ = sut.Subscribe(JsonFormNotificationType.OnLanguageChanged, () => { });

            // Assert
            AssertSubscribersLength(sut, 1);
        }

        [Test]
        public void When_DisposeSubscriptionToken_Then_RemovesSubscriber()
        {
            // Arrange
            var sut = new JsonFormNotificationHandler();
            var subscriptionToken = sut.Subscribe(JsonFormNotificationType.OnLanguageChanged, () => { });

            // Pre-Assert
            AssertSubscribersLength(sut, 1);

            // Act
            subscriptionToken.Dispose();

            // Assert
            AssertSubscribersLength(sut, 0);
        }

        [Test]
        public void When_Notify_Then_InvokesCallback()
        {
            // Arrange
            int assertion = 0;
            var sut = new JsonFormNotificationHandler();
            _ = sut.Subscribe(JsonFormNotificationType.OnLanguageChanged, () => { assertion = 11; });

            // Act
            sut.Notify(JsonFormNotificationType.OnLanguageChanged);

            // Assert
            Assert.That(assertion, Is.EqualTo(11));
        }

        private static void AssertSubscribersLength(JsonFormNotificationHandler sut, int expectedLength)
        {
            var dictionaryObject = sut
                .GetType()
                .GetField("_subscribers", BindingFlags.NonPublic | BindingFlags.Instance)?
                .GetValue(sut)
                as IDictionary;

            Assert.That(dictionaryObject, Is.Not.Null);
            Assert.That(dictionaryObject, Has.Count.EqualTo(expectedLength));
        }
    }
}
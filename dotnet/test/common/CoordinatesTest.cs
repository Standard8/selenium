﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium.Environment;

namespace OpenQA.Selenium
{
    [TestFixture]
    public class CoordinatesTest : DriverTestFixture
    {
        [Test]
        public void ShouldGetCoordinatesOfAnElementInViewPort()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/simple_page.html");
            Assert.AreEqual(new Point(10, 10), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public void ShouldGetCoordinatesOfAnEmptyElement()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_empty_element.html");
            Assert.AreEqual(new Point(10, 10), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public void ShouldGetCoordinatesOfATransparentElement()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_transparent_element.html");
            Assert.AreEqual(new Point(10, 10), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public void ShouldGetCoordinatesOfAHiddenElement()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_hidden_element.html");
            Assert.AreEqual(new Point(10, 10), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public void ShouldGetCoordinatesOfAnInvisibleElement()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_invisible_element.html");
            Assert.AreEqual(new Point(0, 0), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        [IgnoreBrowser(Browser.Chrome, "ChromeDriver2 does not scroll correctly yet")]
        public void ShouldScrollPageAndGetCoordinatesOfAnElementThatIsOutOfViewPort()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_element_out_of_view.html");
            int windowHeight = driver.Manage().Window.Size.Height;
            Point location = GetLocationInViewPort(By.Id("box"));
            Assert.AreEqual(10, location.X);
            Assert.GreaterOrEqual(location.Y, 0);
            Assert.LessOrEqual(location.Y, windowHeight - 100);
        }

        [Test]
        public void ShouldGetCoordinatesOfAnElementInAFrame()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_frame.html");
            driver.SwitchTo().Frame("ifr");
            IWebElement box = driver.FindElement(By.Id("box"));
            Assert.AreEqual(new Point(10, 10), box.Location);
        }

        [Test]
        public void ShouldGetCoordinatesInViewPortOfAnElementInAFrame()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_frame.html");
            driver.SwitchTo().Frame("ifr");
            Assert.AreEqual(new Point(25, 25), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        public void ShouldGetCoordinatesInViewPortOfAnElementInANestedFrame()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/element_in_nested_frame.html");
            driver.SwitchTo().Frame("ifr");
            driver.SwitchTo().Frame("ifr");
            Assert.AreEqual(new Point(40, 40), GetLocationInViewPort(By.Id("box")));
        }

        [Test]
        [IgnoreBrowser(Browser.Firefox)]
        [IgnoreBrowser(Browser.Safari)]
        public void ShouldGetCoordinatesOfAnElementWithFixedPosition()
        {
            driver.Url = EnvironmentManager.Instance.UrlBuilder.WhereIs("coordinates_tests/page_with_fixed_element.html");
            Assert.AreEqual(0, GetLocationInViewPort(By.Id("fixed")).Y);
            Assert.AreEqual(0, GetLocationOnPage(By.Id("fixed")).Y);

            driver.FindElement(By.Id("bottom")).Click();
            Assert.AreEqual(0, GetLocationInViewPort(By.Id("fixed")).Y);
            Assert.Greater(GetLocationOnPage(By.Id("fixed")).Y, 0);
        }

        private Point GetLocationInViewPort(By locator)
        {
            IWebElement element = driver.FindElement(locator);
            return ((ILocatable)element).Coordinates.LocationInViewport;
        }

        private Point GetLocationOnPage(By locator)
        {
            IWebElement element = driver.FindElement(locator);
            return ((ILocatable)element).Coordinates.LocationInDom;
        }
    }
}

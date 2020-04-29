using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NUnit.Framework;
using NSubstitute;
using System.Threading;

namespace Microwave.Test.Integration
{
    class ST1_TimerTests
    {

        CookController _uut;
        MicrowaveOvenClasses.Boundary.Timer _timer;
        IDisplay _display;
        IPowerTube _powerTube;
        IUserInterface _ui;

        [SetUp]
        public void Setup()
        {
            _timer = new MicrowaveOvenClasses.Boundary.Timer();
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();
            _ui = Substitute.For<IUserInterface>();

            _uut = new CookController(_timer, _display, _powerTube, _ui);
        }

        [TestCase(0, 1)]
        [TestCase(50, 1)]
        [TestCase(0, 3)]
        [TestCase(700, 3)]
        public void CookControllerTest(int power, int seconds)
        {
            _uut.StartCooking(0, 1000*seconds);
            Thread.Sleep(1000*seconds);

            Received.InOrder(() =>
            {
                _powerTube.TurnOn(power);

                for (int i = seconds - 1; i >= 0; i--)
                    _display.ShowTime(i / 60, i % 60);

                _powerTube.TurnOff();
                _ui.CookingIsDone();
            });
        }

        [TestCase(0, 1)]
        [TestCase(50, 1)]
        [TestCase(0, 3)]
        [TestCase(700, 3)]
        public void CookControllerTestExt3(int power, int seconds)
        {
            _uut.StartCooking(0, 1000 * seconds);

            int abortAfterms = (1000 * seconds) / 2;

            Thread.Sleep(abortAfterms);

            _uut.Stop();

            Thread.Sleep((seconds * 1000) - abortAfterms);

            Received.InOrder(() =>
            {
                _powerTube.TurnOn(power);

                for (int i = (abortAfterms/1000) - 1; i >= 0; i--)
                    _display.ShowTime(i / 60, i % 60);

                _powerTube.TurnOff();
            });


        }

        [TestCase(0, 1)]
        [TestCase(50, 1)]
        [TestCase(0, 3)]
        [TestCase(700, 3)]
        public void CookControllerTestExt4(int power, int seconds)
        {
            _uut.StartCooking(0, 1000 * seconds);

            int abortAfterms = (1000 * seconds) / 2;

            Thread.Sleep(abortAfterms);

            _uut.Stop();

            Thread.Sleep((seconds*1000) - abortAfterms);

            Received.InOrder(() =>
            {
                _powerTube.TurnOn(power);

                for (int i = (abortAfterms / 1000) - 1; i >= 0; i--)
                    _display.ShowTime(i / 60, i % 60);

                _powerTube.TurnOff();
            });


        }
    }
}

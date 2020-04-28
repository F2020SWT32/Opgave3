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

        [Test]
        public void CookControllerTest()
        {
            _uut.StartCooking(0, 2);
            Thread.Sleep(1000);

            Received.InOrder(() =>
            {
                _powerTube.TurnOn(0);
                _display.ShowTime(0, 0);
                _powerTube.TurnOff();
                _ui.CookingIsDone();
            });
        }
    }
}

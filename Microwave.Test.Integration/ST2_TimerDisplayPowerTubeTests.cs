using System;
using NUnit.Framework;
using NSubstitute;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using System.Threading;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Test.Integration
{
    class ST2_TimerDisplayPowerTubeTests
    {

        Timer           _timer;
        Display          _display;
        PowerTube      _powerTube;
        CookController  _uut;

        IOutput         _output;
        IUserInterface  _ui;


        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();
            _ui     = Substitute.For<IUserInterface>();

            _timer          = new Timer();
            _display        = new Display(_output);
            _powerTube      = new PowerTube(_output);
            _uut            = new CookController(_timer, _display, _powerTube, _ui);
        }

        [TestCase(50, 1)]
        [TestCase(700, 1)]
        [TestCase(50, 3)]
        [TestCase(700, 3)]
        public void CookControllerTest(int power, int seconds)
        {
            _uut.StartCooking(power, seconds);
            Thread.Sleep(100 + 1000 * seconds);

            Received.InOrder(() =>
            {
                _output.OutputLine($"PowerTube works with {power}");

                for (int i = seconds - 1; i >= 0; i--)
                {
                    int sec = i % 60;
                    int min = i / 60;
                    _output.OutputLine($"Display shows: {min:D2}:{sec:D2}");
                }

                _output.OutputLine("PowerTube turned off");
                _ui.CookingIsDone();
            });
        }

        [TestCase(50, 1)]
        [TestCase(700, 1)]
        [TestCase(50, 3)]
        [TestCase(700, 3)]
        public void CookControllerTestExt3(int power, int seconds)
        {
            _uut.StartCooking(power, seconds);

            int abortAfterms = (1000 * seconds) / 2;

            Thread.Sleep(abortAfterms);

            _uut.Stop();

            Thread.Sleep(100 + (seconds * 1000) - abortAfterms);

            Received.InOrder(() =>
            {
                _output.OutputLine($"PowerTube works with {power}");

                for (int i = seconds - 1; i >= Math.Ceiling(abortAfterms / 1000.0); i--)
                {
                    int sec = i % 60;
                    int min = i / 60;
                    _output.OutputLine($"Display shows: {min:D2}:{sec:D2}");
                }

                _output.OutputLine("PowerTube turned off");
            });

        }

        [TestCase(50, 1)]
        [TestCase(700, 1)]
        [TestCase(50, 3)]
        [TestCase(700, 3)]
        public void CookControllerTestExt4(int power, int seconds)
        {
            _uut.StartCooking(power, seconds);

            int abortAfterms = (1000 * seconds) / 2;

            Thread.Sleep(abortAfterms);

            _uut.Stop();

            Thread.Sleep(100 + (seconds * 1000) - abortAfterms);

            Received.InOrder(() =>
            {
                _output.OutputLine($"PowerTube works with {power}");

                for (int i = seconds - 1; i >= Math.Ceiling(abortAfterms / 1000.0); i--)
                {
                    int sec = i % 60;
                    int min = i / 60;
                    _output.OutputLine($"Display shows: {min:D2}:{sec:D2}");
                }

                _output.OutputLine("PowerTube turned off");
            });

        }
    }
}

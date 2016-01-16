﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Prefetch.Test
{
    [TestFixture]
    public class TestPrefetchMain
    {
        [Test]
        public void InvalidFileShouldThrowException()
        {
            Action action = () => Prefetch.Open(@"..\..\TestFiles\Bad\notAPrefetch.pf");

            action.ShouldThrow<Exception>().WithMessage("Invalid signature! Should be 'SCCA'");
        }

        [Test]
        public void Windows10ShouldHaveVersionNumber30()
        {
            foreach (var file in Directory.GetFiles(@"..\..\TestFiles\Win10","*.pf"))
            {
                var pf = Prefetch.Open(file);
                pf.SourceFilename.Should().Be(file);
                pf.Header.Version.Should().Be(Version.Win10);
            }
        }

        [Test]
        public void WindowsXPShouldHaveVersionNumber17()
        {
            foreach (var file in Directory.GetFiles(@"..\..\TestFiles\XPPro", "*.pf"))
            {
                var pf = Prefetch.Open(file);

                pf.SourceFilename.Should().Be(file);

                pf.Header.Version.Should().Be(Version.WinXpOrWin2K3);
            }
        }

        [Test]
        public void Windows2k3ShouldHaveVersionNumber17()
        {
            foreach (var file in Directory.GetFiles(@"..\..\TestFiles\Win2k3", "*.pf"))
            {
                var pf = Prefetch.Open(file);

                pf.SourceFilename.Should().Be(file);

                pf.Header.Version.Should().Be(Version.WinXpOrWin2K3);
            }
        }

        [Test]
        public void SignatureShouldBeSCCA()
        {
            foreach (var file in Directory.GetFiles(@"..\..\TestFiles\XPPro", "*.pf"))
            {
                var pf = Prefetch.Open(file);

                pf.Header.Signature.Should().Be("SCCA");
            }

            foreach (var file in Directory.GetFiles(@"..\..\TestFiles\Win10", "*.pf"))
            {
                var pf = Prefetch.Open(file);

                pf.Header.Signature.Should().Be("SCCA");
            }
        }



    }
}
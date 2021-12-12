﻿using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace Prefetch.Test;

[TestFixture]
public class TestPrefetchMain
{
    public static string BadPath = @"..\..\TestFiles\Bad";
    public static string Win7Path = @"..\..\TestFiles\Win7";
    public static string Win10Path = @"..\..\TestFiles\Win10";
    public static string WinXpPath = @"..\..\TestFiles\XPPro";
    public static string Win8xPath = @"..\..\TestFiles\Win8x";
    public static string Win2k3Path = @"..\..\TestFiles\Win2k3";
    public static string Win2012Path = @"..\..\TestFiles\Win2012";
    public static string WinVistaPath = @"..\..\TestFiles\Vista";
    public static string Win2012R2Path = @"..\..\TestFiles\Win2012R2";

    private readonly List<string> _allPaths = new List<string>
    {
        Win10Path,
        WinXpPath,
        WinVistaPath,
        Win7Path,
        Win8xPath,
        Win2k3Path,
        Win2012Path,
        Win2012R2Path
    };


    [Test]
    public void InvalidFileShouldThrowException()
    {
        var badFile = Path.Combine(BadPath, "notAPrefetch.pf");
        Action action = () => PrefetchFile.Open(badFile);

            

        action.Should().Throw<Exception>().WithMessage("Invalid signature! Should be 'SCCA'");
    }

    [Test]
    public void OneOff()
    {
        var f = @"C:\Temp\500sru\POWERSHELL.EXE-767FB1AE.pf";

        var ms = new FileStream(f,FileMode.Open,FileAccess.Read);
        
        var pf = PrefetchFile.Open(ms,"foo");


        var aa = PrefetchFile.Open(f);


        pf.RunCount.Should().BeGreaterThan(0);
        pf.RunCount.Should().Be(3);

        //   pf.Should().NotBe(null);

    }

    [Test]
    public void SignatureShouldBeSCCA()
    {
        foreach (var allPath in _allPaths)
        {
            foreach (var file in Directory.GetFiles(allPath, "*.pf"))
            {
                var pf = PrefetchFile.Open(file);

                pf.Should().NotBe(null);

                pf.Filenames.Count.Should().Be(pf.FileMetricsCount);

                pf.VolumeCount.Should().Be(pf.VolumeInformation.Count);

                pf.Header.Signature.Should().Be("SCCA");
            }
        }
    }
}
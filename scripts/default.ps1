$framework = '4.0'

. .\psake_ext.ps1

properties {
    $versionMajor = 1
    $versionMinor = 0
    $versionBuild = 0
    $year = (Get-Date).year
    $fullVersion = "$versionMajor.$versionMinor.$versionBuild.1"

    $configuration = "Release"

    $baseDir = ".\.."
    $srcDir = "$baseDir\src"
    $testDir = "$baseDir\test"
    $outputDir = "$baseDir\output"
    $collectDir = "$outputDir\collect"
    $scriptDir = "$baseDir\scripts"

    $solutionFile = "$baseDir\Uncas.WebTester.sln"
    $nunitFolder = "$baseDir\packages\NUnit.2.5.10.11092\tools"
    $nunitExe = "$nunitFolder\nunit-console.exe"
    $nugetExe = "$baseDir\.nuget\nuget.exe"

    $websitePort = "963"
    $websitePath = "$baseDir\src\Uncas.WebTester.Web"
    $websiteName = "WebTesterWeb"
}

task default -depends Test,Pack,FxCop

task Clean {
    if (Test-Path $outputDir)
    {
        rmdir -force -recurse $outputDir
    }
}

task Initialize-ConfigFiles {
    $sourceFile = "$baseDir\config\IntegrationTests.appSettings.config.template"
    $targetFile = "$baseDir\test\Uncas.WebTester.Tests.Integration\App.appSettings.config"
    (cat $sourceFile) -replace '@WebsitePort@', "$websitePort" > $targetFile
}

task Init -depends Clean,Initialize-ConfigFiles {
    if (!(Test-Path $outputDir))
    {
        mkdir $outputDir
    }
    if (!(Test-Path $collectDir))
    {
        mkdir $collectDir
    }

    Generate-Assembly-Info `
        -file "$baseDir\src\VersionInfo.cs" `
        -company "Uncas" `
        -product "Uncas.WebTester" `
        -version "$versionMajor.$versionMinor.$versionBuild" `
        -copyright "Copyright (c) $year, Ole Lynge Sørensen"
}

task Compile -depends Init {
    msbuild $solutionFile /p:Configuration=$configuration
}

task FxCop -depends Compile {
    $fxcopOutput = "$outputDir\fxcopresults.xml"
    if (Test-Path $fxcopOutput)
    {
        Remove-Item $fxcopOutput
    }
    $fxcop = "C:\Program Files (x86)\Microsoft FxCop 10.0\FxCopCmd.exe"
    & $fxcop "/p:$baseDir\WebTester.FxCop" "/o:${fxcopOutput}" /s
    if (Test-Path $fxcopOutput)
    {
        "FxCop errors encountered"
        $fxcopDetails = Get-Content $fxcopOutput
        $fxcopDetails
        throw "FxCop errors encountered"
    }
}

task Test -depends Compile {
    Run-Test "Uncas.WebTester.Tests.Unit" $outputDir
    Run-Test "Uncas.WebTester.Tests.Integration" $outputDir
}

task Collect -depends Compile {
    $files = @()
    $files += "$srcDir\Uncas.WebTester.ConsoleApp\bin\Release\Autofac.dll"
    $files += "$srcDir\Uncas.WebTester.ConsoleApp\bin\Release\Uncas.WebTester.ConsoleApp.exe"
    $files += "$srcDir\Uncas.WebTester.ConsoleApp\bin\Release\Uncas.WebTester.ConsoleApp.exe.config"
    $files += "$srcDir\Uncas.WebTester.NUnitRunner\bin\Release\Uncas.WebTester.NUnitRunner.dll"
    $files += "$srcDir\Uncas.WebTester.NUnitRunner\bin\Release\nunit.framework.dll"
    $files += "$srcDir\Uncas.WebTester\bin\Release\HtmlAgilityPack.dll"
    $files += "$srcDir\Uncas.WebTester\bin\Release\Uncas.WebTester.dll"
    $files += "$testDir\Uncas.WebTester.Tests.SimpleTestProject\bin\Release\Uncas.WebTester.Tests.SimpleTestProject.dll"
    $files += "$testDir\Uncas.WebTester.Tests.SimpleTestProject\bin\Release\Uncas.WebTester.Tests.SimpleTestProject.dll.config"
    copy $files $collectDir
}

task Pack -depends Collect {
    & $nugetExe pack "$scriptDir\icrawl.nuspec" -Version $script:fullVersion -OutputDirectory $outputDir
}

task Publish -depends FxCop,Test,Pack {
    & $nugetExe push "$outputDir\icrawl.$script:fullVersion.nupkg"
}

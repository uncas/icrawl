$framework = '4.0'

. .\psake_ext.ps1

properties {
    $version_major = 1
    $version_minor = 0
    $version_build = 0
    $year = (Get-Date).year
    $full_version = "$version_major.$version_minor.$version_build.1"

    $configuration = "Release"

    $base_dir = ".\.."
    $src_dir = "$base_dir\src"
    $test_dir = "$base_dir\test"
    $output_dir = "$base_dir\output"
    $collect_dir = "$output_dir\collect"

    $solution_file = "$base_dir\Uncas.WebTester.sln"
    $nunit_folder = "$base_dir\packages\NUnit.2.5.10.11092\tools"
    $nunit_exe = "$nunit_folder\nunit-console.exe"
    $nuget_exe = "$base_dir\.nuget\nuget.exe"
}

task default -depends Pack

task Init {
    if (Test-Path $output_dir)
    {
        rmdir -force -recurse $output_dir
    }
    if (!(Test-Path $output_dir))
    {
        mkdir $output_dir
    }
    if (!(Test-Path $collect_dir))
    {
        mkdir $collect_dir
    }

    Generate-Assembly-Info `
        -file "$base_dir\src\VersionInfo.cs" `
        -company "Uncas" `
        -product "Uncas.WebTester" `
        -version "$version_major.$version_minor.$version_build" `
        -copyright "Copyright (c) $year, Ole Lynge Sørensen"
}

task Compile -depends Init {
    msbuild $solution_file /p:Configuration=$configuration
}

task Test -depends Compile {
    Run-Test "Uncas.WebTester.Tests.Unit" $output_dir
    Run-Test "Uncas.WebTester.Tests.Integration" $output_dir
}

task Collect -depends Test {
    copy "$src_dir\Uncas.WebTester.ConsoleApp\bin\Release\Autofac.dll" $collect_dir
    copy "$src_dir\Uncas.WebTester\bin\Release\HtmlAgilityPack.dll" $collect_dir
    copy "$src_dir\Uncas.WebTester.NUnitRunner\bin\Release\nunit.framework.dll" $collect_dir
    copy "$src_dir\Uncas.WebTester.ConsoleApp\bin\Release\Uncas.WebTester.ConsoleApp.exe" $collect_dir
    copy "$src_dir\Uncas.WebTester.ConsoleApp\bin\Release\Uncas.WebTester.ConsoleApp.exe.config" $collect_dir
    copy "$src_dir\Uncas.WebTester\bin\Release\Uncas.WebTester.dll" $collect_dir
    copy "$src_dir\Uncas.WebTester.NUnitRunner\bin\Release\Uncas.WebTester.NUnitRunner.dll" $collect_dir
    copy "$test_dir\Uncas.WebTester.Tests.SimpleTestProject\bin\Release\Uncas.WebTester.Tests.SimpleTestProject.dll" $collect_dir
    copy "$test_dir\Uncas.WebTester.Tests.SimpleTestProject\bin\Release\Uncas.WebTester.Tests.SimpleTestProject.dll.config" $collect_dir
}

task Pack -depends Collect {
    & $nuget_exe pack "$base_dir\icrawl.nuspec" -Version $script:full_version -OutputDirectory $output_dir
}

task Publish -depends Pack {
    & $nuget_exe push "$output_dir\icrawl.$script:full_version.nupkg"
}
